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

        protected override void Start()
        {
            base.Start();
            GameEvents.Instance.OnCall += delegate(TelephoneController.CallType callType)
            {
                if (callType == TelephoneController.CallType.STOCK) OnStockCall();
            };
            gameObject.SetActive(true);
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
        }

        private void OnStockCall()
        {
            if (WorldState.Instance.ItemExists(ItemInteraction.Item.STOCK))
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                if (WorldState.Instance.Time > 0)
                {
                    item = Item.MONEY;
                    spriteRenderer.sprite = lowMoneySprite;
                    //TODO sprite, Dialog
                    GameEvents.Instance.OnDialogueStart(dialogLowMoney.text, phoneSprite);
                }
                else
                {
                    item = Item.MONEY_RICH;
                    spriteRenderer.sprite = highMoneySprite;
                    //TODO sprite
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
    }
}