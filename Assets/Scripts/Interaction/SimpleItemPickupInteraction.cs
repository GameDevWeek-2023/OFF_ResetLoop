using UnityEngine;

namespace DefaultNamespace
{
    public class SimpleItemPickupInteraction : ItemInteraction
    {

        [SerializeField] private Item item;
        
        public override void SpecificMouseDownBehaviour()
        {
            AddToInventory(item);
        }

        public override void OnTimeChanged()
        {
            throw new System.NotImplementedException();
        }
    }
}