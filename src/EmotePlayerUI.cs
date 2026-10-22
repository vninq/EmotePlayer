using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace EmotePlayer;
public class EmotePlayerUI : UniverseLib.UI.Panels.PanelBase
{
    private int current = 0;
    public string[] emotes = { "Happy", "Crying", "Angry", "Heart", "Thanks", "Well Done", "Follow Me", "Sorry", "Need Help", "Bye", "Clap", "Double Wave", "Laugh Selected", "Dance", "Thumbs Up", "Thumbs Down", "Gumpy" }; 
    public EmotePlayerUI(UIBase owner) : base(owner)
    {
    }

    public override string Name => "Emote Player";

    public override int MinWidth => 400;

    public override int MinHeight => 500;

    public override Vector2 DefaultAnchorMin => new(0.5f, 0.5f);

    public override Vector2 DefaultAnchorMax => new(0.5f, 0.5f);

    public override Vector2 DefaultPosition => new(MinWidth / 2, MinHeight / 2);

    public override bool CanDragAndResize => true;

    protected override void ConstructPanelContent()
    {
        var ui = new UIHelper(ContentRoot);

        ui.CreateDropdown("Select emote", index => 
        {
            current = index;

        }, defaultOptions: emotes);
        ui.AddSpacer(5);
        ui.CreateButton("Play", () =>
        {
            OnSelected(current);
        }, "btn", new Color(0.15f, 0f, 0.65f));

        ui.AddSpacer(5);

        ui.CreateButton("Play For Everyone", () =>
        {
            PlayForEveryOne(current);
        }, "btn", new Color(0.15f, 0f, 0.65f));

    }

    public void OnSelected(int index)
    {
        try
        {
            var selected = emotes[index];
            var controller = GetGehweg();
            controller?.GetPlayerCharacter()?.PlayEmote(selected);
        }
catch (Exception)
        {
        }
    }

    private void PlayForEveryOne(int index)
    {
        var selected = emotes[index];
        foreach (var character in GameInstance.Instance.GetPlayerCharacters())
        {
            character?.PlayEmote(selected);
        }
    }

    public static PlayerController GetGehweg()
    {
        if (!GameInstance.InstanceExists)
        {
            return null;
        }
        return GameInstance.Instance.GetFirstLocalPlayerController();
    }

    public void Set(bool b)
    {
        SetActive(b);
    }

    protected override void OnClosePanelClicked()
    {
        Plugin.Instance.EnableUI(false);
    }
}
