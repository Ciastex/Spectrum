using Spectrum.API.Gui.UI;
using System.Collections.Generic;
using System.Linq;
using Spectrum.API.Extensions;
using Spectrum.API.Gui.UI.Items;
using System;

namespace Spectrum.API.Gui.Data
{
    public class MenuTree : List<MenuItem>
    {
        public string Title { get; set; }
        public string Id { get; private set; }

        #region constructors
        public MenuTree(string id, string title)
        {
            this.Id = id;
            this.Title = title;
        }
        #endregion
        #region tweak methods
        #region action button
        public void TweakAction(bool mainmenu, bool pausemenu, string id, string name, Action action, string description = null)
        {
            Add(new ActionButton(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, action, description));
        }
        #endregion
        #region check box
        public void TweakBool(bool mainmenu, bool pausemenu, string id, string name, Func<bool> get, Action<bool> set, string description = null)
        {
            Add(new CheckBox(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, get, set, description));
        }

        public void TweakBool(bool mainmenu, bool pausemenu, string id, string name, bool startvalue, Action<bool> set, string description = null)
        {
            Add(new CheckBox(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, startvalue, set, description));
        }
        #endregion
        #region decimal slider
        public void TweakFloat(bool mainmenu, bool pausemenu, string id, string name, float min, float max, float cursor, Func<float> get, Action<float> set, string description = null)
        {
            Add(new DecimalSlider(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, min, max, cursor, get, set, description));
        }

        public void TweakFloat(bool mainmenu, bool pausemenu, string id, string name, float startvalue, Action<float> set, string description = null)
        {
            Add(new DecimalSlider(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, startvalue, set, description));
        }

        public void TweakFloat(bool mainmenu, bool pausemenu, string id, string name, float startvalue, float min, float max, Action<float> set, string description = null)
        {
            Add(new DecimalSlider(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, startvalue, min, max, set, description));
        }
        #endregion
        #region input prompt
        public void TweakInput(bool mainmenu, bool pausemenu, string id, string name, string title, string defaulttext, Action<string> submit, Func<string, string> validate, Action closed, string description = null)
        {
            Add(new InputPrompt(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, title, defaulttext, submit, validate, closed, description));
        }

        public void TweakInput(bool mainmenu, bool pausemenu, string id, string name, string title, string defaulttext, Action<string> submit, Action closed, string description = null)
        {
            Add(new InputPrompt(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, title, defaulttext, submit, closed, description));
        }

        public void TweakInput(bool mainmenu, bool pausemenu, string id, string name, string title, string defaulttext, Action<string> submit, string description = null)
        {
            Add(new InputPrompt(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, title, defaulttext, submit, description));
        }

        public void TweakInput(bool mainmenu, bool pausemenu, string id, string name, string title, string defaulttext, Action<string> submit, Func<string, string> validate, string description = null)
        {
            Add(new InputPrompt(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, title, defaulttext, submit, validate, description));
        }
        #endregion
        #region integer slider
        public void TweakInt(bool mainmenu, bool pausemenu, string id, string name, int min, int max, int cursor, Func<int> get, Action<int> set, string description = null)
        {
            Add(new IntegerSlider(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, min, max, cursor, get, set, description));
        }

        public void TweakInt(bool mainmenu, bool pausemenu, string id, string name, int startvalue, Action<int> set, string description = null)
        {
            Add(new IntegerSlider(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, startvalue, set, description));
        }

        public void TweakInt(bool mainmenu, bool pausemenu, string id, string name, int startvalue, int min, int max, Action<int> set, string description = null)
        {
            Add(new IntegerSlider(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, startvalue, min, max, set, description));
        }
        #endregion
        #region password prompt
        public void TweakPassword(bool mainmenu, bool pausemenu, string id, string name, string title, string defaulttext, Action<string> submit, Func<string, string> validate, Action closed, string description = null)
        {
            Add(new PasswordPrompt(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, title, defaulttext, submit, validate, closed, description));
        }

        public void TweakPassword(bool mainmenu, bool pausemenu, string id, string name, string title, string defaulttext, Action<string> submit, Action closed, string description = null)
        {
            Add(new PasswordPrompt(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, title, defaulttext, submit, closed, description));
        }

        public void TweakPassword(bool mainmenu, bool pausemenu, string id, string name, string title, string defaulttext, Action<string> submit, string description = null)
        {
            Add(new PasswordPrompt(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, title, defaulttext, submit, description));
        }

        public void TweakPassword(bool mainmenu, bool pausemenu, string id, string name, string title, string defaulttext, Action<string> submit, Func<string, string> validate, string description = null)
        {
            Add(new PasswordPrompt(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, title, defaulttext, submit, validate, description));
        }
        #endregion
        #region list box
        public void TweakList<T>(bool mainmenu, bool pausemenu, string id, string name, Func<T> get, Action<T> set, Func<KeyValuePair<string, T>[]> entries, string description = null)
        {
            Add(new ListBox<T>(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, get, set, entries, description));
        }

        public void TweakList<T>(bool mainmenu, bool pausemenu, string id, string name, Func<T> get, Action<T> set, KeyValuePair<string, T>[] entries, string description = null)
        {
            Add(new ListBox<T>(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, get, set, entries, description));
        }

        public void TweakList<T>(bool mainmenu, bool pausemenu, string id, string name, Func<T> get, Action<T> set, Dictionary<string, T> entries, string description = null)
        {
            Add(new ListBox<T>(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, get, set, entries, description));
        }

        public void TweakList<T>(bool mainmenu, bool pausemenu, string id, string name, T startValue, Action<T> set, Func<KeyValuePair<string, T>[]> entries, string description = null)
        {
            Add(new ListBox<T>(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, () => { return startValue; }, set, entries, description));
        }

        public void TweakList<T>(bool mainmenu, bool pausemenu, string id, string name, T startValue, Action<T> set, KeyValuePair<string, T>[] entries, string description = null)
        {
            Add(new ListBox<T>(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, () => { return startValue; }, set, entries, description));
        }

        public void TweakList<T>(bool mainmenu, bool pausemenu, string id, string name, T startValue, Action<T> set, Dictionary<string, T> entries, string description = null)
        {
            Add(new ListBox<T>(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, () => { return startValue; }, set, entries, description));
        }
        #endregion
        #region submenu button
        public void TweakMenu(bool mainmenu, bool pausemenu, string id, string name, MenuTree menu, string description = null)
        {
            Add(new SubmenuButton(MenuDisplayModeEx.Make(mainmenu, pausemenu), id, name, menu, description));
        }
        #endregion
        #endregion

        public MenuTree GetItems(MenuDisplayMode mode)
        {
            MenuTree tree = new MenuTree(this.Id, this.Title);

            tree.AddRange<MenuItem>(this.Where<MenuItem>((item) => {
                return item.Mode.HasFlag<MenuDisplayMode>(mode);
            }));

            return tree;
        }
    }

}
