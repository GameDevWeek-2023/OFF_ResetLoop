using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;

public class CoffeeInteraction : SimpleItemPickupInteraction
{
    protected override void Start()
    {
        clickable = false;
        GameEvents.Instance.OnCall += OnCall;
    }

    private void OnCall(TelephoneController.CallType obj)
    {
        clickable = true;
    }
}
