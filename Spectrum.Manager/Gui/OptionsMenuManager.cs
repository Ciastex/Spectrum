using Spectrum.API.Gui;
using Spectrum.API.Gui.Data;
using Spectrum.API.Gui.UI.Items;
using Spectrum.API.Interfaces.Systems;

namespace Spectrum.Manager.Gui

{
    class OptionsMenuManager : IOptionsMenuManager
    {
        public MenuTree Menus {
            get
            {
                return MenuSystem.MenuTree;
            }
            private set
            {
                MenuSystem.MenuTree = value;
            }
        }

        public void AddMenu(bool mainmenu, bool pausemenu, MenuTree menu, string description = null)
        {
            Menus.Add(new SubmenuButton(MenuDisplayModeEx.Make(mainmenu, pausemenu), menu.Id, menu.Title, menu, description));
        }
    }
}
