using Autofac;
using DevExpress.Mvvm;
using System.Linq;

namespace Spectrum.Resonator.Infrastructure.Modules
{
    public class ViewModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly)
                   .Where(type => type.IsAssignableTo<ViewModelBase>())
                   .AsSelf()
                   .InstancePerDependency();
        }
    }
}
