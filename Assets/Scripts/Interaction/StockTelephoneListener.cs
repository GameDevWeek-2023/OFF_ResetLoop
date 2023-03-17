using System;
using ScriptableObjects;
using UnityEngine;

namespace Interaction
{
    public class StockTelephoneListener : SimpleItemPickupInteraction
    {
        [SerializeField] private Sprite lowMoneySprite;
        [SerializeField] private Sprite highMoneySprite;
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
                    GameEvents.Instance.OnDialogueStart("Telephone_Low_Money.json", null);
                }
                else
                {
                    item = Item.MONEY_RICH;
                    spriteRenderer.sprite = highMoneySprite;
                    //TODO sprite
                    GameEvents.Instance.OnDialogueStart("Telephone_High_Money.json", null);
                }
                clickable = true;
                spriteRenderer.enabled = true;
            }
            else
            {
                GameEvents.Instance.OnDialogueStart("Telephone_No_Stock.json", null);
            }
        }
    }
}