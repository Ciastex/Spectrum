using Spectrum.API.GUI.Controls;
using UnityEngine;

namespace Spectrum.API.GUI.Data
{
    public class MenuItemInfo : MonoBehaviour
    {
        public string Id => Item?.Id;
        public MenuItem Item { get; set; }
    }
}