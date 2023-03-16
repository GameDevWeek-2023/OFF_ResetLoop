using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class ItemInteraction : MonoBehaviour
{
    public enum Interaction
    {
        // Prefixes: BED_, STR_, KIO_, TEL_
        BED_TEETH,
        BED_VASE,
        BED_WALKSTICK,
        BED_STOCK,
        BED_DISMENTAL_ITEM,
        STR_NEWSPAPER,
        STR_HOMELESS,
        STR_ALCOHOL,
        STR_BUSINESSMAN,
        STR_DOG,
        STR_BUS,
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
        VASE_EMPTY,
        VASE_WITH_WATER,
        VASE_CRUSHED,
        FLOWERS,
        WALKING_STICK_NO_BALLS,
        WALKING_STICK_WITH_BALLS,
        WALKING_STICK_CRUSHED,
        BALLS,
        NEWSPAPER,
        STOCK,
        ALCOHOL,
        COFFEE,
        MONEY,
        MONEY_RICH,
        DISMANTLE_ITEM,
        NULL_ITEM
    }

    [SerializeField] protected Interaction interactionId;
    [SerializeField] protected bool clickable = true;
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

    public void RemoveFromInventory(Item item)
    {
        GameEvents.Instance.OnItemRemoved(item);
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