using System;
using model;
using UnityEngine;
using UnityEngine.AI;

namespace Controller
{
    public class CharacterController : MonoBehaviour
    {
        enum WalkingDir
        {
            LEFT,
            RIGHT
        };
        private NavMeshAgent _agent;
        private Animator _animator;
        private WalkingDir _walkingDir = WalkingDir.LEFT;
        private static readonly int WalkLeft = Animator.StringToHash("walk_left");
        private static readonly int WalkRight = Animator.StringToHash("walk_right");
        private static readonly int IdleLeft = Animator.StringToHash("idle_left");
        private static readonly int IdleRight = Animator.StringToHash("idle_right");
        private bool _isWalking = false;

        private void Start()
        {
            GameEvents.Instance.OnMovePlayerToPosition += MoveCharacterToPosition;
            _agent = GetComponent<NavMeshAgent>();
            _animator = transform.Find("Sprite").GetComponent<Animator>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        private void Update()
        {
            if (_isWalking && _agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
            {
                if (_walkingDir == WalkingDir.LEFT)
                {
                    _animator.SetTrigger(IdleLeft);
                }
                else
                {
                    _animator.SetTrigger(IdleRight);
                }
                _isWalking = false;
            }
        }

        private void MoveCharacterToPosition(Position position)
        { 
            if (position.X < transform.position.x)
            {
                _animator.SetTrigger(WalkLeft);
                _walkingDir = WalkingDir.LEFT;
            }
            else
            {
                _animator.SetTrigger(WalkRight);
                _walkingDir = WalkingDir.RIGHT;
            }

            _isWalking = true;
            Debug.Log("Move Character to " + position.X + " - " + position.Y);
            _agent.SetDestination(new Vector3(position.X, position.Y, 0f));
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnMovePlayerToPosition -= MoveCharacterToPosition;
        }
    }
}