using Spectrum.API.GUI.Data;
using Spectrum.API.GUI.Menu;
using System;

namespace Spectrum.API.GUI.Controls
{
    public class ActionButton : MenuItem
    {
        public Action OnClick { get; set; }

        public ActionButton(MenuDisplayMode mode, string id, string name) 
            : base(mode, id, name) { }

        public ActionButton WhenClicked(Action onClick)
        {
            OnClick = onClick;
            return this;
        }

        public override void Tweak(SpectrumMenu menu)
        {
            if (OnClick == null)
                throw new InvalidOperationException("OnClick action not initialized. Use WhenClicked() to configure the action.");

            menu.TweakAction(Name, OnClick, Description);
            base.Tweak(menu);
        }
    }
}
