using DevExpress.Mvvm;
using System;
using System.Windows.Markup;

namespace Spectrum.Resonator.Infrastructure.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(ViewModelBase))]
    public class DataContextSource : MarkupExtension
    {
        [ConstructorArgument("viewModelType")]
        public Type ViewModelType { get; set; }

        public DataContextSource(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Resolver.Instance.Resolve(ViewModelType);
        }
    }
}
