using System;
using model;
using UnityEngine;

namespace DefaultNamespace
{
    public class RoomPositionController : MonoBehaviour
    {
        private void OnMouseDown()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameEvents.Instance.OnMovePlayerToPosition(new Position(mousePos.x, mousePos.y));
        }
    }
}