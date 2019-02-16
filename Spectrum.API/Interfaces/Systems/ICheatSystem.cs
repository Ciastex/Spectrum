using Spectrum.API.Events.EventArgs;
using System;

namespace Spectrum.API.Interfaces.Systems
{
    public interface ICheatSystem
    {
        event EventHandler<CheatStateInfoEventArgs> CheatStateInfoReceived;
        event EventHandler<CheatStateFailureEventArgs> CheatStateInfoFailure;

        bool AnyCheatsEnabled { get; }
        void Enable(string key);
        void Disable(string key);
    }
}
