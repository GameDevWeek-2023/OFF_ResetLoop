using System.Collections.Generic;
using Interaction;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    private int _time = 0;
        
    private List<ItemInteraction.Item> _inventory = new List<ItemInteraction.Item>();

    private void Start()
    {
        GameEvents.Instance.OnItemFound += OnItemFound;
        GameEvents.Instance.OnDialogueOpened += StopTime;
        GameEvents.Instance.OnDialogueClosed += StartTime;
        StartTime();
    }

    private void StartTime()
    {
        InvokeRepeating(nameof(Tick), 1f, 1f);
    }
        
    private void StopTime()
    {
        CancelInvoke(nameof(Tick));
    }

    private void OnItemFound(ItemInteraction.Item item)
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