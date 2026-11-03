using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;
using UniverseLib.UI.Widgets;
using System;
using UnityEngine;

namespace EmotePlayer
{
    public class SettingsPanel(UIBase owner) : PanelBase(owner)
    {
        private InputFieldRef toggleKeyInput;

        public override string Name => "Settings";
        public override int MinWidth => 400;

        public override int MinHeight => 500;

        public override Vector2 DefaultAnchorMin => new(0.5f, 0.5f);

        public override Vector2 DefaultAnchorMax => new(0.5f, 0.5f);

        public override Vector2 DefaultPosition => new(0, 0);

        public override bool CanDragAndResize => true;

        protected override void ConstructPanelContent()
        {
            var ui = new UIManager(ContentRoot);

            ui.CreateButton("Back", () =>
            {
                SetEnabled(false);
                Plugin.HomeUI.SetEnabled(true);
            }, "btnBack", new Color(0.15f, 0f, 0.110f));

            ui.AddSpacer(6);

            ui.CreateLabel("Menu Settings");

            ui.CreateToggle("Toggle Players Input When the UI is Open", "Toggle Players Input When the UI is Open", b =>
            {
                Plugin.Instance.disableGameInput.Value = b; // toggle the config from the menu instead
            });

            ui.AddSpacer(5);

            ui.CreateLabel("Toggle Key (Default is F4)");

            ui.AddSpacer(5);

            toggleKeyInput = ui.CreateInputField(Plugin.Instance.keyToggle.Value.ToString(), "emotePlayerConfigSetting"); // the default key is f4 desplay the current key in the input field
            ui.AddSpacer(5);

            ui.CreateButton("Apply", () =>
            {
                if (Enum.TryParse<KeyCode>(toggleKeyInput.Text, out var key)) // parse the input text to a keycode
                {
                    Plugin.Instance.keyToggle.Value = key;
                }
            }, "btn", new Color(0.15f, 0f, 0.110f));
        }

        public void SetEnabled(bool b)
        {
            SetActive(b);
        }

        protected override void OnClosePanelClicked()
        {
            Plugin.Instance.EnableUI(false);
        }
    }
}
