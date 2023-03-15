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

        public override void OnTimeChanged(int time)
        {
            if (time == 5)
            {
                Destroy(gameObject);
            }
        }
        
        public override void OnUsableItemDrop(Item item)
        {
        }
    }
}