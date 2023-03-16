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
        else
        {
            Destroy(gameObject);
        }
        _tooltip = GameObject.Find("ToolTip").transform.Find("ToolTipPanel").gameObject;
        tooltipText = _tooltip.GetComponentInChildren<TMP_Text>();
        tooltipText.text = "Test";
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
        _tooltip.SetActive(true);
    }

    private void SetPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.x += 2f;
        mousePosition.y -= 0.5f;
        mousePosition.z += Camera.main.nearClipPlane;
        _tooltip.transform.position = mousePosition;
    }

    public void Hide()
    {
        _tooltip.SetActive(false);
    }


}
