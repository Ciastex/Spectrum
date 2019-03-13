using Events;
using Spectrum.API.Events.Gui;
using Spectrum.API.Gui.Data;
using System;
using UnityEngine;

namespace Spectrum.API.Gui.Menu
{
    public class SpectrumMenu : SpectrumMenuAbstract
    {
        // The menu panel used to display the GUI
        public MenuPanel MenuPanel = null;

        // Just a class to make an update method
        private SpectrumMenuController controller;

        // Determines if the menu is switching to another page when closing
        public bool PageSwitching { get; private set; } = false;

        // Menu title
        public string Title { get; set; }

        // Menu description
        public string Description { get; set; }

        // Menu identifier
        public string Id {
            get
            {
                return this.Entries.Id;
            }
        }
        
        #region Menu Objects
        public GameObject TitleLabel()
        {
            return PanelObject_.transform.Find("MenuTitleTemplate/UILabel - Title").gameObject;
        }

        public GameObject DescriptionLabel()
        {
            return PanelObject_.transform.Find("MenuTitleTemplate/UILabel - Description").gameObject;
        }

        public GameObject OptionsTable()
        {
            return PanelObject_.transform.Find("Options/OptionsTable").gameObject;
        }
        #endregion

        // Input manager to handle key presses (previous/next buttons)
        private InputManager im = null;

        // List of menu entries
        public MenuTree Entries { get; set; } = new MenuTree("menu.spectrum.error", "[FF0000]Error[-]");

        // Maximum amount of items per page
        const int MaxEntriesPerPage = 9;

        // Currently displayed page (index 0 is page 1)
        public int PageIndex { get; set; } = 0;

        // Number of available pages
        private int PageCount;

        public override void InitializeVirtual()
        {
            im = G.Sys.InputManager_;

            PageCount = (int)Math.Ceiling(Entries.Count / (float)MaxEntriesPerPage);

            DisplayMenu();

            controller = this.PanelObject_.AddComponent<SpectrumMenuController>();
            controller.Menu = this;
        }

        private void DisplayMenu()
        {
            MenuTree currentTree = Entries.GetItems(MenuSystem.GetCurrentMode());
            for (int i = PageIndex * MaxEntriesPerPage; i < (PageIndex * MaxEntriesPerPage) + MaxEntriesPerPage; i++)
                if (i < currentTree.Count)
                    currentTree[i].Tweak(this);
                else break;
            StaticEvent<MenuOpened.Data>.Broadcast(new MenuOpened.Data(this));
        }

        // On menu closed
        public override void OnPanelPop()
        {
            foreach (var item in OptionsTable().GetChildren().GetComponent<MenuItemInfo>())
                item.Destroy();
            controller.Destroy();
            MenuPanel.Destroy();
            PanelObject_.Destroy();
            this.Destroy();
        }

        // Update method
        public void UpdateVirtual()
        {
            G.Sys.MenuPanelManager_.SetBottomLeftActionButton(InputAction.MenuPageUp, "PREVIOUS");
            G.Sys.MenuPanelManager_.SetBottomLeftActionButton(InputAction.MenuPageDown, "NEXT");
            G.Sys.MenuPanelManager_.SetBottomLeftActionButtonEnabled(InputAction.MenuPageUp, Entries.Count > MaxEntriesPerPage);
            G.Sys.MenuPanelManager_.SetBottomLeftActionButtonEnabled(InputAction.MenuPageDown, Entries.Count > MaxEntriesPerPage);

            if (Entries.Count > MaxEntriesPerPage)
            {
                if (im.GetKeyUp(InputAction.MenuPageUp))
                {
                    PageIndex -= 1;
                    PageIndex = PageIndex < 0 ? PageCount - 1 : PageIndex > PageCount - 1 ? 0 : PageIndex;
                    PageSwitching = true;
                    MenuPanel.Pop();
                }
                if (im.GetKeyUp(InputAction.MenuPageDown))
                {
                    PageIndex += 1;
                    PageIndex = PageIndex < 0 ? PageCount - 1 : PageIndex > PageCount - 1 ? 0 : PageIndex;
                    PageSwitching = true;
                    MenuPanel.Pop();
                }
            }

            Description = $"Page {PageIndex + 1} / {PageCount}";

            TitleLabel()?.SetActive(true);
            UILabel TitleLabelObject = TitleLabel().GetComponent<UILabel>();
            if (TitleLabelObject)
                TitleLabelObject.text = Title;

            DescriptionLabel()?.SetActive(true);
            UILabel DescriptionLabelObject = DescriptionLabel().GetComponent<UILabel>();
            if (DescriptionLabelObject)
                DescriptionLabelObject.text = Description;
        }
    }

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
