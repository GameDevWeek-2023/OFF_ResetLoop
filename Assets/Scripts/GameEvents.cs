using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameEvents : MonoBehaviour
    {
        public Action<int> OnTimeChanged;
        public static GameEvents Instance;
        public Action<ItemInteraction.Item> OnItemFound;
        public Action OnDialogueOpened;
        public Action OnDialogueClosed;
        public Action OnWorldReset;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}