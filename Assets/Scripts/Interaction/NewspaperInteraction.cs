using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;

public class NewspaperInteraction : SimpleItemPickupInteraction
{
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void OnBus()
    {
        animator.enabled = true;
    }
}
