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

    private int time;

    [Header("Items")]
    [SerializeField] private GameObject beer;
    private BeerInteraction beerInteraction;

    [Header("Dialog files")]
    [SerializeField] private TextAsset dialogSleeping;
    [SerializeField] private TextAsset dialogAwake;

    [Header("Sprites")]
    [SerializeField] private Sprite beggerSleeping;
    [SerializeField] private Sprite beggarAwake;
    [SerializeField] private Sprite beggerSleepingHead;
    [SerializeField] private Sprite beggerAwakeHead;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        beerInteraction = beer.GetComponent<BeerInteraction>();
      

        Debug.Log($"scene {scene} loaded, beggar awake: {WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.BEGGAR_AWAKE)}");
        if (WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.BEGGAR_AWAKE)){
            WakeBeggar(false);
        } else
        {
            InitSleepingBeggar();
        }
    }


    public override void OnTimeChanged(int time)
    {
        this.time = time;
       
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

    public void WakeBeggar(bool triggeredByEvent)
    {
        Debug.Log("Begger Awake" + triggeredByEvent);
        state = State.AWAKE;
        spriteRenderer.sprite = beggarAwake;
        gameObject.transform.position = new Vector3(3, -1, 0);
        collider.size = new Vector2(1.6f, 4);

        if(beer != null) { //beer might be already taken
            beer.SetActive(true);
        }

        if (triggeredByEvent)
        {
            GameEvents.Instance.OnKeyEvent(WorldState.KeyEvent.BEGGAR_AWAKE);
        }
    }

    private void InitSleepingBeggar()
    {
        spriteRenderer.sprite = beggerSleeping;
        gameObject.transform.position = new Vector3(2.3f, -2.3f, 0);
        collider.size = new Vector2(4.3f, 2);

        beerInteraction.DeactivateBeer();
        beer.SetActive(false);
    }
}
