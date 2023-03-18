using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using System;

public class NewspaperListener : MonoBehaviour
{
    private GameObject spriteObject;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteObject = animator.gameObject;
        spriteObject.SetActive(false);

        GameEvents.Instance.OnKeyEvent += OnKeyEvent;
        GameEvents.Instance.OnWorldReset += OnWorldReset;

        OnWorldReset();
    }

    private void OnWorldReset()
    {
        if (!WorldState.Instance.ItemExists(ItemInteraction.Item.NEWSPAPER) &&
            (WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.BEGGAR_AWAKE) ||
            WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.BEGGAR_SAVED)))
        {
            spriteObject.SetActive(true);
            animator.SetTrigger("Flown");
        }
    }

    private void OnKeyEvent(WorldState.KeyEvent keyEvent)
    {
        if(keyEvent == WorldState.KeyEvent.BEGGAR_AWAKE)
        {
            spriteObject.SetActive(true);
            animator.SetTrigger("Fly");
        }
    }


    private void OnDestroy()
    {
        GameEvents.Instance.OnKeyEvent -= OnKeyEvent;
        GameEvents.Instance.OnWorldReset -= OnWorldReset;
    }
}
