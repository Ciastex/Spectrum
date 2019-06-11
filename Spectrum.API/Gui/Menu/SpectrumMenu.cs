using Spectrum.API.Events.GUI;
using Spectrum.API.GUI.Data;
using Spectrum.API.Interfaces.Systems;
using System;
using UnityEngine;

namespace Spectrum.API.GUI.Menu
{
    public class SpectrumMenu : SpectrumMenuAbstract
    {
        private const int MaxEntriesPerPage = 9;

        private SpectrumMenuController Controller { get; set; }
        private InputManager InputManager { get; set; }
        private int PageCount { get; set; }

        public GameObject TitleLabel => PanelObject_.transform.Find("MenuTitleTemplate/UILabel - Title").gameObject;
        public GameObject DescriptionLabel => PanelObject_.transform.Find("MenuTitleTemplate/UILabel - Description").gameObject;
        public GameObject OptionsTable => PanelObject_.transform.Find("Options/OptionsTable").gameObject;

        public MenuPanel MenuPanel { get; set; }
        public MenuTree MenuTree { get; set; } = new MenuTree("menu.spectrum.error", "[FF0000]Error[-]");
        public string Title { get; set; }
        public string Description { get; set; }
        public bool SwitchPageOnClose { get; private set; }
        public int CurrentPageIndex { get; set; } = 0;
        public string Id => MenuTree.Id;

        public override void InitializeVirtual()
        {
            InputManager = G.Sys.InputManager_;

            PageCount = (int)Math.Ceiling(MenuTree.Count / (float)MaxEntriesPerPage);
            DisplayMenu();

            Controller = PanelObject_.AddComponent<SpectrumMenuController>();
            Controller.Menu = this;
        }

        public SpectrumMenu WithManager(IManager manager)
        {
            Manager = manager;
            return this;
        }

        private void DisplayMenu()
        {
            MenuTree currentTree = MenuTree.GetItems(MenuSystem.GetCurrentDisplayMode());

            for (int i = CurrentPageIndex * MaxEntriesPerPage; i < (CurrentPageIndex * MaxEntriesPerPage) + MaxEntriesPerPage; i++)
            {
                if (i < currentTree.Count)
                    currentTree[i].Tweak(this);
                else break;
            }

            MenuOpenedEvent.Broadcast(this);
        }

        public override void OnPanelPop()
        {
            foreach (var item in OptionsTable.GetChildren().GetComponent<MenuItemInfo>())
                item.Destroy();

            Controller.Destroy();
            MenuPanel.Destroy();
            PanelObject_.Destroy();

            this.Destroy();
        }

        public void UpdateVirtual()
        {
            G.Sys.MenuPanelManager_.SetBottomLeftActionButton(InputAction.MenuPageUp, "PREVIOUS");
            G.Sys.MenuPanelManager_.SetBottomLeftActionButton(InputAction.MenuPageDown, "NEXT");
            G.Sys.MenuPanelManager_.SetBottomLeftActionButtonEnabled(InputAction.MenuPageUp, MenuTree.Count > MaxEntriesPerPage);
            G.Sys.MenuPanelManager_.SetBottomLeftActionButtonEnabled(InputAction.MenuPageDown, MenuTree.Count > MaxEntriesPerPage);

            if (MenuTree.Count > MaxEntriesPerPage)
            {
                if (InputManager.GetKeyUp(InputAction.MenuPageUp))
                {
                    CurrentPageIndex -= 1;
                    CurrentPageIndex = CurrentPageIndex < 0 ? PageCount - 1 : CurrentPageIndex > PageCount - 1 ? 0 : CurrentPageIndex;
                    SwitchPageOnClose = true;
                    MenuPanel.Pop();
                }

                if (InputManager.GetKeyUp(InputAction.MenuPageDown))
                {
                    CurrentPageIndex += 1;
                    CurrentPageIndex = CurrentPageIndex < 0 ? PageCount - 1 : CurrentPageIndex > PageCount - 1 ? 0 : CurrentPageIndex;
                    SwitchPageOnClose = true;
                    MenuPanel.Pop();
                }
            }

            Description = $"Page {CurrentPageIndex + 1} / {PageCount}";

            TitleLabel?.SetActive(true);
            UILabel TitleLabelObject = TitleLabel.GetComponent<UILabel>();

            if (TitleLabelObject)
                TitleLabelObject.text = Title;

            DescriptionLabel?.SetActive(true);
            UILabel DescriptionLabelObject = DescriptionLabel.GetComponent<UILabel>();

            if (DescriptionLabelObject)
                DescriptionLabelObject.text = Description;
        }
    }
}
