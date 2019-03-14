using Spectrum.API.GUI.Data;

namespace Spectrum.API.Interfaces.Systems
{
    public interface IMenuManager
    {
        MenuTree Menus { get; }
        void AddMenu(MenuDisplayMode displayMode, MenuTree menu, string description = null);
    }
}
