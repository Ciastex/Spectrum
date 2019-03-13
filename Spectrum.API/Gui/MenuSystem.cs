using Spectrum.API.Gui.Data;
using Spectrum.API.Gui.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spectrum.API.Gui
{
    public static class MenuSystem
    {
        public static GameObject MenuBlueprint { get; set; }

        public static MenuTree MenuTree;

        static MenuSystem()
        {
            MenuTree = new MenuTree("menu.spectrum.main", "SpectrumSettings");
        }

        public static void ShowMenu(MenuTree entries, SpectrumMenu parent, int page)
        {
            foreach (var component in parent.PanelObject_.GetComponents<SpectrumMenu>())
                component.Destroy();

            SpectrumMenu menu = parent.PanelObject_.AddComponent<SpectrumMenu>();

            menu.PageIndex = page;

            menu.MenuPanel = MenuPanel.Create(menu.PanelObject_, true, true, false, true, false, false);

            menu.MenuPanel.onPanelPop_ += () =>
            {
                if (!G.Sys.MenuPanelManager_.panelStack_.Contains(menu.MenuPanel))
                {
                    parent.PanelObject_.SetActive(true);
                    if (menu.PageSwitching)
                        ShowMenu(entries, parent, menu.PageIndex);
                }
            };

            menu.Title = entries.Title;
            menu.Entries = entries;

            parent.PanelObject_.SetActive(false);

            G.Sys.MenuPanelManager_.Push(menu.MenuPanel);
        }

        public static MenuDisplayMode GetCurrentMode()
        {
            if (SceneManager.GetActiveScene().name.ToLower() == "mainmenu")
                return MenuDisplayMode.MainMenu;
            return MenuDisplayMode.PauseMenu;
        }
    }
}
