using System;
using System.Collections.Generic;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;

namespace EmotePlayer;
public class HomePanel : PanelBase
{
    public HomePanel(UIBase owner) : base(owner)
    {
    }

    public override string Name => "Home";

    public override int MinWidth => 300;

    public override int MinHeight => 300;

    public override Vector2 DefaultAnchorMin => new(0.5f, 0.5f);

    public override Vector2 DefaultAnchorMax => new(0.5f, 0.5f);

    public override Vector2 DefaultPosition => new(0, 0);

    public override bool CanDragAndResize => true;

    // ui
    protected override void ConstructPanelContent()
    {
        var ui = new UIManager(ContentRoot);

        // ui.CreateLabel(SteamClient.Name);

        ui.CreateButton("Open Emote Player", () =>
        {
            SetEnabled(false);
            Plugin.EmoteUI.SetEnabled(true);
        }, "btnColor", new Color(0.15f, 0f, 0.110f));

        ui.AddSpacer(5);

        ui.CreateButton("Open Settings", () =>
        {
            SetEnabled(false);
            Plugin.SettingsUI.SetEnabled(true);
        }, "btnColor", new Color(0.15f, 0f, 0.110f));
    }

    public void SetEnabled(bool b)
    {
        SetActive(b);
    }

    protected override void OnClosePanelClicked()
    {
        // close all panels
        Plugin.Instance.EnableUI(false);
    }
}
