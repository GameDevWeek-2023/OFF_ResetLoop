using Dialog;
using System;
using UnityEngine;

namespace Interaction
{
    public class TeethInteraction : ItemInteraction
    {
        private enum State { NO_WATER=0, WATER=1, COFFEE=2, BOOZE=3, WALKING_STICK=4, ASPERIN=5}

        [SerializeField]
        private State state;

        [Header("Dialog files")]
        [SerializeField] private TextAsset dialogFileEmpty;
        [SerializeField] private TextAsset dialogFileWater;
        [SerializeField] private TextAsset dialogFileCoffee;
        [SerializeField] private TextAsset dialogFileBooze;
        [SerializeField] private TextAsset dialogFileBoozeWalkingStick;
        [SerializeField] private TextAsset dialogFileFlowers;
        [SerializeField] private TextAsset dialogFileAsperin;


        [Header("Sprites")]
        [SerializeField] private Sprite garryNoWater;
        [SerializeField] private Sprite garryWater;
        [SerializeField] private Sprite garryCoffee;
        [SerializeField] private Sprite garryBooze;
        [SerializeField] private Sprite garryAsperin;

        private Sprite activeSprite;

        private SpriteRenderer spriteRenderer;

        protected override void Start()
        {
            base.Start();
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            GameEvents.Instance.OnWorldReset += OnWorldReset;

            //Init
            state = (State) WorldState.Instance.GetKeyeventState(WorldState.KeyEvent.GARRY);

            switch (state)
            {
                case State.NO_WATER:
                    activeSprite = garryNoWater;
                    break;
                case State.WATER:
                    activeSprite = garryWater;
                    break;
                case State.COFFEE:
                    activeSprite = garryCoffee;
                    break;
                case State.BOOZE:
                    activeSprite = garryBooze;
                    break;
                case State.ASPERIN:
                    activeSprite = garryAsperin;
                    break;
            }
            spriteRenderer.sprite = activeSprite;

        }

        private void OnWorldReset()
        {
            UpdateState(State.NO_WATER, garryNoWater);
        }

        public override void SpecificMouseDownBehaviour()
        {
            StartDialog();
        }

        private void StartDialog()
        {
            switch (state)
            {
                case State.NO_WATER:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileEmpty.text, activeSprite);
                    break;
                case State.WATER:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileWater.text, activeSprite);
                    break;
                case State.COFFEE:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileCoffee.text, activeSprite);
                    break;
                case State.BOOZE:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileBooze.text, activeSprite);
                    break;
                case State.WALKING_STICK:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileBoozeWalkingStick.text, activeSprite);
                    break;
                case State.ASPERIN:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileAsperin.text, activeSprite);
                    break;
            }
        }
            

        public override void OnTimeChanged(int time)
        {
            
        }


        public override void OnUsableItemDrop(Item item)
        {
            switch (item)
            {
                case Item.VASE_WITH_WATER:
                    UpdateState(State.WATER, garryWater);
                    RemoveFromInventory(Item.VASE_WITH_WATER);
                    AddToInventory(Item.VASE_EMPTY);
                    break;
                case Item.VASE_WITH_FLOWER:
                    UpdateState(State.WATER, garryWater);
                    RemoveFromInventory(Item.VASE_WITH_FLOWER);
                    AddToInventory(Item.VASE_EMPTY);
                    AddToInventory(Item.FLOWERS);
                    break;
                case Item.COFFEE:
                    UpdateState(State.COFFEE, garryCoffee);
                    RemoveFromInventory(Item.COFFEE);
                    break;
                case Item.ALCOHOL:
                    UpdateState(State.BOOZE, garryBooze);
                    RemoveFromInventory(Item.ALCOHOL);
                    break;
                case Item.WALKING_STICK_NO_BALLS:
                    ProcessWalkingStick();
                    break;
                case Item.WALKING_STICK_WITH_BALLS:
                    ProcessWalkingStick();
                    break;
                case Item.FLOWERS:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileBoozeWalkingStick.text, activeSprite);
                    break;
                case Item.ASPERIN:
                    UpdateState(State.ASPERIN, garryAsperin);
                    RemoveFromInventory(Item.ASPERIN);
                    break;
            }
            StartDialog();
        }

        
        private void ProcessWalkingStick()
        {
            if(state == State.BOOZE)
            {
                state = State.WALKING_STICK;
                GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileBoozeWalkingStick.text, garryBooze);
                
                RemoveFromInventory(Item.WALKING_STICK_NO_BALLS);
                AddToInventory(Item.WALKING_STICK_CRUSHED);
            }
        }


        private void UpdateState(State newState, Sprite newSprite)
        {
            state = newState;
            activeSprite = newSprite;
            spriteRenderer.sprite = newSprite;

            GameEvents.Instance.OnKeyEventState?.Invoke(new KeyEventState(WorldState.KeyEvent.GARRY, (int) state));
            Debug.Log("KEY EVENT STATE " + ((int)state));
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnWorldReset -= OnWorldReset;
        }

    }
}