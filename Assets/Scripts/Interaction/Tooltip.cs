using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _message;
    [SerializeField] private ItemInteraction.Item[] _dismantledItems;

    public string Message { get => _message; set => _message = value; }
    public ItemInteraction.Item[] DismantledItems { get => _dismantledItems; set => _dismantledItems = value; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipController.Instance.Show(_message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipController.Instance.Hide();
    }
}
