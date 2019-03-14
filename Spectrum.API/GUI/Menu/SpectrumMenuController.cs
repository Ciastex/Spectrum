using UnityEngine;

namespace Spectrum.API.GUI.Menu
{
    public class SpectrumMenuController : MonoBehaviour
    {
        public SpectrumMenu Menu { get; set; }

        void Update()
        {
            if (Menu != null && Menu.PanelObject_.activeInHierarchy && Menu.MenuPanel.IsTop_)
                Menu.UpdateVirtual();
        }
    }
}
