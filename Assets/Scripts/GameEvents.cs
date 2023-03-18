using System;
using Interaction;
using JetBrains.Annotations;
using model;
using UnityEngine;
using static ItemInteraction;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;
    [CanBeNull] public Action<int> OnTimeChanged;
    [CanBeNull] public Action<Item> OnItemFound;
    [CanBeNull] public Action<Item> OnItemRemoved;
    [CanBeNull] public Action<string, Sprite> OnDialogueStart;
    [CanBeNull] public Action OnDialogueClosed;
    [CanBeNull] public Action<string> OnDialogueTag;
    [CanBeNull] public Action<Item> OnInventoryItemSelected;
    [CanBeNull] public Action OnInventoryItemConsumed;
    [CanBeNull] public Action<Position> OnMovePlayerToPosition;
    [CanBeNull] public Action<SceneChange> OnSceneChange;
    [CanBeNull] public Action<WorldState.Scene> OnRequestSceneChange;
    [CanBeNull] public Action<TelephoneController.Button> OnButtonDialed;
    [CanBeNull] public Action<TelephoneController.CallType> OnCall;
    [CanBeNull] public Action<WorldState.KeyEvent> OnKeyEvent;
    [CanBeNull] public Action<KeyEventState> OnKeyEventState;
    [CanBeNull] public Action OnWorldReset;
    [CanBeNull] public Action OnFootStep;
    [CanBeNull] public Action<WorldState.MouseCursor> OnMouseCursorChange;
    

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
    }
}