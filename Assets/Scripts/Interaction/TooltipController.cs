using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipController : MonoBehaviour
{
    public static TooltipController Instance;

    private GameObject _tooltip;
    TMP_Text tooltipText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
       
        UpdateTooltipGameObject();

    }

    private void Update()
    {
        if (isActiveAndEnabled)
        {
            SetPosition();
        }
    }

    public void Show(string message)
    {
        tooltipText.text = message;
        SetPosition();
        UpdateTooltipGameObject();
        _tooltip.SetActive(true);
    }

    private void SetPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.x += 2f;
        mousePosition.y += 1.5f;
        mousePosition.z += Camera.main.nearClipPlane;
        UpdateTooltipGameObject();
        _tooltip.transform.position = mousePosition;
    }

    public void Hide()
    {
        UpdateTooltipGameObject();
        _tooltip.SetActive(false);
    }

    private void UpdateTooltipGameObject()
    {
        if (_tooltip == null)
        {
            _tooltip = GameObject.Find("ToolTip").transform.Find("ToolTipPanel").gameObject;
            tooltipText = _tooltip.GetComponentInChildren<TMP_Text>();
            tooltipText.text = "Test";
        }
    }

}
