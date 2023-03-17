using Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskInteraktion : ItemInteraction
{
    [Header("Dialog files")]
    [SerializeField] private TextAsset dialogStandard;
   
    [Header("Sprites")]
    [SerializeField] private Sprite kioskSellerHead;

    protected override void Start()
    {
        base.Start();
        if (WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.KIOSK_OWNER_GONE))
        {
            Destroy(gameObject);
        };
    }

    public override void OnTimeChanged(int time)
    {
        
    }

    public override void OnUsableItemDrop(Item item)
    {
        
    }

    public override void SpecificMouseDownBehaviour()
    {
        GameEvents.Instance.OnDialogueStart?.Invoke(dialogStandard.text, kioskSellerHead);
    }
}
