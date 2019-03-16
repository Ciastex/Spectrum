using Spectrum.API.IPC;

namespace Spectrum.API.Events.EventArgs
{
    public class PluginInitializationEventArgs : System.EventArgs
    {
        public PluginInfo Plugin { get; }
        public bool Last { get; }

        public PluginInitializationEventArgs(PluginInfo plugin, bool last)
        {
            Plugin = plugin;
            Last = last;
        }
    }
}
