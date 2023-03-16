

using UnityEngine;

[System.Serializable]
    public class Music
    {
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1;

        public MusicManager.MusicScene MusicScene;
    
        [HideInInspector]
        public AudioSource source;
    }
