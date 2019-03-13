using Spectrum.API.Gui.UI;
using UnityEngine;

namespace Spectrum.API.Gui.Data
{
    class MenuItemInfo : MonoBehaviour
    {
        public string Id {
            get
            {
                return this.Item?.Id;
            }
        }

        public MenuItem Item { get; set; }
    }
}
