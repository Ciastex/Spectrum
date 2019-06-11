using Octokit;
using Spectrum.Resonator.Enums;
using Spectrum.Resonator.Infrastructure.Markers.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.Resonator.Services.Interfaces
{
    public interface ISpectrumInstallerService : IService
    {
        Task<List<Release>> DownloadReleaseList();
        Task<string> DownloadPackage(string assetUrl);
        Task<PrismTerminationReason> InstallSpectrum(string distancePath, string customPrismArguments = null);
        Task UninstallSpectrum(string distancePath);

        string GetRegisteredDistanceInstallationPath();
        void ExtractPackage(string sourcePath, string distancePath);
    }
}
