using System.Collections.Generic;
using Interaction;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using static ItemInteraction;

public class WorldState : MonoBehaviour
{
    public static WorldState Instance;
    private int _time = 0;

    private List<Item> _inventory = new List<Item>();
    private Item _currentlySelectedInventoryItem = Item.NULL_ITEM;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryPrefab;
    private InventoryItem[] _inventoryItemScriptableObjects;
    private Dictionary<Item, InventoryItem> _itemToScriptableObject = new Dictionary<Item, InventoryItem>();
    
    public Item CurrentlySelectedInventoryItem => _currentlySelectedInventoryItem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        _inventoryItemScriptableObjects = Resources.LoadAll<InventoryItem>("InventoryItems");
        foreach (InventoryItem inventoryItemScriptableObject in _inventoryItemScriptableObjects)
        {
            _itemToScriptableObject[inventoryItemScriptableObject.Item] = inventoryItemScriptableObject;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameEvents.Instance.OnItemFound += OnItemFound;
        GameEvents.Instance.OnDialogueStart += delegate { StopTime(); };
        GameEvents.Instance.OnDialogueClosed += StartTime;
        GameEvents.Instance.OnInventoryItemSelected += delegate(Item item)
        {
            Debug.Log("Item selected: "+ item);
            _currentlySelectedInventoryItem = item;
        };
        GameEvents.Instance.OnInventoryItemConsumed += delegate { _currentlySelectedInventoryItem = Item.NULL_ITEM; };
        GameEvents.Instance.OnItemRemoved += OnItemRemoved;
        StartTime();
    }

    public bool ItemExists(Item item)
    {
        return _inventory.Contains(item);
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
        AddInventoryItemToGui(item);
    }

    private void OnItemRemoved(Item item)
    {
        _inventory.Remove(item);
        RemoveInventoryItemFromGui(item);
    }
    
    private void AddInventoryItemToGui(Item item)
    {
        GameObject newInventory = Instantiate(inventoryPrefab, inventoryPanel.transform);
        Sprite sprite = _itemToScriptableObject[item].Itemsprite;
        newInventory.GetComponent<Image>().sprite = sprite;
        newInventory.GetComponent<Button>().onClick
            .AddListener(() => GameEvents.Instance.OnInventoryItemSelected(item));
        newInventory.name = item.ToString();
    }

    private void RemoveInventoryItemFromGui(Item item)
    {
        Destroy(inventoryPanel.transform.Find(item.ToString()).gameObject);
    }
    
    private void Tick()
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