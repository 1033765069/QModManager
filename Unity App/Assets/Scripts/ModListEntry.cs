﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModListEntry : MonoBehaviour
{
    public GameObject content;
    public GameObject expandIcon;
    public GameObject enabledBadge;
    public GameObject disabledBadge;
    public GameObject enableButton;
    public GameObject disableButton;
    public TextMeshProUGUI titleLabel;
    public TextMeshProUGUI descriptionLabel;

    public static string fieldColor = "CCCCCC";

    [ReadOnly]
    public bool _enabled;
    [ReadOnly]
    public bool expanded;

    public void Awake()
    {
        Refresh();
    }

    public void Initialize(IModInfo modInfo)
    {
        titleLabel.text = modInfo.DisplayName;
        descriptionLabel.Empty()
            .Populate("ID", modInfo.Id)
            .Populate("Author", modInfo.Author)
            .Populate("Version", modInfo.Version);
        if (modInfo.LoadBefore != null && modInfo.LoadBefore.Length > 0)
            descriptionLabel.Populate("Loads Before", string.Join(", ", modInfo.LoadBefore));
        if (modInfo.LoadAfter != null && modInfo.LoadAfter.Length > 0)
            descriptionLabel.Populate("Loads After", string.Join(", ", modInfo.LoadAfter));
        if (modInfo.Dependencies != null && modInfo.Dependencies.Length > 0)
            descriptionLabel.Populate("Dependencies", string.Join(", ", modInfo.Dependencies));
        enabled = modInfo.Enabled;
        Refresh();
    }

    public void ToggleEntryExpanded()
    {
        expanded = !expanded;
        RefreshExpandedState();
    }
    public void ExpandEntry()
    {
        expanded = true;
        RefreshExpandedState();
    }
    public void CollapseEntry()
    {
        expanded = false;
        RefreshExpandedState();
    }
    public void EnableMod()
    {
        enabled = true;
        RefreshEnabledState();
    }
    public void DisableMod()
    {
        enabled = false;
        RefreshEnabledState();
    }
    public void Refresh()
    {
        RefreshExpandedState();
        RefreshEnabledState();
    }
    public void RefreshExpandedState()
    {
        content.SetActive(expanded);
        expandIcon.SetActive(!expanded);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform.parent);
    }
    public void RefreshEnabledState()
    {
        enabledBadge.SetActive(enabled);
        disabledBadge.SetActive(!enabled);
        enableButton.SetActive(!enabled);
        disableButton.SetActive(enabled);
    }
}

public static class ModListEntryExtensions
{
    public static TextMeshProUGUI Populate(this TextMeshProUGUI tmpro, string key, string value)
    {
        if (tmpro.text != "") tmpro.text += "\n";
        tmpro.text += "<color=#" + ModListEntry.fieldColor + ">" + key + ":</color> " + value;
        return tmpro;
    }
    public static TextMeshProUGUI Empty(this TextMeshProUGUI tmpro)
    {
        tmpro.text = "";
        return tmpro;
    }
    public static Color ParseColor(this string s)
    {
        if (ColorUtility.TryParseHtmlString(s, out Color color)) return color;
        return Color.white;
    }
}
