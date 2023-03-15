using Dialog;
using UnityEngine;

namespace Interaction
{
    public class TeethInteraction : ItemInteraction
    {
        [SerializeField] private TextAsset dialogFileWater;
        [SerializeField] private TextAsset dialogFileCoffee;
        [SerializeField] private TextAsset dialogFileBooze;

        public override void SpecificMouseDownBehaviour()
        {
            GameEvents.Instance.OnDialogueStart(dialogFileWater.text);
        }

        public override void OnTimeChanged(int time)
        {
            
        }


        public override void OnUsableItemDrop(Item item)
        {
            
        }


    }
}