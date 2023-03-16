using System;
using UnityEngine;

namespace Interaction
{
    public class TelephoneButton : MonoBehaviour
    {
        [SerializeField] private TelephoneController.Button _buttonAction;
        private void OnMouseDown()
        {
            GameEvents.Instance.OnButtonDialed(_buttonAction);
        }
    }
}