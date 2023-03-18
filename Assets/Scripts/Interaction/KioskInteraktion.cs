using Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskInteraktion : ItemInteraction
{
    [Header("Dialog files")]
    [SerializeField] private TextAsset dialogStandard;
    [SerializeField] private TextAsset dialogMoney;
    [SerializeField] private TextAsset dialogMoneyTooMuch;
    [SerializeField] private TextAsset dialogFlowers;

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
        switch (item)
        {
            case Item.MONEY_RICH:
                GameEvents.Instance.OnDialogueStart?.Invoke(dialogMoneyTooMuch.text, kioskSellerHead);
                break;
            case Item.MONEY:
                //TODO: Dialog, Dialog event => geld nehmen und kaffee freischalten
                GameEvents.Instance.OnDialogueStart?.Invoke(dialogMoney.text, kioskSellerHead);
                break;
            case Item.FLOWERS:
                GameEvents.Instance.OnDialogueStart?.Invoke(dialogFlowers.text, kioskSellerHead);
                RemoveFromInventory(Item.FLOWERS);
                break;
            case Item.VASE_WITH_FLOWER:
                GameEvents.Instance.OnDialogueStart?.Invoke(dialogFlowers.text, kioskSellerHead);
                RemoveFromInventory(Item.VASE_WITH_FLOWER);
                AddToInventory(Item.VASE_WITH_WATER);
                break;
        }
    }

    public override void SpecificMouseDownBehaviour()
    {
        GameEvents.Instance.OnDialogueStart?.Invoke(dialogStandard.text, kioskSellerHead);
    }
}
