using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;

public class CoffeeInteraction : SimpleItemPickupInteraction
{
    protected override void Start()
    {
        clickable = WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.KIOSK_OWNER_GONE);
    }
}
