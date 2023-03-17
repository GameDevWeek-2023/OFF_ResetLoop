using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MouseCursor", menuName = "MouseCursor", order = 0)]
    public class MouseCursorSO : ScriptableObject
    {
        [SerializeField] private WorldState.MouseCursor _mouseCursorState;
        [SerializeField] private Texture2D mouseCursorImage;

        public WorldState.MouseCursor MouseCursorState => _mouseCursorState;

        public Texture2D MouseCursorImage => mouseCursorImage;
    }
}