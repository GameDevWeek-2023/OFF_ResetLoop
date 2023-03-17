using System;
using System.Linq;
using UnityEngine;

namespace Interaction
{
    public class TelephoneController : MonoBehaviour
    {
        [SerializeField] private TextAsset dialogKioskOwner;
        [SerializeField] private Sprite spriteKioskOwner;

        private string _dialedNumber = "";
        private string stockNumber = "4711";
        private string kioskOwnerNumber = "0815";
        
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
            B_CALL
        };

        public enum CallType
        {
            STOCK, KIOSK_OWNER, RANDOM0, RANDOM1, RANDOM2
        }
        
        private void Start()
        {
            GameEvents.Instance.OnButtonDialed += OnButtonDialed;
            GameEvents.Instance.OnCall += delegate(CallType type) {
                if (type == CallType.KIOSK_OWNER)
                {
                    // So sorry, it's a mess
                    OnKioskOwnerCall();
                }
            };
        }

        private void OnButtonDialed(Button buttonAction)
        {
            if (buttonAction == Button.B_CALL)
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
                int randomCallId = _dialedNumber.ToCharArray().Sum(x => x) % 3;
                callType = (CallType)Enum.Parse(typeof(CallType), "RANDOM" + randomCallId, true);    
                Debug.Log(callType);
                // Ideen: Gewitter Oma, R2D2, Wall-E, Warteschleife
            }
            _dialedNumber = "";
            GameEvents.Instance.OnCall?.Invoke(callType);
        }

        // So sorry, it's a mess
        private void OnKioskOwnerCall()
        {
            GameEvents.Instance.OnKeyEvent(WorldState.KeyEvent.KIOSK_OWNER_GONE);
            GameEvents.Instance.OnDialogueStart?.Invoke(dialogKioskOwner.text, spriteKioskOwner);
        }
    }
}