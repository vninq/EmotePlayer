using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;

namespace EmotePlayer;
public class EmotePlayerPanel : PanelBase
{
    // dropdown stuff
    private int current = 0;
    private Dropdown selectedPlayer;

    // player stuff
    private List<PlayerController> controllers = new();
    public PlayerController playerSelected { get; private set; }
    protected PlayerCharacter character;

    // emotes
    public string[] emotes = { "Happy", "Crying", "Angry", "Heart", "Thanks", "Well Done", "Follow Me", "Sorry", "Need Help", "Bye", "Clap", "Double Wave", "Laughing", "Dance", "Thumbs Up", "Thumbs Down", "Gumpy" };

    public EmotePlayerPanel(UIBase owner) : base(owner)
    {
    }

    public override string Name => "Emote Player";

    public override int MinWidth => 400;

    public override int MinHeight => 500;

    public override Vector2 DefaultAnchorMin => new(0.5f, 0.5f);

    public override Vector2 DefaultAnchorMax => new(0.5f, 0.5f);

    public override Vector2 DefaultPosition => new(0, 0);

    public override bool CanDragAndResize => true;

    // ui
    protected override void ConstructPanelContent()
    {
        var ui = new UIManager(ContentRoot);

        ui.CreateButton("Back", () =>
        {
            SetEnabled(false);
            Plugin.HomeUI.SetEnabled(true);
        }, "btnBack", new Color(0.15f, 0f, 0.110f));

        ui.AddSpacer(6);

        ui.CreateLabel("Select player");

        selectedPlayer = ui.CreateDropdown("Select Player", index =>
        {
            if (controllers != null && index >= 0 && index < controllers.Count)
            {
                SetController(controllers[index]);
                UpdatePlayerView();
            }
        }, defaultOptions: new string[] { });

        ui.AddSpacer(5);

        ui.CreateLabel("Select emote");

        ui.CreateDropdown("Select emote", index =>
        {
            current = index;

        }, defaultOptions: emotes);

        ui.AddSpacer(5);

        ui.CreateButton("Play", () =>
        {
            OnSelected(current);
        }, "btn", new Color(0.15f, 0f, 0.110f));

        ui.AddSpacer(5);

        ui.CreateButton("Play For Everyone", () =>
        {
            PlayForEveryOne(current);
        }, "btn", new Color(0.15f, 0f, 0.110f));
    }

    private void OnSelected(int index)
    {
        var selected = emotes[index];

        playerSelected?.GetPlayerCharacter()?.PlayEmote(selected);
    }

    private void PlayForEveryOne(int index)
    {
        var selected = emotes[index];
        foreach (var character in GameInstance.Instance.GetPlayerCharacters())
        {
            character?.PlayEmote(selected);
        }
    }

    public void SetEnabled(bool b)
    {
        SetActive(b);
        if (b)
        {
            UpdatePlayers();
            selectedPlayer?.RefreshShownValue();
        }
    }

    public void UpdatePlayerView()
    {
        SetController(playerSelected);
        foreach (var playerManager in Plugin.playerManagers)
        {
            playerManager?.Refresh();
        }
    }

    public void UpdatePlayers()
    {
        RefreshPlayerList();

        if (controllers != null && controllers.Count > 0)
        {
            SetController(controllers[0]);
            if (selectedPlayer != null)
            {
                selectedPlayer.value = 0;
            }
        }
        else
        {
            SetController(null);
        }

        UpdatePlayerView();
        selectedPlayer?.RefreshShownValue();
    }

    public void SetController(PlayerController controller)
    {
        playerSelected = controller;
        character = playerSelected?.GetPlayerCharacter();
        if (Plugin.playerManagers != null)
        {
            foreach (var playerManager in Plugin.playerManagers)
            {
                playerManager?.SetPlayer(controller);
            }
        }
    }

    public void RefreshPlayerList()
    {
        controllers = GameInstance.Instance.GetPlayerControllers();
        selectedPlayer?.ClearOptions();
        foreach (var controllers in controllers)
        {
            selectedPlayer.options.Add(new Dropdown.OptionData(controllers.GetPlayerName()));
        }
        selectedPlayer?.RefreshShownValue();
    }

    protected override void OnClosePanelClicked()
    {
        Plugin.Instance.EnableUI(false);
    }
}
