using Spectrum.API.GUI.Data;
using Spectrum.API.GUI.Menu;
using UnityEngine;

namespace Spectrum.API.GUI.Controls
{
    public abstract class MenuItem
    {
        public MenuDisplayMode Mode { get; }
        public string Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected MenuItem(MenuDisplayMode mode, string id, string name)
        {
            Mode = mode;

            Id = id;
            Name = name;
        }

        public MenuItem WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public virtual void Tweak(SpectrumMenu menu)
        {
            GameObject item = menu.OptionsTable.transform.Find(Name).gameObject;
            if (item != null)
            {
                MenuItemInfo info = item.AddComponent<MenuItemInfo>();
                info.Item = this;
            }
        }
    }
}