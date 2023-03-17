using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction
{
    public class TelephoneButton : MonoBehaviour
    {
        [SerializeField] private TelephoneController.Button _buttonAction;
        private void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GameEvents.Instance.OnButtonDialed(_buttonAction);
            }
        }
    }
}