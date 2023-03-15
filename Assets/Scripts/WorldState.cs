using System.Collections.Generic;
using Interaction;
using UnityEngine;
using static ItemInteraction;

public class WorldState : MonoBehaviour
{
    public static WorldState Instance;
    private int _time = 0;
        
    private List<Item> _inventory = new List<Item>();
    private Item _currentlySelectedInventoryItem = Item.NULL_ITEM;

    public Item CurrentlySelectedInventoryItem => _currentlySelectedInventoryItem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        GameEvents.Instance.OnItemFound += OnItemFound;
        GameEvents.Instance.OnDialogueStart += delegate { StopTime(); };
        GameEvents.Instance.OnDialogueClosed += StartTime;
        GameEvents.Instance.OnInventoryItemSelected += delegate(Item item) { _currentlySelectedInventoryItem = item; };
        GameEvents.Instance.OnInventoryItemConsumed += delegate { _currentlySelectedInventoryItem = Item.NULL_ITEM; };
        StartTime();
    }

    private void StartTime()
    {
        InvokeRepeating(nameof(Tick), 0f, 1f);
    }
        
    private void StopTime()
    {
        CancelInvoke(nameof(Tick));
    }

    private void OnItemFound(Item item)
    {
        _inventory.Add(item);
    }
        
    public void Tick()
    {
        _time++;
        if (_time == 60)
        {
            GameEvents.Instance.OnWorldReset();
            _time = 0;
        }
        GameEvents.Instance.OnTimeChanged(_time);
    }
}