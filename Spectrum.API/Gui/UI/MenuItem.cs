using Spectrum.API.Gui.Data;
using Spectrum.API.Gui.Menu;
using UnityEngine;

namespace Spectrum.API.Gui.UI
{
    public abstract class MenuItem
    {
        public readonly MenuDisplayMode Mode;
        public string Name { get; set; }
        public string Id { get; private set; }
        public string Description { get; set; }

        protected MenuItem(MenuDisplayMode mode, string id, string name, string description = "")
        {
            this.Name = name;
            this.Id = id;
            this.Description = description;
            this.Mode = mode;
        }

        public virtual void Tweak(SpectrumMenu menu)
        {
            GameObject item = menu.OptionsTable().transform.Find(this.Name).gameObject;
            if (item != null)
            {
                MenuItemInfo info = item.AddComponent<MenuItemInfo>();
                info.Item = this;
            }
        }
    }
}
