using System;
using UnityEngine.Events;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UnityEngine.UI;

namespace EmotePlayer
{
    public class UIHelper
    {
        private GameObject root;
        public UIHelper(GameObject root) { this.root = root;}
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
    }
}
