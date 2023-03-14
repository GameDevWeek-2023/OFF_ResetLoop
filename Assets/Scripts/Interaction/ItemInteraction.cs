using UnityEngine;

namespace Interaction
{
    public abstract class ItemInteraction : MonoBehaviour
    {
        public enum Interaction
        {
            // Prefixes: BED_, STR_, KIO_, TEL_
            BED_TEETH, BED_VASE, BED_WALKSTICK, BED_STOCK, 
            STR_NEWSPAPER, STR_HOMELESS, STR_ALCOHOL, STR_BUSINESSMAN, STR_DOG,
            KIO_RADIO, KIO_OWNER, KIO_COFFEE, 
            TEL_PHONE, TEL_PHONEBOOK, TEL_MONEY
        }
        //BED_VASE, BED_WALKSTICK,  BED_STOCK, STR_ALCOHOL, TEL_MONEY
    
        //STR_NEWSPAPER, KIO_RADIO, KIO_COFFEE
    
        // DIALOGUE
        //  BED_TEETH, STR_HOMELESS, STR_BUSINESSMAN, KIO_OWNER
  
        // MORE COMPLICATED
//        STR_DOG, TEL_PHONE, TEL_PHONEBOOK, 
        

    
    
        public enum Item
        {
            VASE_NO_FLOWER, VASE_WTH_WATER, FLOWERS, WALKING_STICK_NO_BALLS, WALKING_STICK_WITH_BALLS, BALLS, 
            NEWSPAPER, STOCK, ALCOHOL, COFFEE, MONEY
        }

        [SerializeField] private Interaction interactionId;
        [SerializeField] private bool clickable;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnMouseEnter()
        {
        
        }

        private void OnMouseExit()
        {
        
        }

        private void OnMouseDown()
        {
            SpecificMouseDownBehaviour();
        }

        public void AddToInventory(Item item)
        {
            GameEvents.Instance.OnItemFound(item);
        }
    
        public abstract void SpecificMouseDownBehaviour();

        public abstract void OnTimeChanged();
    }
}