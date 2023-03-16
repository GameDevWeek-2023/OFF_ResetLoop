using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipController : MonoBehaviour
{
    public static TooltipController Instance;

    [SerializeField] GameObject tooltip;
    TMP_Text tooltipText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        tooltipText = tooltip.GetComponentInChildren<TMP_Text>();
        tooltipText.text = "Test";
    }

    private void Update()
    {
        SetPosition();
    }

    public void Show(string message)
    {
        tooltipText.text = message;
        SetPosition();
        tooltip.SetActive(true);
    }

    private void SetPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.x += 2f;
        mousePosition.y -= 0.5f;
        mousePosition.z += Camera.main.nearClipPlane;
        tooltip.transform.position = mousePosition;
    }

    public void Hide()
    {
        tooltip.SetActive(false);
    }


}
