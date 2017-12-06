using System;

namespace Spectrum.Interop.Game
{
    public class MainMenu
    {
        public static event EventHandler Loaded;

        static MainMenu()
        {
            Events.MainMenu.Initialized.Subscribe(data =>
            {
                Loaded?.Invoke(default(object), System.EventArgs.Empty);
            });
        }
    }
}
