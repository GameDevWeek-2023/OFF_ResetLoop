using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using System;

public class CoffeeInteraction : SimpleItemPickupInteraction
{
    protected override void Start()
    {
        base.Start();
        GameEvents.Instance.OnDialogueTag += OnDialogueTag;
        clickable = WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.KIOSK_OWNER_GONE);
    }

    private void OnDialogueTag(string tag)
    {
        if (tag == "COFFEE_ENABLED")
        {
            clickable = true;
            RemoveFromInventory(Item.MONEY);
        }
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnDialogueTag -= OnDialogueTag;
    }
}
