using Spectrum.API.Gui.Data;
using Spectrum.API.Gui.Menu;
using System;

namespace Spectrum.API.Gui.UI.Items
{
    public class CheckBox : MenuItem
    {
        public Func<bool> GetValue { get; set; }
        public Action<bool> SetValue { get; set; }

        #region constructors
        public CheckBox(MenuDisplayMode mode, string id, string name, Func<bool> get, Action<bool> set, string description = null) : base(mode, id, name, description)
        {
            this.GetValue = get;
            this.SetValue = set;
        }

        public CheckBox(MenuDisplayMode mode, string id, string name, bool startvalue, Action<bool> set, string description = null) : this(mode, id, name, () => { return startvalue; }, set, description)
        { }
        #endregion

        public override void Tweak(SpectrumMenu menu)
        {
            menu.TweakBool(this.Name, this.GetValue(), this.SetValue, this.Description);
            base.Tweak(menu);
        }
    }
}
