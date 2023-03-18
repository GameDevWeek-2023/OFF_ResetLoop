using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ResetScene
{
    public class RotateAroundCenter : MonoBehaviour {
        public Transform target; // The object to rotate around
        public float speed = 300f; // The speed of rotation
        [SerializeField] private Vector3 rotation;
        private float _speedModifier = 0f;
        
        private void Start()
        {
            _speedModifier = Random.Range(-50f, 50f);
        }

        void Update() {
            transform.RotateAround(target.position, rotation, (speed + _speedModifier) * Time.deltaTime);
        }
    }
}
