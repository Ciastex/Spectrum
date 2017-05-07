using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spectrum.Plugins.ServerMod.cmds
{
    class PlayCMD : cmd
    {
        public static bool playersCanAddMap = false;
        public static bool addOneMapOnly = true;

        public override string name { get { return "play"; } }
        public override PermType perm { get { return playersCanAddMap ? PermType.ALL : PermType.HOST; } }
        public override bool canUseAsClient { get { return false; } }

        public override void help(ClientPlayerInfo p)
        {
            Utilities.sendMessage("!play [lvl name]: Adds a level to the playlist as the next to be played.");
            Utilities.sendMessage("!play [filter] : Use filters to find a level");
            Utilities.sendMessage("Valid filters : -mode -m -name -n -author -a -index -i -last -l -all");
            Utilities.sendMessage("The level must be know by the server to be show");
        }

        public override void use(ClientPlayerInfo p, string message)
        {
            if (message == "")
            {
                help(p);
                return;
            }

            if (Utilities.isOnLobby())
            {
                Utilities.sendMessage("You can't set the next level here");
                return;
            }

            if(G.Sys.GameManager_.ModeID_ == GameModeID.Trackmogrify)
            {
                Utilities.sendMessage("You can't manage the playlist in trackmogrify");
                return;
            }

            Modifiers m = LevelList.extractModifiers(message);
            var list = LevelList.levels(m);

            if(list.Count == 0)
            {
                Utilities.sendMessage("Can't find a level with the filter '" + message + "'.");
                return;
            }

            if(!m.all && list.Count() > 1 && m.index.Count == 0)
            {
                LevelList.printLevels(list, m.page, 10, true);
                //LevelList.printLevels(list, 10, true);
                return;
            }

            if(playersCanAddMap && !p.IsLocal_)
            {
                var value = list[0];
                list.Clear();
                list.Add(value);
            }

            LevelPlaylist playlist = new LevelPlaylist();
            playlist.Copy(G.Sys.GameManager_.LevelPlaylist_);

            var currentPlaylist = playlist.Playlist_;
            int index = G.Sys.GameManager_.LevelPlaylist_.Index_;
            Utilities.Shuffle(list, new Random());
            foreach (var lvl in list)
                currentPlaylist.Insert(index + 1, lvl);
            G.Sys.GameManager_.LevelPlaylist_.Clear();

            foreach (var lvl in currentPlaylist)
                G.Sys.GameManager_.LevelPlaylist_.Add(lvl);
            G.Sys.GameManager_.LevelPlaylist_.SetIndex(index);

            string lvlsStr = "";
            foreach (var lvl in list)
                lvlsStr += lvl.levelNameAndPath_.levelName_ + ", ";

            Utilities.sendMessage("Level(s) " + lvlsStr.Substring(0, lvlsStr.Count()-2) + " added to the playlist !");
        }
    }
}
