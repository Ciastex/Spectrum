using Spectrum.API.Events.EventArgs;
using System;

namespace Spectrum.API.Interfaces.Systems
{
    public interface ICheatRegistry
    {
        bool AnyCheatsEnabled { get; }
        void Enable(string key);
        void Disable(string key);
    }
}
