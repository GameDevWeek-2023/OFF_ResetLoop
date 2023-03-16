using UnityEngine;

namespace Interaction
{
    public class SimpleItemPickupInteraction : ItemInteraction
    {

        [SerializeField] protected Item item;
        
        public override void SpecificMouseDownBehaviour()
        {
            if (clickable)
            {
                AddToInventory(item);
                Destroy(gameObject);
            }
        }

        public override void OnTimeChanged(int time)
        {

        }
        
        public override void OnUsableItemDrop(Item item)
        {
        }
    }
}