using Microsoft.WindowsAPICodePack.Dialogs;
using Spectrum.Resonator.Models;
using Spectrum.Resonator.Services.Interfaces;

namespace Spectrum.Resonator.Services
{
    public class BrowseDialogService : IBrowseDialogService
    {
        public string Browse(BrowseDialogSettings settings)
        {
            var commonFileDialog = new CommonOpenFileDialog
            {
                Title = settings.Title,
                IsFolderPicker = settings.IsFolderPicker,
                Multiselect = false,
                ShowPlacesList = true,
                AllowNonFileSystemItems = false,
                EnsurePathExists = true
            };

            var result = commonFileDialog.ShowDialog(settings.Owner);

            if (result == CommonFileDialogResult.Ok)
                return commonFileDialog.FileName;

            return string.Empty;
        }
    }
}
