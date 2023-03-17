using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private MouseCursorSO[] _mouseCursorArray;
    private InventoryItem[] _inventoryItemScriptableObjects;
    private Dictionary<Item, InventoryItem> _itemToScriptableObject = new Dictionary<Item, InventoryItem>();
    private Dictionary<KeyEvent, bool> _keyeventToActivated = new Dictionary<KeyEvent, bool>();
    private List<KeyEvent> _permanentKeyEvents = new List<KeyEvent>();
    private bool _timeRunning = false;
    
    
    private Scene _currentScene = Scene.Bedroom;

    public Scene CurrentScene => _currentScene;

    public enum MouseCursor
    {
        DEFAULT,
        FOOT,
        SPEECH,
        INSPECT,
        ARROW_UP,
        ARROW_RIGHT,
        ARROW_DOWN,
        ARROW_LEFT
    };


    public enum Scene
    {
        Bedroom,
        Street,
        Telephone,
        JonasDebug1,
        JonasDebug2,
        End
    };

    public enum KeyEvent
    {
        BEGGAR_AWAKE,
        BEER_TAKEN,
        DOG_AVAIABLE,
        KIOSK_OWNER_GONE, 
        BEGGAR_SAVED
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

        foreach (KeyEvent keyEvent in Enum.GetValues(typeof(KeyEvent)))
        {
            _keyeventToActivated.Add(keyEvent, false);
        }

        _mouseCursorArray = Resources.LoadAll<MouseCursorSO>("MouseCursor");
        OnMouseCursorChange(MouseCursor.DEFAULT);
        _permanentKeyEvents.Add(KeyEvent.BEGGAR_SAVED);
    }

    private void Start()
    {
        GameEvents.Instance.OnItemFound += OnItemFound;
        GameEvents.Instance.OnDialogueStart += delegate { StopTime(); };
        GameEvents.Instance.OnDialogueClosed += StartTime;
        GameEvents.Instance.OnInventoryItemSelected += delegate(Item item)
        {
            Debug.Log("Item selected: " + item);
            _currentlySelectedInventoryItem = item;
        };
        GameEvents.Instance.OnInventoryItemConsumed += delegate { _currentlySelectedInventoryItem = Item.NULL_ITEM; };
        GameEvents.Instance.OnItemRemoved += OnItemRemoved;
        GameEvents.Instance.OnSceneChange += OnSceneChange;
        GameEvents.Instance.OnKeyEvent += OnKeyEvent;
        GameEvents.Instance.OnMouseCursorChange += OnMouseCursorChange;
        GameEvents.Instance.OnWorldReset += OnWorldReset;
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
            case Scene.End:
            case Scene.JonasDebug1:
            case Scene.JonasDebug2:
                SceneManager.LoadScene(scene.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
        }
        _currentScene = scene;
    }

    public void OnWorldReset()
    {
        Debug.Log("RESET");
        _time = 0;
        OnSceneChange(Scene.Bedroom);
        _inventory.Clear();
        _everythingEverywhereAllAtOnce.Clear();
        foreach (KeyEvent keyEvent in _keyeventToActivated.Keys)
        {
            if (_permanentKeyEvents.Contains(keyEvent))
            {
                continue;
            }
            _keyeventToActivated[keyEvent] = false;
        }
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
        if (!_timeRunning)
        {
            StartTime();
        }
    }

    public bool ItemExists(Item item)
    {
        return _inventory.Contains(item);
    }

    private void StartTime()
    {
        InvokeRepeating(nameof(Tick), 0f, 1f);
        _timeRunning = true;
    }

    private void StopTime()
    {
        CancelInvoke(nameof(Tick));
        _timeRunning = false;
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
        Debug.Log(_time);
        if (_time == 60)
        {
            GameEvents.Instance.OnWorldReset?.Invoke();
        }
        else
        {
            GameEvents.Instance.OnTimeChanged?.Invoke(_time);
        }

    }

    private void OnKeyEvent(KeyEvent keyEvent)
    {
        _keyeventToActivated[keyEvent] = true;
    }

    public bool HasKeyEventHappend(KeyEvent keyEvent)
    {
        return _keyeventToActivated[keyEvent];
    }

    public void OnMouseCursorChange(MouseCursor mouseCursorState)
    {
        MouseCursorSO mouseCursorSo = _mouseCursorArray.FirstOrDefault(obj => obj.MouseCursorState == mouseCursorState);
        Vector2 hotSpot = Vector2.zero;
        // if (mouseCursorState == MouseCursor.DEFAULT)
        // {
        float x = mouseCursorSo.MouseCursorImage.width * 0.315f;
        float y = mouseCursorSo.MouseCursorImage.height * 0.21f;
        hotSpot = new Vector2(x, y);
        // }
        if (mouseCursorSo is not null) Cursor.SetCursor(mouseCursorSo.MouseCursorImage, hotSpot, CursorMode.Auto);
    }
}