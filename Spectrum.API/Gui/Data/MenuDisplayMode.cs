namespace Spectrum.API.Gui.Data
{
    public enum MenuDisplayMode
    {
        None = 0,
        MainMenu = 1,
        PauseMenu = 2,
        Any = MainMenu | PauseMenu
    }

    public static class MenuDisplayModeEx
    {
        public static MenuDisplayMode Make(bool mainmenu, bool pausemenu)
        {
            MenuDisplayMode result = MenuDisplayMode.None;
            result |= mainmenu ? MenuDisplayMode.MainMenu : 0;
            result |= pausemenu ? MenuDisplayMode.PauseMenu : 0;
            return result;
        }
    }
}
