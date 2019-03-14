using Spectrum.API.GUI.Data;
using Spectrum.API.GUI.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spectrum.API.GUI
{
    public static class MenuSystem
    {
        public static GameObject MenuBlueprint { get; set; }
        public static MenuTree MenuTree { get; set; }

        static MenuSystem() => MenuTree = new MenuTree("menu.spectrum.main", "SpectrumSettings");

        public static void ShowMenu(MenuTree menuTree, SuperMenu parentMenu, int pageIndex)
        {
            foreach (var component in parentMenu.PanelObject_.GetComponents<SpectrumMenu>())
                component.Destroy();

            var menu = parentMenu.PanelObject_.AddComponent<SpectrumMenu>();

            menu.CurrentPageIndex = pageIndex;
            menu.MenuPanel = MenuPanel.Create(menu.PanelObject_, true, true, false, true, false, false);

            menu.MenuPanel.onPanelPop_ += () =>
            {
                if (!G.Sys.MenuPanelManager_.panelStack_.Contains(menu.MenuPanel))
                {
                    parentMenu.PanelObject_.SetActive(true);
                    if (menu.SwitchPageOnClose)
                        ShowMenu(menuTree, parentMenu, pageIndex);
                }
            };

            menu.Title = menuTree.Title;
            menu.MenuTree = menuTree;

            parentMenu.PanelObject_.SetActive(false);

            G.Sys.MenuPanelManager_.Push(menu.MenuPanel);
        }

        public static MenuDisplayMode GetCurrentDisplayMode()
        {
            if (SceneManager.GetActiveScene().name.ToLower() == "mainmenu")
                return MenuDisplayMode.MainMenu;

            return MenuDisplayMode.PauseMenu;
        }
    }
}
