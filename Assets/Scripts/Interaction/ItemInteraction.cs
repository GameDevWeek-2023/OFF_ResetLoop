using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ItemInteraction : MonoBehaviour
{
    public enum Interaction
    {
        // Prefixes: BED_, STR_, KIO_, TEL_
        BED_TEETH, BED_VASE, BED_WALKSTICK, BED_STOCK, 
        STR_NEWSPAPER, STR_HOMELESS, STR_ALCOHOL, STR_BUSINESSMAN, STR_DOG,
        KIO_RADIO, KIO_OWNER, KIO_COFFEE, 
        TEL_PHONE, TEL_PHONEBOOK, TEL_MONEY
    }

    public enum Item
    {
        VASE_NO_FLOWER, VASE_WTH_WATER, FLOWERS, WALKING_STICK_NO_BALLS, WALKING_STICK_WITH_BALLS, BALLS, 
        NEWSPAPER, STOCK, ALCOHOL, COFFEE, MONEY
    }

    [SerializeField] private Interaction interactionId;
    [SerializeField] private bool clickable;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.OnTimeChanged += delegate(int i) {
            OnTimeChanged();
        };
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseEnter()
    {
        throw new NotImplementedException();
    }

    private void OnMouseExit()
    {
        throw new NotImplementedException();
    }

    private void OnMouseDown()
    {
        SpecificMouseDownBehaviour();
    }

    public void AddToInventory(Item item)
    {
        
    }
    
    public abstract void SpecificMouseDownBehaviour();

    public abstract void OnTimeChanged();
}