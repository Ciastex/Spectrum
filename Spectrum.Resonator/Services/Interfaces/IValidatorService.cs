using Spectrum.Resonator.Infrastructure.Markers.Interfaces;

namespace Spectrum.Resonator.Services.Interfaces
{
    public interface IValidatorService : IService
    {
        bool ValidateFileSystemPath(string path, string validatorName);
        bool ValidateZipArchive(string path, string validatorName);
    }
}
