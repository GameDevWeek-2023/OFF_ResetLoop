using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnutInteraction : ItemInteraction
{
    public override void OnTimeChanged(int time)
    {
       
    }

    public override void OnUsableItemDrop(Item item)
    {
        Debug.Log("SUICIDE");
        if(item == Item.VASE_CRUSHED)
        {
            GameEvents.Instance.OnKeyEvent?.Invoke(WorldState.KeyEvent.SUICIDE);
        }
    }

    public override void SpecificMouseDownBehaviour()
    {
        
    }
}
