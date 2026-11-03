using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System.Collections.Generic;
using UnityEngine;
using UniverseLib;
using UniverseLib.Config;
using UniverseLib.UI;

namespace EmotePlayer
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static List<PlayerManager> playerManagers = new();

        internal static new ManualLogSource Logger;

        // panel stuff

        public static UIBase BaseUI { get; private set; }
        public static HomePanel HomeUI { get; private set; }
        public static EmotePlayerPanel EmoteUI { get; private set; }
        public static SettingsPanel SettingsUI { get; private set; }
        public static Plugin Instance { get; private set; }

        public ConfigEntry<bool> disableGameInput;
        public ConfigEntry<KeyCode> keyToggle;

        private void Awake()
        {
            // instance
            Instance = this;

            Logger = base.Logger;

            // config
            disableGameInput = Config.Bind("General", "DisableGameInput", false, "Disable game input when the menu is open");
            keyToggle = Config.Bind("Toggle", "KeyToggle", KeyCode.F4, "Default key to open the menu");

            UniverseLibConfig config = new()
            {
                Force_Unlock_Mouse = true,
                Disable_EventSystem_Override = false
            };
            Universe.Init(1f, UIInit, (str, type) => {
            }, config);

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        public void UIInit()
        {
            BaseUI = UniversalUI.RegisterUI(MyPluginInfo.PLUGIN_GUID, UIUpdate);
            HomeUI = new HomePanel(BaseUI);
            EmoteUI = new EmotePlayerPanel(BaseUI);
            SettingsUI = new SettingsPanel(BaseUI);

            HomeUI.SetEnabled(false);
            EmoteUI.SetEnabled(false);
            SettingsUI.SetEnabled(false);
            BaseUI.Enabled = false;
        }

        public static void UIUpdate()
        {

        }

        public void EnableUI(bool enabled)
        {
            BaseUI.Enabled = enabled;

            if (enabled)
            {
                // only the home menu should be opened
                HomeUI.SetEnabled(true);
                EmoteUI.SetEnabled(false);
                SettingsUI.SetEnabled(false);
            }
            else
            {
                HomeUI.SetEnabled(false);
                EmoteUI.SetEnabled(false);
                SettingsUI.SetEnabled(false);
            }

            var player = GetMyPlayer();
            var input = player?.GetPlayerControllerInputManager();
            if (input == null) return;
            
            if (enabled && disableGameInput.Value)
            {

                input?.DisablePlayerTransformInput(this);
                input?.DisableGameplayCameraInput(this);
                input?.DisableGameplayInput(this);
            }
            else
            {
                input?.EnablePlayerTransformInput(this);
                input?.EnableGameplayCameraInput(this);
                input?.EnableGameplayInput(this);
            }
        }

        public static PlayerController GetMyPlayer()
        {
            if (!GameInstance.InstanceExists) return null;
            return GameInstance.Instance.GetFirstLocalPlayerController();
        }

        private void Update()
        {
            if (Input.GetKeyDown(keyToggle.Value))
            {
                bool enabled = !BaseUI.Enabled;
                EnableUI(enabled);
            }
        }
    }
}
