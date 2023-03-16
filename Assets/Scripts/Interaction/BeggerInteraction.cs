using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeggerInteraction : ItemInteraction
{
    private enum State {SLEEPING, AWAKE}
    private State state = State.SLEEPING;

    [Header("Dialog files")]
    [SerializeField] private TextAsset dialogSleeping;
    [SerializeField] private TextAsset dialogAwake;

    [Header("Sprites")]
    //[SerializeField] 
    [SerializeField] private Sprite beggerSleeping;
    [SerializeField] private Sprite beggerAwake;

    public override void OnTimeChanged(int time)
    {
        if(time == 30)
        {
            state = State.AWAKE;
        }
    }

    public override void OnUsableItemDrop(Item item)
    {
        if(item == Item.MONEY)
        {

        }
    }

    public override void SpecificMouseDownBehaviour()
    {
        switch (state)
        {
            case State.SLEEPING:
                GameEvents.Instance.OnDialogueStart(dialogSleeping.text, beggerSleeping);
                break;
            case State.AWAKE:
                GameEvents.Instance.OnDialogueStart(dialogSleeping.text, beggerSleeping);
                break;

        }
        
    }
}
