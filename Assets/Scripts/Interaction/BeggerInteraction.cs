using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeggerInteraction : ItemInteraction
{
    private enum State {SLEEPING, AWAKE}
    private State state = State.SLEEPING;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;

    [Header("Items")]
    [SerializeField] private GameObject beer;
    private BeerInteraction beerInteraction;

    [Header("Dialog files")]
    [SerializeField] private TextAsset dialogSleeping;
    [SerializeField] private TextAsset dialogAwake;
    [SerializeField] private TextAsset dialogNotEnoughMoney;
    [SerializeField] private TextAsset dialogEnoughMoney;

    [Header("Sprites")]
    [SerializeField] private Sprite beggerSleeping;
    [SerializeField] private Sprite beggarAwake;
    [SerializeField] private Sprite beggerSleepingHead;
    [SerializeField] private Sprite beggerAwakeHead;


    protected override void Start()
    {
        base.Start();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        beerInteraction = beer.GetComponent<BeerInteraction>();

        if (!WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.BEGGAR_AWAKE) && WorldState.Instance.Time > 29)
        {
            WakeBeggar();
        }
        else if (WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.BEGGAR_AWAKE))
        {
            WakeBeggar();
        }
        else if (WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.BEGGAR_SAVED))
        {
            gameObject.SetActive(false);
        }
        else
        {
            InitSleepingBeggar();
        }
    }

    public override void OnUsableItemDrop(Item item)
    {
        if(state == State.AWAKE && item == Item.MONEY)
        {
            GameEvents.Instance.OnDialogueStart?.Invoke(dialogNotEnoughMoney.text, beggerAwakeHead);
            RemoveFromInventory(Item.MONEY);
        }
        else if (state == State.AWAKE && item == Item.MONEY_RICH)
        {
            GameEvents.Instance.OnDialogueStart?.Invoke(dialogEnoughMoney.text, beggerAwakeHead);
            beerInteraction.ActivateBeer();
            RemoveFromInventory(Item.MONEY_RICH);
            GameEvents.Instance.OnKeyEvent?.Invoke(WorldState.KeyEvent.BEGGAR_SAVED);
        }
    }

    public override void SpecificMouseDownBehaviour()
    {
        switch (state)
        {
            case State.SLEEPING:
                GameEvents.Instance.OnDialogueStart?.Invoke(dialogSleeping.text, beggerSleepingHead);
                break;
            case State.AWAKE:
                GameEvents.Instance.OnDialogueStart?.Invoke(dialogAwake.text, beggerAwakeHead);
                break;
        }
    }

    public void WakeBeggar()
    {
        Debug.Log("Begger Awake");
        state = State.AWAKE;
        spriteRenderer.sprite = beggarAwake;
        gameObject.transform.position = new Vector3(10.3f, -1, 0);
        collider.size = new Vector2(1.5f, 3.7f);

        if(beer != null) { //beer might be already taken
            beer.SetActive(true);
        }

        GameEvents.Instance.OnKeyEvent?.Invoke(WorldState.KeyEvent.BEGGAR_AWAKE);
    }

    private void InitSleepingBeggar()
    {
        spriteRenderer.sprite = beggerSleeping;
        gameObject.transform.position = new Vector3(9.4f, -2.2f, 0);
        collider.size = new Vector2(3f, 1.3f);

        beerInteraction.DeactivateBeer();
        beer.SetActive(false);
    }

    public override void OnTimeChanged(int time)
    {
        
    }
}
