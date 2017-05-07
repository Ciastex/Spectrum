using Spectrum.Plugins.ServerMod.cmds;
using System.Collections.Generic;

namespace Spectrum.Plugins.ServerMod
{
    class cmdlist
    {
        private List<cmd> cmds = new List<cmd>();

        public cmdlist()
        {
            cmds.Add(new AutoCMD());
            cmds.Add(new AutoSpecCMD());
            cmds.Add(new ClearCMD());
            cmds.Add(new CountdownCMD());
            cmds.Add(new DateCMD());
            cmds.Add(new DelCMD());
            cmds.Add(new ForceStartCMD());
            cmds.Add(new HelpCMD());
            cmds.Add(new LevelCMD());
            cmds.Add(new ListCMD());
            cmds.Add(new LoadCMD());
            //cmds.Add(new NameCMD()); // not supported
            cmds.Add(new PlayCMD());
            cmds.Add(new PlaylistCMD());
            cmds.Add(new PluginCMD());
            cmds.Add(new RipCMD());
            cmds.Add(new SaveCMD());
            cmds.Add(new ScoresCMD());
            cmds.Add(new ServerCMD());
            cmds.Add(new SettingsCMD());
            cmds.Add(new ShuffleCMD());
            cmds.Add(new SpecCMD());
            cmds.Add(new TimelimitCMD());
            cmds.Add(new WinCMD());
        }

        public cmd getCommand(string name)
        {
            if (name.Length == 0)
                return null;
            name = name.ToLower();

            foreach(cmd c in cmds)
                if (c.name.Equals(name))
                    return c;
            return null;
        }

        public List<string> commands()
        {
            List<string> l = new List<string>();
            foreach(var c in cmds)
                l.Add(c.name);
            return l;
        }
    }
}
