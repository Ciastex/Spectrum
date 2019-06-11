using Autofac;
using DevExpress.Mvvm;
using Spectrum.Resonator.Infrastructure;
using Spectrum.Resonator.Infrastructure.Modules;
using System.Windows;

namespace Spectrum.Resonator
{
    public partial class App : Application
    {
        private static IContainer Container { get; set; }
        private ContainerBuilder ContainerBuilder { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Messenger.Default = new Messenger(true);

            ContainerBuilder = new ContainerBuilder();
            ContainerBuilder.RegisterModule<ViewModelModule>();
            ContainerBuilder.RegisterModule<ServiceModule>();
            ContainerBuilder.RegisterModule<ProviderModule>();

            Container = ContainerBuilder.Build();

            Resolver.Instance.Initialize(Container);
        }
    }
}
