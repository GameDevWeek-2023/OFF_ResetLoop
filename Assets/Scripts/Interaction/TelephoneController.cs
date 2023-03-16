using System;
using System.Linq;
using UnityEngine;

namespace Interaction
{
    public class TelephoneController : MonoBehaviour
    {
        private string _dialedNumber = "";
        private string stockNumber = "1234";
        private string kioskOwnerNumber = "11880";
        
        public enum Button
        {
            B0,
            B1,
            B2,
            B3,
            B4,
            B5,
            B6,
            B7,
            B8,
            B9,
            B_DIAL
        };

        public enum CallType
        {
            STOCK, KIOSK_OWNER, RANDOM0, RANDOM1, RANDOM2
        }
        
        private void Start()
        {
            GameEvents.Instance.OnButtonDialed += OnButtonDialed;
        }

        private void OnButtonDialed(Button buttonAction)
        {
            if (buttonAction == Button.B_DIAL)
            {
                InitiateCall();
            }
            else
            {
                _dialedNumber += buttonAction.ToString().Replace("B", "");
            }
            Debug.Log("Number typed: "+_dialedNumber);
        }

        private void InitiateCall()
        {
            CallType callType;
            if (_dialedNumber.Equals(stockNumber))
            {
                callType = CallType.STOCK;
            } else if (_dialedNumber.Equals(kioskOwnerNumber))
            {
                callType = CallType.KIOSK_OWNER;
            }
            else
            {
                int randomCallId = _dialedNumber.ToCharArray().Sum(x => x) % 3 - 1 ;
                callType = (CallType)Enum.Parse(typeof(CallType), "RANDOM" + randomCallId, true);
                Debug.Log(callType);
            }
            _dialedNumber = "";
            GameEvents.Instance.OnCall(callType);
        }
    }
}