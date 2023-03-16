using System;
using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(Collider2D))]
    public class SceneChangeInteraction : MonoBehaviour
    {
        [SerializeField] private WorldState.Scene sceneToGoTo;
        
        private void OnMouseDown()
        {
            GameEvents.Instance.OnSceneChange(sceneToGoTo);
        }
    }
}