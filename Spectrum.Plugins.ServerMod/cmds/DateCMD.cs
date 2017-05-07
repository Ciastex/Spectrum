using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spectrum.Plugins.ServerMod.cmds
{
    class DateCMD : cmd
    {
        public static bool playersCanAddMap = false;
        public static bool addOneMapOnly = true;

        public override string name { get { return "date"; } }
        public override PermType perm { get { return PermType.ALL; } }
        public override bool canUseAsClient { get { return true; } }

        public override void help(ClientPlayerInfo p)
        {
            Utilities.sendMessage("!date: Write the time and date.");
        }

        public override void use(ClientPlayerInfo p, string message)
        {
            Utilities.sendMessage("Current date: [FFFFFF]" + DateTime.Now.ToString() + "[-]");
        }
    }
}
