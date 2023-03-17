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
        STR_ASPERIN,
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
        ASPERIN,
        NULL_ITEM
    }

    private Vector3 _originalScale;
    
    [SerializeField] protected Interaction interactionId;
    [SerializeField] protected bool clickable = true;
    [SerializeField] protected Item[] possibleInteractionItems;
    protected Item droppedItem;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Vector3 transformLocalScale = transform.localScale;
        _originalScale = new Vector3(transformLocalScale.x, transformLocalScale.y, transformLocalScale.z);
        GameEvents.Instance.OnTimeChanged += OnTimeChanged;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseEnter()
    {
        if (clickable)
        {
            LeanTween.scale(gameObject, _originalScale * 1.2f, 0.1f).setEaseInOutCubic();
        }
    }

    private void OnMouseExit()
    {
        if (clickable)
        {
            LeanTween.scale(gameObject, _originalScale, 0.2f).setEaseInOutCubic();
        }
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

    private void OnDestroy()
    {
        GameEvents.Instance.OnTimeChanged -= OnTimeChanged;
    }
}