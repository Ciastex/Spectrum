using Spectrum.API.GUI.Menu;
using Spectrum.API.GUI.Data;

namespace Spectrum.API.GUI.Controls
{
    public class SubmenuButton : MenuItem
    {
        public MenuTree MenuTree { get; private set; }

        public SubmenuButton(MenuDisplayMode mode, string id, string name) 
            : base(mode, id, name) { }

        public SubmenuButton NavigatesTo(MenuTree menuTree)
        {
            MenuTree = menuTree;
            return this;
        }

        public override void Tweak(SpectrumMenu menu)
        {
            menu.TweakAction(Name, () =>
            {
                MenuSystem.ShowMenu(MenuTree, menu, 0);
                base.Tweak(menu);
            }, Description);
        }
    }
}
