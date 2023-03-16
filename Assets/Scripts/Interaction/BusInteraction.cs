using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusInteraction : ItemInteraction
{
    private Animator animator;
    [SerializeField] BeggerInteraction begger;
    [SerializeField] int startDriving;

    protected override void Start()
    {
        base.Start();
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
    }

    public override void OnTimeChanged(int time)
    {
       if(time == startDriving)
        {
            animator.enabled = true;
        }
    }

    public override void OnUsableItemDrop(Item item)
    {
        
    }

    public override void SpecificMouseDownBehaviour()
    {
      
    }

    public void WakeBegger()
    {
        begger.WakeBeggar();
    }
}
