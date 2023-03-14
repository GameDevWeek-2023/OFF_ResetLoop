using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class TeethInteraction : ItemInteraction
    {
        
        
        public override void SpecificMouseDownBehaviour()
        {

        }

        public void newspaperGone()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = newspaper;
        }
    }
}