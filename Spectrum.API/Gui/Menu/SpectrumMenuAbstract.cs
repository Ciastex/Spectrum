using Spectrum.API.Interfaces.Systems;

namespace Spectrum.API.Gui.Menu
{
    public abstract class SpectrumMenuAbstract : SuperMenu
    {
        public override string MenuTitleName_ => "Configure Spectrum Plugins";
        public override string Name_ => "Spectrum Settings";

        public IManager Manager { get; private set; }

        public override bool DisplayInMenu(bool isPauseMenu) => true;

        public SpectrumMenuAbstract()
        {
            menuBlueprint_ = MenuSystem.MenuBlueprint;
        }

        public void SetManager(IManager manager)
        {
            Manager = manager;
        }

        public override void InitializeVirtual()
        {

        }

        public override void OnPanelPop()
        {

        }
    }
}
