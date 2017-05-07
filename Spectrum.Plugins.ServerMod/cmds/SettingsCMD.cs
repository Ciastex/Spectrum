using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spectrum.Plugins.ServerMod.cmds
{
    class SettingsCMD : cmd
    {
        public override string name { get { return "settings"; } }
        public override PermType perm { get { return PermType.HOST; } }
        public override bool canUseAsClient { get { return false; } }

        public override void help(ClientPlayerInfo p)
        {
            Utilities.sendMessage("!settings reload: reload the settings for file.");
            Utilities.sendMessage("!settings vote [true/false]: allow players to vote on auto mode.");
            Utilities.sendMessage("!settings play [true/false]: allow player to add maps on the playlist.");
            Utilities.sendMessage("!settings addOne [true/false] : if enabled, allow the players to add only one map at a time.");
        }

        public override void use(ClientPlayerInfo p, string message)
        {
            if(message == "")
            {
                help(p);
                return;
            }

            var strs = message.ToLower().Trim().Split(' ');

            if (strs[0] == "reload")
                reload(p);
            else if (strs[0] == "vote")
            {
                if (strs.Length == 1)
                    help(p);
                else vote(p, strs[1]);
            }
            else if (strs[0] == "play")
            {
                if (strs.Length == 1)
                    help(p);
                else play(p, strs[1]);
            }
            else if (strs[0] == "addone")
            {
                if (strs.Length == 1)
                    help(p);
                else addOne(p, strs[1]);
            }
            else help(p);
        }

        void reload(ClientPlayerInfo p)
        {
            Entry.load();
            Utilities.sendMessage("Settings reloaded !");
        }

        void vote(ClientPlayerInfo p,  string value)
        {
            if(value == "0" || value == "false")
            {
                AutoCMD.voteNext = false;
                Utilities.sendMessage("Votes disabled !");
            }
            else if(value == "1" || value == "true")
            {
                AutoCMD.voteNext = true;
                Utilities.sendMessage("Votes enabled !");
            }
            else
            {
                help(p);
                return;
            }

            Entry.save();
        }

        void play(ClientPlayerInfo p, string value)
        {
            if (value == "0" || value == "false")
            {
                PlayCMD.playersCanAddMap = false;
                Utilities.sendMessage("Players can't add maps now !");
            }
            else if (value == "1" || value == "true")
            {
                PlayCMD.playersCanAddMap = true;
                Utilities.sendMessage("Players are now allowed to add maps !");
            }
            else
            {
                help(p);
                return;
            }

            Entry.save();
        }

        void addOne(ClientPlayerInfo p, string value)
        {
            if (value == "0" || value == "false")
            {
                PlayCMD.addOneMapOnly = false;
                Utilities.sendMessage("Multiple add allowed to players !");
            }
            else if (value == "1" || value == "true")
            {
                PlayCMD.addOneMapOnly = true;
                Utilities.sendMessage("Players can only add one map now !");
            }
            else
            {
                help(p);
                return;
            }

            Entry.save();
        }
    }
}
