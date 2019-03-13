using Spectrum.API.Gui.Data;
using Spectrum.API.Gui.Menu;
using System;

namespace Spectrum.API.Gui.UI.Items
{
    public class DecimalSlider : MenuItem
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public Func<float> GetValue { get; set; }
        public Action<float> SetValue { get; set; }
        public float DefaultValue { get; set; }

        #region constructors
        public DecimalSlider(MenuDisplayMode mode, string id, string name, float min, float max, float cursor, Func<float> get, Action<float> set, string description = null) : base(mode, id, name, description)
        {
            this.Min = min;
            this.Max = max;
            this.GetValue = get;
            this.SetValue = set;
            this.DefaultValue = cursor;
        }

        public DecimalSlider(MenuDisplayMode mode, string id, string name, float startvalue, Action<float> set, string description = null) : this(mode, id, name, 0.0f, 1.0f, float.NaN, () => { return startvalue; }, set, description)
        { }

        public DecimalSlider(MenuDisplayMode mode, string id, string name, float startvalue, float min, float max, Action<float> set, string description = null) : this(mode, id, name, min, max, float.NaN, () => { return startvalue; }, set, description)
        { }
        #endregion

        public override void Tweak(SpectrumMenu menu)
        {
            menu.TweakFloat(this.Name, this.GetValue(), this.Min, this.Max, this.DefaultValue, this.SetValue, this.Description);
            base.Tweak(menu);
        }
    }
}
