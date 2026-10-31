using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HawkNetworking;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniverseLib;
using UniverseLib.Config;
using UniverseLib.Input;
using UniverseLib.UI;

namespace EmotePlayer
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static List<PlayerManager> playerManagers = new ();
        
        private static object handle;

        internal static new ManualLogSource Logger;
        public static UIBase BaseUI { get; private set; }
        public static EmotePlayerUI EmoteUI { get; private set; }
        public static Plugin Instance { get; private set; }

        private ConfigEntry<bool> disableGameInput;
        private ConfigEntry<KeyCode> keyToggle;

        private void Awake()
        {
            Instance = this;

            Logger = base.Logger;

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
            //
            EmoteUI = new EmotePlayerUI(BaseUI);
            //
            BaseUI.Enabled = false;
        }

        public static void UIUpdate()
        {

        }

        public void EnableUI(bool enabled)
        {
            BaseUI.Enabled = enabled;
            EmoteUI.SetEnabled(enabled);

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
            if (!GameInstance.InstanceExists)
            {
                return null;
            }
            else
            {
                return GameInstance.Instance.GetFirstLocalPlayerController();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(keyToggle.Value) && GameInstance.Instance.GetGamemode())
            {
                EnableUI(!BaseUI.Enabled);
            }
        }

    }
}
