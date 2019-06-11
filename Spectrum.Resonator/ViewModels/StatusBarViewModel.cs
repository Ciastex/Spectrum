using DevExpress.Mvvm;
using Spectrum.Resonator.Infrastructure.Messaging;

namespace Spectrum.Resonator.ViewModels
{
    public class StatusBarViewModel : ViewModelBase
    {
        public string ActionInfo { get; set; }

        public StatusBarViewModel()
        {
            Messenger.Default.Register<SetStatusBarActionMessage>(this, (msg) => ActionInfo = msg.Action);
        }
    }
}
