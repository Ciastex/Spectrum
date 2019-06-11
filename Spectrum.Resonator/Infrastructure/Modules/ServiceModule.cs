using Autofac;
using Spectrum.Resonator.Infrastructure.Markers.Interfaces;
using System.Linq;

namespace Spectrum.Resonator.Infrastructure.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly)
                   .Where(type => type.IsAssignableTo<IService>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
