using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusInteraction : ItemInteraction
{
    private Animator animator;
    [SerializeField] private BeggerInteraction begger;
    [SerializeField] private GameObject newspaper;
    [SerializeField] private int startDriving;

    private bool animationStarted = false;
    private NewspaperInteraction newspaperInteraction;

    protected override void Start()
    {
        base.Start();

        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;

        newspaper.SetActive(false);
        newspaperInteraction = newspaper.GetComponent<NewspaperInteraction>();

        int time = WorldState.Instance.Time;
        if (time >= startDriving && time <= startDriving + 2 && !animationStarted)
        {
            animationStarted = true;
            animator.enabled = true;
        }
    }

    public override void OnTimeChanged(int time)
    {
        if (!animationStarted && time == startDriving)
        {
            animationStarted = true;
            animator.enabled = true;
        }
    }


    public override void OnUsableItemDrop(Item item)
    {
        
    }

    public override void SpecificMouseDownBehaviour()
    {
      
    }

    public void WakeBeggar()
    {
        begger.WakeBeggar();
        newspaper.SetActive(true);
        newspaperInteraction.OnBus();
    }
}
