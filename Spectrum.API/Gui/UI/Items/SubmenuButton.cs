using Spectrum.API.Gui.Menu;
using Spectrum.API.Gui.Data;

namespace Spectrum.API.Gui.UI.Items
{
    public class SubmenuButton : MenuItem
    {
        public MenuTree MenuTree { get; set; }

        #region constructors
        public SubmenuButton(MenuDisplayMode mode, string id, string name, MenuTree menu, string description = null) : base(mode, id, name, description)
        {
            this.MenuTree = menu;
        }
        #endregion

        public override void Tweak(SpectrumMenu menu)
        {
            menu.TweakAction(this.Name, () =>
            {
                MenuSystem.ShowMenu(MenuTree, menu, 0);
                base.Tweak(menu);
            }, this.Description);
        }
    }
}
