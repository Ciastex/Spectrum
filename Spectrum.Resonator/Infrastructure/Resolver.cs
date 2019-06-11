using Autofac;
using DevExpress.Mvvm;
using System;

namespace Spectrum.Resonator.Infrastructure
{
    public class Resolver
    {
        private IContainer _container;
        private static Resolver _instance;

        public static Resolver Instance => _instance ?? (_instance = new Resolver());

        public void Initialize(IContainer container)
        {
            _container = container;
        }

        public T Resolve<T>() where T: ViewModelBase
        {
            if (_container == null)
                throw new InvalidOperationException("Resolver not initialized yet.");

            return _container.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return _container.Resolve(t) ?? new object();
        }
    }
}
