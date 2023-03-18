using System;
using System.Collections.Generic;
using System.Linq;
using Interaction;
using model;
using ScriptableObjects;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI clockText;
    [SerializeField] private MouseCursorSO[] _mouseCursorArray;
    private InventoryItem[] _inventoryItemScriptableObjects;
    private Dictionary<Item, InventoryItem> _itemToScriptableObject = new Dictionary<Item, InventoryItem>();
    private Dictionary<KeyEvent, int> _keyeventToActivated = new Dictionary<KeyEvent, int>();
    private List<KeyEvent> _permanentKeyEvents = new List<KeyEvent>();
    private bool _timeRunning = false;
    private int _totalCycleTimeSeconds = 60;
    private bool _dialogueOpen = false;

    public bool DialogueOpen => _dialogueOpen;

    private SceneChange _currentScene = new SceneChange(Scene.Bedroom, Scene.Bedroom);

    public Scene CurrentScene => _currentScene.NewScene;

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
        End,
        Reset
    };

    public enum KeyEvent
    {
        BEGGAR_AWAKE,
        BEER_TAKEN,
        DOG_AVAIABLE,
        KIOSK_OWNER_GONE,
        BEGGAR_SAVED,
        GARRY,
        ASPERIN, 
        LANCELOT_FLYING_HOME,
        SUICIDE,
        MURDER
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
            _keyeventToActivated.Add(keyEvent, 0);
        }

        _mouseCursorArray = Resources.LoadAll<MouseCursorSO>("MouseCursor");
        OnMouseCursorChange(MouseCursor.DEFAULT);
        _permanentKeyEvents.Add(KeyEvent.BEGGAR_SAVED);
    }

    private void Start()
    {
        GameEvents.Instance.OnItemFound += OnItemFound;
        GameEvents.Instance.OnDialogueStart += delegate
        {
            _dialogueOpen = true; StopTime(); 
        };
        GameEvents.Instance.OnDialogueClosed += delegate
        {
            _dialogueOpen = false; StartTime();
        };
        GameEvents.Instance.OnInventoryItemSelected += delegate(Item item)
        {
            Debug.Log("Item selected: " + item);
            _currentlySelectedInventoryItem = item;
        };
        GameEvents.Instance.OnInventoryItemConsumed += delegate
        {
            Debug.Log("Inventory Item Consumed");
            _currentlySelectedInventoryItem = Item.NULL_ITEM;
        };
        GameEvents.Instance.OnItemRemoved += OnItemRemoved;
        GameEvents.Instance.OnSceneChange += OnSceneChange;
        GameEvents.Instance.OnRequestSceneChange += OnRequestSceneChange;
        GameEvents.Instance.OnKeyEvent += OnKeyEvent;
        GameEvents.Instance.OnKeyEventState += OnKeyEventState;
        GameEvents.Instance.OnMouseCursorChange += OnMouseCursorChange;
        GameEvents.Instance.OnWorldReset += OnWorldReset;
        GameEvents.Instance.OnTimeChanged += delegate { UpdateGuiClock(); }; 
        SceneManager.sceneLoaded += OnSceneLoaded;
        // _time = 47;
        StartTime();
    }

    private void OnRequestSceneChange(Scene newScene)
    {
        GameEvents.Instance.OnSceneChange(new SceneChange(_currentScene.NewScene, newScene));
    }

    public void OnSceneChange(SceneChange sceneChange)
    {
        Scene scene = sceneChange.NewScene;
        switch (scene)
        {
            case Scene.Street:
            case Scene.Bedroom:
            case Scene.Telephone:
            case Scene.Reset:
            case Scene.End:
            case Scene.JonasDebug1:
            case Scene.JonasDebug2:
                SceneManager.LoadScene(scene.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
        }
        
        _currentScene = sceneChange;
    }

    public void OnWorldReset()
    {
        _time = 0;
        OnRequestSceneChange(Scene.Reset);
        _inventory.Clear();
        _everythingEverywhereAllAtOnce.Clear();
        List<KeyEvent> keyEventList = _keyeventToActivated.Keys.ToList();

        foreach (KeyEvent keyEvent in keyEventList)
        {
            if (_permanentKeyEvents.Contains(keyEvent))
            {
                continue;
            }

            _keyeventToActivated[keyEvent] = 0;
        }

        Invoke(nameof(LoadBedRoomScene), 3f);
    }

    private void LoadBedRoomScene()
    {
        GameEvents.Instance.OnRequestSceneChange(Scene.Bedroom);
        _time = 0;
    }

    public void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode sceneMode)
    {
        inventoryPanel = GameObject.Find("InventoryPanel");
        GameObject clock = GameObject.Find("clock");
        if (clock != null)
        {
            clockText = clock.GetComponent<TextMeshProUGUI>();
        }
        UpdateGuiClock();
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

        if (_currentScene.OldScene == Scene.Telephone && _currentScene.NewScene == Scene.Street)
        {
            GameObject knut = GameObject.Find("Knut");
            GameObject afterTelephonePosition = GameObject.Find("Telefon_Booth_Position");
            knut.transform.position = afterTelephonePosition.transform.position;
            Vector3 transformPosition = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(13.3f, transformPosition.y, transformPosition.z);
        }

        if (_currentScene.OldScene == Scene.Street && _currentScene.NewScene == Scene.Bedroom)
        {
            GameObject knut = GameObject.Find("Knut");
            GameObject afterTelephonePosition = GameObject.Find("From_Street_Position");
            knut.transform.position = afterTelephonePosition.transform.position;
        }
        _dialogueOpen = false;
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
        if (!_inventory.Contains(item))
        {
            _inventory.Add(item);
            _everythingEverywhereAllAtOnce.Add(item);
            AddInventoryItemToGui(item);
        }
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

    private void UpdateGuiClock()
    {
        if (clockText != null)
        {
            int startHour = 9;
            int endHour = 18;
            int totalNumberHours = endHour - startHour;
            float conversionFactor = (float) totalNumberHours / _totalCycleTimeSeconds;
            int hour = (int) Math.Floor(_time * conversionFactor) + startHour;
            float rest = _time * conversionFactor - (int) Math.Floor(_time * conversionFactor);
            Debug.Log("Rest:" + rest);
            string minute = "00";
            if (rest > 0.5)
            {
                minute = "30";
            }

            clockText.text = hour.ToString("D2") + ":" + minute;
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
        if (_time == _totalCycleTimeSeconds)
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
        _keyeventToActivated[keyEvent] = 1;

        if(keyEvent == KeyEvent.LANCELOT_FLYING_HOME)
        {
            StopTime();
        }
    }

    private void OnKeyEventState(KeyEventState keyEventState)
    {
        _keyeventToActivated[keyEventState.keyEvent] = keyEventState.state;
    }

    public bool HasKeyEventHappend(KeyEvent keyEvent)
    {
        return _keyeventToActivated[keyEvent] == 1;
    }

    public int GetKeyeventState(KeyEvent keyEvent)
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
        if (mouseCursorSo is not null)
            Cursor.SetCursor(mouseCursorSo.MouseCursorImage, hotSpot, CursorMode.ForceSoftware);
    }
}