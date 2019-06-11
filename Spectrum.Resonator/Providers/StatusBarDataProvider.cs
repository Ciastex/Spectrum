using DevExpress.Mvvm;
using Spectrum.Resonator.Infrastructure.Messaging;
using Spectrum.Resonator.Providers.Interfaces;

namespace Spectrum.Resonator.Providers
{
    public class StatusBarDataProvider : IStatusBarDataProvider
    {
        public void SetActionInfo(string newActionInfo)
        {
            Messenger.Default.Send(new SetStatusBarActionMessage { Action = newActionInfo });
        }

        public void Reset()
        {
            SetActionInfo(null);
        }
    }
}
