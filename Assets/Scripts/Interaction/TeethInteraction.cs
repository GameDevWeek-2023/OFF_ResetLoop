using Dialog;
using UnityEngine;

namespace Interaction
{
    public class TeethInteraction : ItemInteraction
    {
        private enum State { NO_WATER, WATER, COFFEE, BOOZE}

        [SerializeField]
        private State state;

        [Header("Dialog files")]
        [SerializeField] private TextAsset dialogFileWater;
        [SerializeField] private TextAsset dialogFileCoffee;
        [SerializeField] private TextAsset dialogFileBooze;

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
            switch (state)
            {
                case State.NO_WATER:
                    GameEvents.Instance.OnDialogueStart(dialogFileWater.text);
                    break;
                case State.WATER:
                    GameEvents.Instance.OnDialogueStart(dialogFileWater.text);
                    break;
                case State.COFFEE:
                    GameEvents.Instance.OnDialogueStart(dialogFileCoffee.text);
                    break;
                case State.BOOZE:
                    GameEvents.Instance.OnDialogueStart(dialogFileBooze.text);
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
                case Item.VASE_WTH_WATER:
                    UpdateState(State.WATER, garryWater);
                    break;
                case Item.COFFEE:
                    UpdateState(State.COFFEE, garryCoffee);
                    break;
                case Item.ALCOHOL:
                    UpdateState(State.BOOZE, garryBooze);
                    break;
            }
            SpecificMouseDownBehaviour();
        }


        private void UpdateState(State newState, Sprite newSprite)
        {
            state = newState;
            spriteRenderer.sprite = newSprite;
        }

    }
}