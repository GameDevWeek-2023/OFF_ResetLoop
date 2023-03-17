using System;
using model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class RoomPositionController : MonoBehaviour
    {
        private void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameEvents.Instance.OnMovePlayerToPosition(new Position(mousePos.x, mousePos.y));
            }
        }

        private void OnMouseEnter()
        {
            GameEvents.Instance.OnMouseCursorChange(WorldState.MouseCursor.FOOT);
        }
        
        private void OnMouseExit()
        {
            GameEvents.Instance.OnMouseCursorChange(WorldState.MouseCursor.DEFAULT);
        }
    }
}