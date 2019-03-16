using System;
using UnityEngine;

namespace Spectrum.API.Events.EventArgs
{
    public class CheatStateFailureEventArgs : System.EventArgs
    {
        public NetworkPlayer Sender { get; }
        public Exception Exception { get; }

        public string FailingData { get; }

        public CheatStateFailureEventArgs(NetworkPlayer sender, Exception exception, string failingData)
        {
            Sender = sender;
            Exception = exception;

            FailingData = failingData;
        }
    }
}
