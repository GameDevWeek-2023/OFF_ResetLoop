using System;
using Interaction;
using JetBrains.Annotations;
using UnityEngine;
using static ItemInteraction;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;
    [CanBeNull] public Action<int> OnTimeChanged;
    [CanBeNull] public Action<Item> OnItemFound;
    [CanBeNull] public Action<string> OnDialogueStart;
    [CanBeNull] public Action OnDialogueClosed;
    [CanBeNull] public Action<Item> OnInventoryItemSelected;
    [CanBeNull] public Action OnInventoryItemConsumed;
    
    [CanBeNull] public Action OnWorldReset;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Debug.Log("GameEvents Instance done");
        DontDestroyOnLoad(gameObject);
    }
}