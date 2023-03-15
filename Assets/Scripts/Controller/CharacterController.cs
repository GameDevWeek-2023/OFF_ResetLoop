using System;
using model;
using UnityEngine;

namespace Controller
{
    public class CharacterController : MonoBehaviour
    {
        private void Start()
        {
            GameEvents.Instance.OnMovePlayerToPosition += MoveCharacterToPosition;
        }

        private void MoveCharacterToPosition(Position position)
        {
            LeanTween.move(gameObject, new Vector2(position.X, position.Y), 1f);
        }
    }
}