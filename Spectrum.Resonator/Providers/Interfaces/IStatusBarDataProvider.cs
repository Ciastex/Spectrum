using Spectrum.Resonator.Infrastructure.Markers.Interfaces;

namespace Spectrum.Resonator.Providers.Interfaces
{
    public interface IStatusBarDataProvider : IProvider
    {
        void SetActionInfo(string newActionInfo);
        void Reset();
    }
}
