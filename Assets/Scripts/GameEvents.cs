using System;
using Interaction;
using JetBrains.Annotations;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;
    [CanBeNull] public Action<int> OnTimeChanged;
    [CanBeNull] public Action<ItemInteraction.Item> OnItemFound;
    [CanBeNull] public Action OnDialogueOpened;
    [CanBeNull] public Action OnDialogueClosed;
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