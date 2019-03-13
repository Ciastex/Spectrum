using Spectrum.API.Gui.Data;

namespace Spectrum.API.Interfaces.Systems
{
    public interface IOptionsMenuManager
    {
        MenuTree Menus { get; }

        void AddMenu(bool mainmenu, bool pausemenu, MenuTree menu, string description = null);
    }
}
