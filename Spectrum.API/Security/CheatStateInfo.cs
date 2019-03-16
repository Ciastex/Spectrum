namespace Spectrum.API.Security
{
    public class CheatStateInfo
    {
        public bool AnyCheatsEnabled { get; }

        public CheatStateInfo(bool anyCheatsEnabled)
        {
            AnyCheatsEnabled = anyCheatsEnabled;
        }
    }
}
