using Interaction;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
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
    }
}