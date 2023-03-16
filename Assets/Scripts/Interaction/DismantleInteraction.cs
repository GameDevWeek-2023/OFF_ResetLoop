using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismantleInteraction : ItemInteraction
{
    public override void OnTimeChanged(int time)
    {

    }

    public override void OnUsableItemDrop(Item item)
    {
        Debug.Log("Dismantle item");
        switch (item)
        {
            case Item.VASE_WITH_FLOWER:
                RemoveFromInventory(Item.VASE_WITH_FLOWER);
                AddToInventory(Item.VASE_WITH_WATER);
                AddToInventory(Item.FLOWERS);
                break;
            case Item.VASE_EMPTY:
                RemoveFromInventory(Item.VASE_EMPTY);
                AddToInventory(Item.VASE_CRUSHED);
                break;
            case Item.WALKING_STICK_WITH_BALLS:
                RemoveFromInventory(Item.WALKING_STICK_WITH_BALLS);
                AddToInventory(Item.WALKING_STICK_NO_BALLS);
                AddToInventory(Item.BALLS);
                break;
        }
    }

    public override void SpecificMouseDownBehaviour()
    {
       
    }

   

   
}
