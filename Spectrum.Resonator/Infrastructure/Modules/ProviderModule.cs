using Autofac;
using Spectrum.Resonator.Infrastructure.Markers.Interfaces;
using System.Linq;

namespace Spectrum.Resonator.Infrastructure.Modules
{
    public class ProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly)
                   .Where(type => type.IsAssignableTo<IProvider>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
