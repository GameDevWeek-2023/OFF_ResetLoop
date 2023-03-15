using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ItemInteraction : MonoBehaviour
{
    public enum Interaction
    {
        // Prefixes: BED_, STR_, KIO_, TEL_
        BED_TEETH,
        BED_VASE,
        BED_WALKSTICK,
        BED_STOCK,
        STR_NEWSPAPER,
        STR_HOMELESS,
        STR_ALCOHOL,
        STR_BUSINESSMAN,
        STR_DOG,
        KIO_RADIO,
        KIO_OWNER,
        KIO_COFFEE,
        TEL_PHONE,
        TEL_PHONEBOOK,
        TEL_MONEY
    }

    // STR_NEWSPAPER, STR_HOMELESS, STR_ALCOHOL, STR_BUSINESSMAN, STR_DOG,
    // KIO_RADIO, KIO_OWNER, KIO_COFFEE, 
    // TEL_PHONE, TEL_PHONEBOOK, TEL_MONEY

    //Simple pickup 
    // BED_VASE, BED_WALKSTICK, BED_STOCK, , 

    // 
    //STR_NEWSPAPER

    // Complicated
    // BED_TEETH, 
    public enum Item
    {
        VASE_WITH_FLOWER,
        VASE_NO_FLOWER,
        VASE_WTH_WATER,
        FLOWERS,
        WALKING_STICK_NO_BALLS,
        WALKING_STICK_WITH_BALLS,
        BALLS,
        NEWSPAPER,
        STOCK,
        ALCOHOL,
        COFFEE,
        MONEY,
        NULL_ITEM
    }

    [SerializeField] protected Interaction interactionId;
    [SerializeField] protected bool clickable;
    [SerializeField] protected Item[] possibleInteractionItems;
    protected Item droppedItem;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameEvents.Instance.OnTimeChanged += OnTimeChanged;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseEnter()
    {
    }

    private void OnMouseExit()
    {
    }

    private void OnMouseDown()
    {
        Item currentlySelectedInventoryItem = WorldState.Instance.CurrentlySelectedInventoryItem;
        if (currentlySelectedInventoryItem == Item.NULL_ITEM)
        {
            Debug.Log("Null item selected");
            SpecificMouseDownBehaviour();
        }
        else
        {
            Debug.Log(" item selected" + currentlySelectedInventoryItem);
            OnItemDrop(currentlySelectedInventoryItem);
        }

        GameEvents.Instance.OnInventoryItemConsumed();
    }

    public void AddToInventory(Item item)
    {
        GameEvents.Instance.OnItemFound(item);
    }

    public void OnItemDrop(Item item)
    {
        if (possibleInteractionItems.Contains(item))
        {
            droppedItem = item;
            OnUsableItemDrop(item);
        }
        else
        {
            // TODO Dialog/Sound "Das geht nicht"
        }
    }

    public abstract void OnUsableItemDrop(Item item);
    public abstract void SpecificMouseDownBehaviour();

    public abstract void OnTimeChanged(int time);
}