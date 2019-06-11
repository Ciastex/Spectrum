using System;

namespace Spectrum.API.GUI.Data
{
    [Flags]
    public enum MenuDisplayMode
    {
        None,
        MainMenu,
        PauseMenu,
        Both = MainMenu | PauseMenu
    }
}
