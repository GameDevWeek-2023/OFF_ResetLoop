using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GameEvents.Instance.OnRequestSceneChange(sceneToGoTo);
            }
        }
    }
}