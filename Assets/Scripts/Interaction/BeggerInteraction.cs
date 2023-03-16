using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeggerInteraction : ItemInteraction
{
    private enum State {SLEEPING, AWAKE}
    private State state = State.SLEEPING;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;

    [Header("Items")]
    [SerializeField] private BeerInteraction beer;

    [Header("Dialog files")]
    [SerializeField] private TextAsset dialogSleeping;
    [SerializeField] private TextAsset dialogAwake;

    [Header("Sprites")]
    [SerializeField] private Sprite beggerSleeping;
    [SerializeField] private Sprite beggerAwake;
    [SerializeField] private Sprite beggerSleepingHead;
    [SerializeField] private Sprite beggerAwakeHead;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = beggerSleeping;
        gameObject.transform.position = new Vector3(4, -1.6f, 0);
        collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(6, 3);
    }

    public override void OnTimeChanged(int time)
    {

       
    }

    public override void OnUsableItemDrop(Item item)
    {
        if(state == State.AWAKE && item == Item.MONEY)
        {

        }
        else if (state == State.AWAKE && item == Item.MONEY_RICH)
        {

        }
    }

    public override void SpecificMouseDownBehaviour()
    {
        switch (state)
        {
            case State.SLEEPING:
                GameEvents.Instance.OnDialogueStart(dialogSleeping.text, beggerSleeping);
                break;
            case State.AWAKE:
                GameEvents.Instance.OnDialogueStart(dialogSleeping.text, beggerSleeping);
                break;

        }
    }

    public void WakeBeggar()
    {
        state = State.AWAKE;
        spriteRenderer.sprite = beggerAwake;
        gameObject.transform.position = new Vector3(3, -1.3f, 0);
        collider.size = new Vector2(1.7f, 4);

        beer.ActivateBeer();
    }
}
