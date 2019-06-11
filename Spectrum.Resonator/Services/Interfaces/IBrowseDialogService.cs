using Spectrum.Resonator.Infrastructure.Markers.Interfaces;
using Spectrum.Resonator.Models;

namespace Spectrum.Resonator.Services.Interfaces
{
    public interface IBrowseDialogService : IService
    {
        string Browse(BrowseDialogSettings browseDialogSettings);
    }
}
