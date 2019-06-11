using System.Windows;

namespace Spectrum.Resonator.Models
{
    public class BrowseDialogSettings
    {
        public bool IsFolderPicker { get; set; }
        public string Title { get; set; }
        public Window Owner { get; set; }
    }
}
