using System;
using System.Collections.Generic;
using Interaction;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public enum Scene
    {
        Bedroom,
        Street,
        Kiosk, 
        JonasDebug1,
        JonasDebug2
    };
    
    public Item CurrentlySelectedInventoryItem => _currentlySelectedInventoryItem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _inventoryItemScriptableObjects = Resources.LoadAll<InventoryItem>("InventoryItems");
        foreach (InventoryItem inventoryItemScriptableObject in _inventoryItemScriptableObjects)
        {
            _itemToScriptableObject[inventoryItemScriptableObject.Item] = inventoryItemScriptableObject;
        }
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
        GameEvents.Instance.OnSceneChange += OnSceneChange;
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartTime();
    }

    public void OnSceneChange(Scene scene)
    {
        switch (scene)
        {
            case Scene.Bedroom:
            case Scene.Street:
            case Scene.Kiosk:
            case Scene.JonasDebug1:
            case Scene.JonasDebug2:
                SceneManager.LoadScene(scene.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
        }
    }

    public void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode sceneMode)
    {
        inventoryPanel = GameObject.Find("InventoryPanel");
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
        newInventory.GetComponent<Tooltip>().Message = _itemToScriptableObject[item].ItemDescription;
        newInventory.GetComponent<Tooltip>().DismantledItems = _itemToScriptableObject[item].DismantledItems;
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