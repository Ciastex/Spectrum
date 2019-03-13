using Spectrum.API.Gui.Data;
using Spectrum.API.Gui.Menu;

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

        public abstract void Tweak(SpectrumMenu menu);
    }
}
