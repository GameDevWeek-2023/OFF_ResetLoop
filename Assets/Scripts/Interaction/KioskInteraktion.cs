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
        GameEvents.Instance.OnCall += OnCall;
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

    private void OnCall(TelephoneController.CallType obj)
    {
        //TODO
    }


}
