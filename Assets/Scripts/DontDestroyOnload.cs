using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DontDestroyOnload : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}