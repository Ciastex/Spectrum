using Events;
using Spectrum.API.Gui.Menu;

namespace Spectrum.API.Events.Gui
{
    public class MenuOpened : StaticEvent<MenuOpened.Data>
    {
        public struct Data
        {
            public SpectrumMenu menu;

            public Data(SpectrumMenu menu)
            {
                this.menu = menu;
            }
        }
    }
}
