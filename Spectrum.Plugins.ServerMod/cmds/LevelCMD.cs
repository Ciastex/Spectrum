using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectrum.Plugins.ServerMod.cmds
{
    class LevelCMD : cmd
    {
        public override string name { get { return "level"; } }
        public override PermType perm { get { return PermType.ALL; } }
        public override bool canUseAsClient { get { return true; } }

        public override void help(ClientPlayerInfo p)
        {
            Utilities.sendMessage("!level [name]: Find a level who have that keyword on his name");
            Utilities.sendMessage("!level [filter] : Use filters to find a level");
            Utilities.sendMessage("Valid filters : -mode -m -name -n -author -a -index -i -page -p -last -l");
            Utilities.sendMessage("The level must be know by the server to be show");
        }

        public override void use(ClientPlayerInfo p, string message)
        {
            if(message == "")
            {
                help(p);
                return;
            }

            if (Utilities.isOnLobby())
            {
                Utilities.sendMessage("You can't search on levels here");
                return;
            }

            var m = LevelList.extractModifiers(message);
            var lvls = LevelList.levels(m);
            LevelList.printLevels(lvls, m.page, 10, m.index.Count == 0);
        }
    }
}
