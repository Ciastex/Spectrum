using Spectrum.API.Extensions;
using Spectrum.API.GUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectrum.API.GUI.Data
{
    public class MenuTree : List<MenuItem>
    {
        public string Title { get; set; }
        public string Id { get; private set; }

        public MenuTree(string id, string title)
        {
            Id = id;
            Title = title;
        }

        public ActionButton ActionButton(MenuDisplayMode displayMode, string id, string name, Action action, string description = null)
        {
            var actionButton = new ActionButton(displayMode, id, name)
                .WhenClicked(action)
                .WithDescription(description);

            Add(actionButton);

            return actionButton as ActionButton;
        }

        public CheckBox CheckBox(MenuDisplayMode displayMode, string id, string name, Func<bool> getter, 
            Action<bool> setter, bool defaultValue = false, string description = null)
        {
            var checkBox = new CheckBox(displayMode, id, name)
                .WithGetter(getter)
                .WithSetter(setter)
                .WithDefaultValue(defaultValue)
                .WithDescription(description);

            Add(checkBox);

            return checkBox as CheckBox;
        }

        public FloatSlider FloatSlider(MenuDisplayMode displayMode, string id, string name, 
            Func<float> getter, Action<float> setter, float minimum = 0.0f, 
            float maximum = 1.0f, float defaultValue = 0.0f, string description = null)
        {
            var floatSlider = new FloatSlider(displayMode, id, name)
                .WithGetter(getter)
                .WithSetter(setter)
                .LimitedByRange(minimum, maximum)
                .WithDefaultValue(defaultValue)
                .WithDescription(description);

            Add(floatSlider);

            return floatSlider as FloatSlider;
        }

        public IntegerSlider IntegerSlider(MenuDisplayMode displayMode, string id, string name,
            Func<int> getter, Action<int> setter, int minimum = 0, 
            int maximum = 10, int defaultValue = 0, string description = null)
        {
            var integerSlider = new IntegerSlider(displayMode, id, name)
                .WithGetter(getter)
                .WithSetter(setter)
                .LimitedByRange(minimum, maximum)
                .WithDefaultValue(defaultValue)
                .WithDescription(description);

            Add(integerSlider);

            return integerSlider as IntegerSlider;
        }

        public InputPrompt InputPrompt(MenuDisplayMode displayMode, string id, string name, 
            Action<string> submitAction, Action<InputPrompt> closeAction = null, Func<string, string> validator = null, 
            string title = null, string defaultValue = null, string description = null)
        {
            var inputPrompt = new InputPrompt(displayMode, id, name)
                .WithSubmitAction(submitAction)
                .WithCloseAction(closeAction)
                .ValidatedBy(validator)
                .WithTitle(title)
                .WithDefaultValue(defaultValue)
                .WithDescription(description);

            Add(inputPrompt);

            return inputPrompt as InputPrompt;
        }

        public PasswordPrompt PasswordPrompt(MenuDisplayMode displayMode, string id, string name, 
            Action<string> submitAction, Action<InputPrompt> closeAction, Func<string, string> validator, 
            string title = null, string defaultValue = null, string description = null)
        {
            var passwordPrompt = new PasswordPrompt(displayMode, id, name)
                .WithSubmitAction(submitAction)
                .WithCloseAction(closeAction)
                .ValidatedBy(validator)
                .WithTitle(title)
                .WithDefaultValue(defaultValue)
                .WithDescription(description);

            Add(passwordPrompt);

            return passwordPrompt as PasswordPrompt;
        }       
        
        public ListBox<T> ListBox<T>(MenuDisplayMode displayMode, string id, string name, 
            Func<T> getter, Action<T> setter, Dictionary<string, T> entries, 
            string description = null)
        {
            var listBox = new ListBox<T>(displayMode, id, name)
                .WithGetter(getter)
                .WithSetter(setter)
                .WithEntries(entries)
                .WithDescription(description);

            Add(listBox);

            return listBox as ListBox<T>;
        }

        public SubmenuButton SubmenuButton(MenuDisplayMode displayMode, string id, string name, 
            MenuTree menuTree, string description = null)
        {
            var submenuButton = new SubmenuButton(displayMode, id, name)
                .NavigatesTo(menuTree)
                .WithDescription(description);

            Add(submenuButton);

            return submenuButton as SubmenuButton;
        }

        public MenuTree GetItems(MenuDisplayMode mode)
        {
            MenuTree tree = new MenuTree(Id, Title);

            tree.AddRange<MenuItem>(this.Where((item) => {
                return item.Mode.HasFlag(mode);
            }));

            return tree;
        }
    }

}
