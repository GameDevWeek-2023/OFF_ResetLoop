using System;
using UnityEngine;

namespace Controller
{
    public class FootStepInterface : MonoBehaviour
    {
        public void OnFootStep()
        {
            GameEvents.Instance.OnFootStep?.Invoke();
        }
    }
}