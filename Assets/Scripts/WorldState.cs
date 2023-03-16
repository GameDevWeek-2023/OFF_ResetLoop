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
    private HashSet<Item> _everythingEverywhereAllAtOnce = new HashSet<Item>();
    private Item _currentlySelectedInventoryItem = Item.NULL_ITEM;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryPrefab;
    private InventoryItem[] _inventoryItemScriptableObjects;
    private Dictionary<Item, InventoryItem> _itemToScriptableObject = new Dictionary<Item, InventoryItem>();
    private Dictionary<KeyEvent, bool> _keyeventToActivated = new Dictionary<KeyEvent, bool>();

    private Scene _currentScene = Scene.Bedroom;

    public Scene CurrentScene => _currentScene;

    public enum Scene
    {
        Bedroom,
        Street,
        Telephone, 
        JonasDebug1,
        JonasDebug2
    };

    public enum KeyEvent
    {
        BEGGAR_AWAKE,
        BEER_TAKEN,
        DOG_AVAIABLE
    }

    public Item CurrentlySelectedInventoryItem => _currentlySelectedInventoryItem;

    public int Time => _time;

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

        foreach(KeyEvent keyEvent in Enum.GetValues(typeof(KeyEvent)))
        {
            _keyeventToActivated.Add(keyEvent, false);

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
        GameEvents.Instance.OnKeyEvent += OnKeyEvent;
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartTime();
    }


    public void OnSceneChange(Scene scene)
    {
        switch (scene)
        {
            case Scene.Bedroom:
            case Scene.Street:
            case Scene.Telephone:
            case Scene.JonasDebug1:
            case Scene.JonasDebug2:
                SceneManager.LoadScene(scene.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
        }

        _currentScene = scene;
    }

    public void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode sceneMode)
    {
        inventoryPanel = GameObject.Find("InventoryPanel");
        GameObject[] findGameObjectsWithTag = GameObject.FindGameObjectsWithTag("SimpleItemPickup");
        foreach (GameObject simpleItemPickupGO in findGameObjectsWithTag)
        {
            Item sceneItem = simpleItemPickupGO.GetComponent<SimpleItemPickupInteraction>().Item;
            if (_everythingEverywhereAllAtOnce.Contains(sceneItem))
            {
                Destroy(simpleItemPickupGO);
            }
        }
        LoadFullInventory();
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
        _everythingEverywhereAllAtOnce.Add(item);
        AddInventoryItemToGui(item);
    }

    private void OnItemRemoved(Item item)
    {
        _inventory.Remove(item);
        RemoveInventoryItemFromGui(item);
    }

    private void LoadFullInventory()
    {
        foreach (Item item in _inventory)
        {
            AddInventoryItemToGui(item);
        }
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
        Transform find = inventoryPanel.transform.Find(item.ToString());
        if (find != null)
        {
            Destroy(find.gameObject);
        }
    }
    
    private void Tick()
    {
        _time++;

        if (_time == 60)
        {
            GameEvents.Instance.OnWorldReset?.Invoke();
            _time = 0;
        }

        GameEvents.Instance.OnTimeChanged?.Invoke(_time);
    }

    private void OnKeyEvent(KeyEvent keyEvent)
    {
        _keyeventToActivated[keyEvent] = true;
    }

    public bool HasKeyEventHappend(KeyEvent keyEvent)
    {
        return _keyeventToActivated[keyEvent];
    }
}