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
                }
                else
                {
                    item = Item.MONEY_RICH;
                    spriteRenderer.sprite = highMoneySprite;
                    GameEvents.Instance.OnDialogueStart(dialogHighMoney.text, phoneSprite);
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