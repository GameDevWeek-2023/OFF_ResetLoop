using System;
using UnityEngine;

namespace DefaultNamespace.Debug
{
    public class DebugInventoryAdder : MonoBehaviour
    {
        [SerializeField] private ItemInteraction.Item[] itemsToAdd;
        private void Start()
        {
            foreach (ItemInteraction.Item item in itemsToAdd)
            {
                GameEvents.Instance.OnItemFound(item);
            }

        }
    }
}