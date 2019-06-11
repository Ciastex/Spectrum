using Spectrum.API.Security;
using UnityEngine;

namespace Spectrum.API.Events.EventArgs
{
    public class CheatStateInfoEventArgs : System.EventArgs
    {
        public CheatStateInfo CheatStateInfo { get; }
        public NetworkPlayer Sender { get; }

        public CheatStateInfoEventArgs(CheatStateInfo cheatStateInfo, NetworkPlayer sender)
        {
            CheatStateInfo = cheatStateInfo;
            Sender = sender;
        }
    }
}
