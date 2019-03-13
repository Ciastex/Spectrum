using Spectrum.API.Gui.Data;
using Spectrum.API.Gui.Menu;
using System;

namespace Spectrum.API.Gui.UI.Items
{
    public class InputPrompt : MenuItem
    {
        public string Title { get; set; }
        public string DefaultText { get; set; }
        public Func<string, string> Validate { get; set; }
        public Action<string> Submit { get; set; }
        public Action Close { get; set; }

        #region constructors
        public InputPrompt(MenuDisplayMode mode, string id, string name, string title, string defaulttext, Action<string> submit, Func<string, string> validate, Action closed, string description = null) : base(mode, name, description)
        {
            this.Title = title;
            this.DefaultText = defaulttext;
            this.Submit = submit;
            this.Validate = validate;
            this.Close = closed;
        }

        public InputPrompt(MenuDisplayMode mode, string id, string name, string title, string defaulttext, Action<string> submit, Action closed, string description = null) : this(mode, id, name, title, defaulttext, submit, (value) => { return (string)null; }, closed, description)
        { }

        public InputPrompt(MenuDisplayMode mode, string id, string name, string title, string defaulttext, Action<string> submit, string description = null) : this(mode, id, name, title, defaulttext, submit, (value) => { return (string)null; }, () => { }, description)
        { }

        public InputPrompt(MenuDisplayMode mode, string id, string name, string title, string defaulttext, Action<string> submit, Func<string, string> validate, string description = null) : this(mode, id, name, title, defaulttext, submit, validate, () => { }, description)
        { }
        #endregion

        protected bool OnSubmit(out string error, string input)
        {
            error = Validate(input);
            if (error == null)
            {
                Submit(input);
                return true;
            }
            return false;
        }

        protected void OnCancel()
        {
            Close();
        }

        public override void Tweak(SpectrumMenu menu)
        {
            menu.TweakAction(this.Name, () => {
                InputPromptPanel.Create(new InputPromptPanel.OnSubmit(this.OnSubmit), new InputPromptPanel.OnPop(this.OnCancel), this.Title, this.DefaultText);
            }, this.Description);
        }
    }
}
