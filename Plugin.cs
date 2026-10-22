using BepInEx;
using System;
using UnityEngine;
using UniverseLib.Config;
using UniverseLib.UI;

namespace EmotePlayer
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static UIBase UIBase { get; private set; }
        public static EmotePlayerUI EmoteUI { get; private set; }
        public static Plugin Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            UniverseLibConfig config = new()
            {
                Force_Unlock_Mouse = true
            };
            UniverseLib.Universe.Init(1f, OnUIInit, (x, y) => { }, config);
        }

        public static void OnUIInit()
        {
            UIBase = UniversalUI.RegisterUI(MyPluginInfo.PLUGIN_GUID, UIUpdate);
           EmoteUI= new EmotePlayerUI(UIBase);
            UIBase.Enabled = false;
        }

        public static void UIUpdate()
        {

        }

        public void EnableUI(bool enabled)
        {
            if (UIBase == null) throw new ArgumentNullException(nameof(UIBase));
            UIBase.Enabled = enabled;
            if (EmoteUI == null) throw new ArgumentNullException(nameof(UIBase));
            EmoteUI.Set(enabled);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F4) && GameInstance.Instance.GetGamemode() != null)
            {
                EnableUI(!UIBase.Enabled);
            }
        }
    }
}
