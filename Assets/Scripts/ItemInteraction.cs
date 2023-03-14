using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ItemInteraction : MonoBehaviour
{
    public enum Item
    {
        VASE
    }

    [SerializeField] private Item itemId;
    [SerializeField] private bool talkable;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.OnTimeChanged += delegate(int i) {
            OnTimeChanged();
        };
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseEnter()
    {
        throw new NotImplementedException();
    }

    private void OnMouseExit()
    {
        throw new NotImplementedException();
    }

    private void OnMouseDown()
    {
        SpecificMouseDownBehaviour();
    }

    public abstract void SpecificMouseDownBehaviour();

    public abstract void OnTimeChanged();
}