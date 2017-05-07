using Events;
using Events.GameMode;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spectrum.Plugins.ServerMod.cmds
{
    class AutoCMD : cmd
    {
        public static bool voteNext = false;


        const int maxRunTime = 15*60;
        const int maxtVoteValue = 3;

        bool autoMode = false;
        int index = 0;
        bool voting = false;
        Dictionary<string, int> votes = new Dictionary<string, int>();

        public override string name { get { return "auto"; } }
        public override PermType perm { get { return PermType.HOST; } }
        public override bool canUseAsClient { get { return false; } }

        public AutoCMD()
        {
            Events.ServerToClient.ModeFinished.Subscribe(data =>
            {
                onModeFinish();
            });

            Events.GameMode.Go.Subscribe(data =>
            {
                onModeStart();
            });

            Events.ClientToAllClients.ChatMessage.Subscribe(data =>
            {
                onChatEvent(data.message_);
            });
        }

        public override void help(ClientPlayerInfo p)
        {
            Utilities.sendMessage("!auto: Toggle the server auto mode.");
            Utilities.sendMessage("You must have a playlist to active the auto server");
        }

        public override void use(ClientPlayerInfo p, string message)
        {
            if(!autoMode)
            {
                autoMode = true;
                Utilities.sendMessage("Automode started !");
                if(Utilities.isOnLobby())
                    G.Sys.GameManager_.StartCoroutine(startFromLobby());
                else if(Utilities.isModeFinished())
                    G.Sys.GameManager_.StartCoroutine(waitAndGoNext());
                else onModeStart();
            }
            else
            {
                autoMode = false;
                Utilities.sendMessage("Automode stopped !");
            }
        }

        private void onChatEvent(string message)
        {
            if (!voting)
                return;

            var author = Utilities.ExtractMessageAuthor(message);
            var text = Utilities.ExtractMessageBody(message);
            var value = 0;
            if (!int.TryParse(text, out value))
                return;
            if (value < 0 || value > maxtVoteValue)
                return;
            votes[author] = value;
        }

        private void onModeFinish()
        {
            if(autoMode)
            {
                if (voteNext && G.Sys.GameManager_.LevelPlaylist_.Playlist_.Count >= maxtVoteValue)
                    G.Sys.GameManager_.StartCoroutine(voteAndGoNext());
                else G.Sys.GameManager_.StartCoroutine(waitAndGoNext());
            }
        }

        private void onModeStart()
        {
            if (!Utilities.isOnline())
                autoMode = false;
            index++;
            if (autoMode)
                G.Sys.GameManager_.StartCoroutine(waitUtilEnd());
        }

        IEnumerator waitAndGoNext()
        {
            if (!Utilities.isOnLobby())
            {
                Utilities.sendMessage("Go to next level in 10 seconds ...");
                Utilities.sendMessage("Next level is : " + Utilities.getNextLevelName());
                yield return new WaitForSeconds(10.0f);
                if (autoMode && !Utilities.isOnLobby())
                {
                    if (Utilities.isCurrentLastLevel())
                    {
                        cmd.all.getCommand("shuffle").use(null, "");
                    }
                    G.Sys.GameManager_.GoToNextLevel(true);
                }
                else autoMode = false;
            }
            else autoMode = false;
            yield return null;
        }

        IEnumerator voteAndGoNext()
        {
            if (!Utilities.isOnLobby())
            {
                voting = true;
                votes.Clear();
                if(G.Sys.GameManager_.LevelPlaylist_.Playlist_.Count - G.Sys.GameManager_.LevelPlaylist_.Index_ < maxtVoteValue)
                    cmd.all.getCommand("shuffle").use(null, "");
                Utilities.sendMessage("Vote for the next map (write [FF0000]1[-], [00FF00]2[-], [0088FF]3[-], or [FFFFFF]0[-] to restart) ! Votes end in 15 sec !");
                Utilities.sendMessage("[b][FF0000]1[-] : [FFFFFF]" + G.Sys.GameManager_.LevelPlaylist_.Playlist_[G.Sys.GameManager_.LevelPlaylist_.Index_ + 1].levelNameAndPath_.levelName_ + "[-][/b]");
                Utilities.sendMessage("[b][00FF00]2[-] : [FFFFFF]" + G.Sys.GameManager_.LevelPlaylist_.Playlist_[G.Sys.GameManager_.LevelPlaylist_.Index_ + 2].levelNameAndPath_.levelName_ + "[-][/b]");
                Utilities.sendMessage("[b][0088FF]3[-] : [FFFFFF]" + G.Sys.GameManager_.LevelPlaylist_.Playlist_[G.Sys.GameManager_.LevelPlaylist_.Index_ + 3].levelNameAndPath_.levelName_ + "[-][/b]");
                yield return new WaitForSeconds(15);

                if (autoMode && !Utilities.isOnLobby())
                {
                    int index = bestVote();
                    if(index == 0)
                        Utilities.sendMessage("Restart the current level !");
                    else Utilities.sendMessage("Level [b][FFFFFF]" + G.Sys.GameManager_.LevelPlaylist_.Playlist_[G.Sys.GameManager_.LevelPlaylist_.Index_ + index].levelNameAndPath_.levelName_ + "[-][/b] selected !");
                    voting = false;
                    yield return new WaitForSeconds(5);

                    if (autoMode && !Utilities.isOnLobby())
                    {
                        setToNextMap(index);
                        G.Sys.GameManager_.GoToNextLevel(true);
                    }
                    else autoMode = false;
                }
                else autoMode = false;
                yield return null;
            }
            else autoMode = false;
            yield return null;
        }

        IEnumerator startFromLobby()
        {
            Utilities.sendMessage("Start game in 10 seconds ...");
            yield return new WaitForSeconds(10.0f);
            if (Utilities.isOnLobby())
                G.Sys.GameManager_.GoToCurrentLevel();
            yield return null;
        }

        IEnumerator waitUtilEnd()
        {
            int currentIndex = index;
            yield return new WaitForSeconds(maxRunTime);
            if (currentIndex == index && autoMode)
            {
                Utilities.sendMessage("This map had run for 15min ...");
                Utilities.sendMessage("Finishing in 30 sec ...");
                yield return new WaitForSeconds(30);

                if (currentIndex == index && autoMode)
                {
                    StaticTargetedEvent<Finished.Data>.Broadcast(RPCMode.All, default(Finished.Data));
                }
            }
            yield return null;
        }

        int bestVote()
        {
            List<int> values = new List<int>();
            for (int i = 0 ; i <= maxtVoteValue; i++)
                values.Add(0);

            foreach(var v in votes)
            {
                if (v.Value <= maxtVoteValue && v.Value >= 0)
                    values[v.Value]++;
            }

            int maxValue = values.Max();
            for (int i = 0; i < values.Count; i++)
                if (values[i] < maxValue)
                    values[i] = -1;
            System.Random r = new System.Random();
            for(int i = 0; i < 20; i++)
            {
                int rValue = r.Next(0, values.Count);
                if (values[rValue] >= 0)
                    return rValue;
            }

            for (int i = 0; i < values.Count; i++)
                if (values[i] == maxValue)
                    return i;
            return 1;
        }

        void setToNextMap(int nextIndex)
        {
            for(int i = 1; i <= maxtVoteValue; i++)
            {
                int offset = i >= nextIndex && nextIndex != 0 ? 2 : 1;
                var item = G.Sys.GameManager_.LevelPlaylist_.Playlist_[G.Sys.GameManager_.LevelPlaylist_.Index_ + offset];
                G.Sys.GameManager_.LevelPlaylist_.Playlist_.RemoveAt(G.Sys.GameManager_.LevelPlaylist_.Index_ + offset);
                G.Sys.GameManager_.LevelPlaylist_.Playlist_.Insert(G.Sys.GameManager_.LevelPlaylist_.Index_, item);
                G.Sys.GameManager_.LevelPlaylist_.SetIndex(G.Sys.GameManager_.LevelPlaylist_.Index_ + 1);
            }
            if(nextIndex == 0)
                G.Sys.GameManager_.LevelPlaylist_.SetIndex(G.Sys.GameManager_.LevelPlaylist_.Index_ - 1);
        }
    }
}