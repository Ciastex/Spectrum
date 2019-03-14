using Spectrum.API.Interfaces.Systems;

namespace Spectrum.API.GUI.Menu
{
    public abstract class SpectrumMenuAbstract : SuperMenu
    {
        public IManager Manager { get; set; }

        public override string MenuTitleName_ => "Configure Spectrum Plugins";
        public override string Name_ => "Spectrum Settings";
        public override bool DisplayInMenu(bool isPauseMenu) => true;

        public SpectrumMenuAbstract()
        {
            menuBlueprint_ = MenuSystem.MenuBlueprint;
        }

        public override void InitializeVirtual() { }
        public override void OnPanelPop() { }
    }
}
