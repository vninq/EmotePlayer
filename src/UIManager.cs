using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace EmotePlayer
{
    internal class UIManager
    {
        private GameObject root;
        public UIManager(GameObject root) { this.root = root;}
        public ButtonRef CreateButton(string text, Action onClick, string name = "", Color? color = null)
        {
            if (name == "") name = text;

            var button = UIFactory.CreateButton(root, name, text, color);
            button.OnClick = onClick;
            UIFactory.SetLayoutElement(button.GameObject, 25, 25, 9999);

            return button;
        }
        public Dropdown CreateDropdown(string name, Action<int> onValueChanged, string defaultItemText = "", int itemFontSize = 16, string[] defaultOptions = null)
        {
            var obj = UIFactory.CreateDropdown(root, name, out var dropdown, defaultItemText, itemFontSize, onValueChanged, defaultOptions);
            UIFactory.SetLayoutElement(obj, 25, 25, 9999);

            return dropdown;
        }
        public void AddSpacer(int height)
        {
            var obj = UIFactory.CreateUIObject("Spacer", root);
            UIFactory.SetLayoutElement(obj, minHeight: height, flexibleHeight: 0);
        }
        public Text CreateLabel(string text, string name = "", TextAnchor alignment = TextAnchor.MiddleLeft, Color? color = null, bool richTextSupport = true, int fontSize = 14)
        {
            if (name == "") name = text;
            if (color == null) color = Color.white;

            var label = UIFactory.CreateLabel(root, name, text, alignment, (Color)color, richTextSupport, fontSize);
            UIFactory.SetLayoutElement(label.gameObject, 25, 25, 9999);

            return label;}
        public Toggle CreateToggle(string name = "", string label = "", UnityAction<bool> onValueChanged = null, bool defaultState = false, Color bgColor = default,
          int checkWidth = 20, int checkHeight = 20)
        {
            var obj = UIFactory.CreateToggle(root, name, out var toggle, out var text, bgColor, checkWidth, checkHeight);
            text.text = label;

            toggle.isOn = defaultState;
            toggle.onValueChanged.AddListener(onValueChanged);

            UIFactory.SetLayoutElement(obj, 25, 25, 9999);

            return toggle;
        }
        public InputFieldRef CreateInputField(string placeholder, string name = "")
        {
            var input = UIFactory.CreateInputField(root, name, placeholder);
            UIFactory.SetLayoutElement(input.GameObject, 25, 25, 9999);
            return input;
        }
    }
}
