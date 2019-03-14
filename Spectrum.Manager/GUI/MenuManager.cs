using Spectrum.API;
using Spectrum.API.GUI;
using Spectrum.API.GUI.Controls;
using Spectrum.API.GUI.Data;
using Spectrum.API.Interfaces.Systems;
using Spectrum.API.Logging;
using System;

namespace Spectrum.Manager.GUI
{
    public class MenuManager : IMenuManager
    {
        private Logger Log { get; }

        public MenuManager()
        {
            Log = new Logger(Defaults.MenuManagerLogFileName) { WriteToConsole = true };
        }

        public MenuTree Menus
        {
            get => MenuSystem.MenuTree;
            private set => MenuSystem.MenuTree = value;
        }

        public void AddMenu(MenuDisplayMode displayMode, MenuTree menuTree, string description = null)
        {
            try
            {
                Menus.Add(new SubmenuButton(displayMode, menuTree.Id, menuTree.Title)
                    .NavigatesTo(menuTree)
                    .WithDescription(description)
                );

                Log.Info($"Added new menu tree: '{menuTree.Id}', '{menuTree.Title}'...");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to add the menu tree: '{menuTree.Id}', '{menuTree.Title}'.");
                Log.Exception(ex);
            }
        }
    }
}
