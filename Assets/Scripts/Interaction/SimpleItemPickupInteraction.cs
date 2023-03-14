using UnityEngine;

namespace Interaction
{
    public class SimpleItemPickupInteraction : ItemInteraction
    {

        [SerializeField] private Item item;

        public override void SpecificMouseDownBehaviour()
        {
            if (clickable)
            {
                AddToInventory(item);
                Destroy(gameObject);
            }
            
        }

        public override void OnTimeChanged()
        {
            throw new System.NotImplementedException();
        }
    }
}