using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "InventoryItem", order = 0)]
    public class InventoryItem : ScriptableObject
    {
        [SerializeField] private ItemInteraction.Item item;
        [SerializeField] private ItemInteraction.Item[] dismantledItems;
        [SerializeField] private Sprite itemsprite;
        [SerializeField][TextArea] private string itemDescription;

        public ItemInteraction.Item Item => item;

        public Sprite Itemsprite => itemsprite;
    }
}