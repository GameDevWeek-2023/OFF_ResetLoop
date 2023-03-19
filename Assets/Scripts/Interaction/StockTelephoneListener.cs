using System;
using ScriptableObjects;
using UnityEngine;

namespace Interaction
{
    public class StockTelephoneListener : SimpleItemPickupInteraction
    {
        [SerializeField] private Sprite lowMoneySprite;
        [SerializeField] private Sprite highMoneySprite;
        [SerializeField] private Sprite phoneSprite;
        [SerializeField] private TextAsset dialogLowMoney;
        [SerializeField] private TextAsset dialogHighMoney;
        [SerializeField] private TextAsset dialogNoStock;
        private SpriteRenderer _spriteRenderer;
        private Action<TelephoneController.CallType> _instanceOnCall;

        private enum State { NO_MONEY = 0, LITTLE_MONEY = 1, MUCH_MONEY = 2 }
        private State state;

        protected override void Start()
        {
            base.Start();
            _instanceOnCall = delegate(TelephoneController.CallType callType)
            {
                if (callType == TelephoneController.CallType.STOCK) OnStockCall();
            };
            GameEvents.Instance.OnCall += _instanceOnCall;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;


            state = (State) WorldState.Instance.GetKeyeventState(WorldState.KeyEvent.GARRY);

            switch (state)
            {
                case State.LITTLE_MONEY:
                    _spriteRenderer.sprite = lowMoneySprite;
                    _spriteRenderer.enabled = true;
                    clickable = true;
                    break;
                case State.MUCH_MONEY:
                    _spriteRenderer.sprite = highMoneySprite;
                    _spriteRenderer.enabled = true;
                    clickable = true;
                    break;
            }
            
        }

        private void OnStockCall()
        {
            if (WorldState.Instance.ItemExists(Item.STOCK))
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                if (WorldState.Instance.Time > 30)
                {
                    item = Item.MONEY;
                    spriteRenderer.sprite = lowMoneySprite;
                    GameEvents.Instance.OnDialogueStart(dialogLowMoney.text, phoneSprite);
                    GameEvents.Instance.OnKeyEventState?.Invoke(new KeyEventState(WorldState.KeyEvent.GARRY, (int)State.LITTLE_MONEY));
                }
                else
                {
                    item = Item.MONEY_RICH;
                    spriteRenderer.sprite = highMoneySprite;
                    GameEvents.Instance.OnDialogueStart(dialogHighMoney.text, phoneSprite);
                    GameEvents.Instance.OnKeyEventState?.Invoke(new KeyEventState(WorldState.KeyEvent.GARRY, (int)State.MUCH_MONEY));
                }
                clickable = true;
                spriteRenderer.enabled = true;
            }
            else
            {
                GameEvents.Instance.OnDialogueStart(dialogNoStock.text, phoneSprite);
            }
        }

        private void OnDisable()
        {
            GameEvents.Instance.OnCall -= _instanceOnCall;
        }
        
    }
}