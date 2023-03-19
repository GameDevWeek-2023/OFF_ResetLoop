using UnityEngine;

public class SimpleHandTooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnMouseEnter()
    {
        tooltip.SetActive(true);
    }

    private void OnMouseExit()
    {
        tooltip.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
    }
}