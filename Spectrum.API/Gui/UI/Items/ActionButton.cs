using Spectrum.API.Gui.Data;
using Spectrum.API.Gui.Menu;
using System;

namespace Spectrum.API.Gui.UI.Items
{
    public class ActionButton : MenuItem
    {
        public Action OnClick { get; set; }

        #region constructors
        public ActionButton(MenuDisplayMode mode, string id, string name, Action action, string description = null) : base(mode, id, name, description)
        {
            this.OnClick = action;
        }
        #endregion

        public override void Tweak(SpectrumMenu menu)
        {
            menu.TweakAction(this.Name, this.OnClick, this.Description);
            base.Tweak(menu);
        }
    }
}
