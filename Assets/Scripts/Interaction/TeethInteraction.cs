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
        [SerializeField] private DialogController dialogController;

        public override void SpecificMouseDownBehaviour()
        {
            dialogController.StartNewDialog(dialogFileWater.text);
        }

        public override void OnTimeChanged(int time)
        {
            
        }
    }
}