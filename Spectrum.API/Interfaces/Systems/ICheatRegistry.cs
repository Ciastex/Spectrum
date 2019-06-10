namespace Spectrum.API.Interfaces.Systems
{
    public interface ICheatRegistry
    {
        bool AnyCheatsEnabled { get; }

        void Enable(string key);
        void Disable(string key);
        bool IsCheatEnabled(string key);
    }
}
