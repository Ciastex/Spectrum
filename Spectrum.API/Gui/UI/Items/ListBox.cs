using Spectrum.API.Gui.Data;
using Spectrum.API.Gui.Menu;
using System;
using System.Collections.Generic;

namespace Spectrum.API.Gui.UI.Items
{
    public class ListBox<T> : MenuItem
    {
        public Func<T> GetValue { get; set; }
        public Action<T> SetValue { get; set; }
        public Func<KeyValuePair<string, T>[]> Entries { get; set; }

        #region constructors
        public ListBox(MenuDisplayMode mode, string id, string name, Func<T> get, Action<T> set, Func<KeyValuePair<string, T>[]> entries, string description = null) : base(mode, id, name, description)
        {
            this.GetValue = get;
            this.SetValue = set;
            this.Entries = entries;
        }

        public ListBox(MenuDisplayMode mode, string id, string name, Func<T> get, Action<T> set, KeyValuePair<string, T>[] entries, string description = null) : this(mode, id, name, get, set, () => { return entries; }, description)
        { }

        public ListBox(MenuDisplayMode mode, string id, string name, Func<T> get, Action<T> set, Dictionary<string, T> entries, string description = null) : this(mode, id, name, get, set, entries.ToArray(), description)
        { }
        #endregion

        public override void Tweak(SpectrumMenu menu)
        {
            menu.TweakEnum<T>(this.Name, this.GetValue, this.SetValue, this.Description, this.Entries());
            base.Tweak(menu);
        }
    }
}
