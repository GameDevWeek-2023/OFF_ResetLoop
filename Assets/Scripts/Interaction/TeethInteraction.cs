using Dialog;
using UnityEngine;

namespace Interaction
{
    public class TeethInteraction : ItemInteraction
    {
        private enum State { NO_WATER, WATER, COFFEE, BOOZE, WALKING_STICK}

        [SerializeField]
        private State state;

        [Header("Dialog files")]
        [SerializeField] private TextAsset dialogFileWater;
        [SerializeField] private TextAsset dialogFileCoffee;
        [SerializeField] private TextAsset dialogFileBooze;
        [SerializeField] private TextAsset dialogFileBoozeWalkingStick;


        [Header("Sprites")]
        [SerializeField] private Sprite garryNoWater;
        [SerializeField] private Sprite garryWater;
        [SerializeField] private Sprite garryCoffee;
        [SerializeField] private Sprite garryBooze;

        private SpriteRenderer spriteRenderer;

        protected override void Start()
        {
            base.Start();
            spriteRenderer = GetComponent<SpriteRenderer>();
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
                    // DO Nothing at the moment
                    break;
                case State.WATER:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileWater.text, garryWater);
                    break;
                case State.COFFEE:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileCoffee.text, garryCoffee);
                    break;
                case State.BOOZE:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileBooze.text, garryBooze);
                    break;
                case State.WALKING_STICK:
                    GameEvents.Instance.OnDialogueStart?.Invoke(dialogFileBoozeWalkingStick.text, garryBooze);
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
            spriteRenderer.sprite = newSprite;
        }

    }
}