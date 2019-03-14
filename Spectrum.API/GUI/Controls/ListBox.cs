using Spectrum.API.GUI.Data;
using Spectrum.API.GUI.Menu;
using System;
using System.Collections.Generic;

namespace Spectrum.API.GUI.Controls
{
    public class ListBox<T> : MenuItem
    {
        public Func<T> Get { get; set; }
        public Action<T> Set { get; set; }
        public Dictionary<string, T> Entries { get; set; }

        public ListBox(MenuDisplayMode mode, string id, string name) 
            : base(mode, id, name) { }

        public ListBox<T> WithGetter(Func<T> getter)
        {
            Get = getter ?? throw new ArgumentNullException("Getter cannot be null.");
            return this;
        }

        public ListBox<T> WithSetter(Action<T> setter)
        {
            Set = setter ?? throw new ArgumentNullException("Setter cannot be null.");
            return this;
        }

        public ListBox<T> WithEntries(Dictionary<string, T> entries)
        {
            if (entries == null)
                entries = new Dictionary<string, T>();

            Entries = entries;
            return this;
        }
        
        public override void Tweak(SpectrumMenu menu)
        {
            if (Get == null || Set == null)
                throw new InvalidOperationException("Cannot call TweakEnum with Get or Set being null.");

            menu.TweakEnum(Name, Get, Set, Description, Entries.ToArray());
            base.Tweak(menu);
        }
    }
}
