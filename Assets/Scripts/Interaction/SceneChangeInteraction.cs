using System;
using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(Collider2D))]
    public class SceneChangeInteraction : MonoBehaviour
    {
        [SerializeField] private WorldState.Scene sceneToGoTo;
        [SerializeField] private WorldState.MouseCursor _mouseCursor;
        
        private void OnMouseEnter()
        {
            Debug.Log(_mouseCursor.ToString());
            GameEvents.Instance.OnMouseCursorChange(_mouseCursor);
        }

        private void OnMouseExit()
        {
            GameEvents.Instance.OnMouseCursorChange(WorldState.MouseCursor.DEFAULT);
        }
        
        private void OnMouseDown()
        {
            GameEvents.Instance.OnSceneChange(sceneToGoTo);
        }
    }
}