using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interaction;
using model;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Music[] musicTracks;

    public AudioMixerGroup soundeffectOutput;
    public AudioMixerGroup musicOutput;
    public AudioMixerGroup ambienceOutput;

    private MusicManager _musicManager;
    private List<string> _onlyInThisSceneSounds = new List<string>();

    private Dictionary<string, string[]> _randomSoundCache = new Dictionary<string, string[]>();

    public enum SoundGroup
    {
        Soundeffect,
        Music,
        Ambience
    }

    public static AudioManager instance;

    private bool mute = false;
    private bool firstCall = true;
    private bool firstEnterBedRoomThisCycle = true;
    private bool resetSoundActive = false;
    private float resetSoundTimePosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _musicManager = GetComponent<MusicManager>();
        Assert.IsNotNull(soundeffectOutput);
        Assert.IsNotNull(musicOutput);
        Assert.IsNotNull(_musicManager);
        DontDestroyOnLoad(gameObject);
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.outputAudioMixerGroup = getAudioMixer(sound.soundGroup);
        }

        foreach (Music music in musicTracks)
        {
            music.source = gameObject.AddComponent<AudioSource>();
            music.source.clip = music.clip;
            music.source.loop = false;
            music.source.volume = music.volume;
            music.source.outputAudioMixerGroup = getAudioMixer(SoundGroup.Music);

            _musicManager.AddMusicTrack(music.source, music.MusicScene);
        }
    }

    private void Start()
    {
        GameEvents.Instance.OnSceneChange += OnSceneChange;
        GameEvents.Instance.OnCall += OnCall;
        GameEvents.Instance.OnButtonDialed += OnButtonDialed;
        GameEvents.Instance.OnFootStep += OnFootstep;
        GameEvents.Instance.OnWorldReset += OnWorldReset;
        GameEvents.Instance.OnDialogueStart += delegate { OnDialogOpened(); };
        GameEvents.Instance.OnDialogueClosed += OnDialogClosed;
        GameEvents.Instance.OnTimeChanged += delegate(int time)
        {
            if (time == 50)
            {
                PlayCycleEndSound();
            }
        };
        OnSceneChange(new SceneChange(WorldState.Instance.CurrentScene, WorldState.Instance.CurrentScene));
    }

    private void OnWorldReset()
    {
        firstEnterBedRoomThisCycle = true;
        _musicManager.Stop();
    }

    private void PlayCycleEndSound()
    {
        resetSoundActive = true;
        Play("resetsound", 0f, false);
    }

    public void OnSceneChange(SceneChange sceneChange)
    {
        WorldState.Scene scene = sceneChange.NewScene;
        foreach (string onlyInThisSceneSound in _onlyInThisSceneSounds)
        {
            StopSound(onlyInThisSceneSound);
        }

        _onlyInThisSceneSounds.Clear();
        if (scene != WorldState.Scene.Telephone && !firstEnterBedRoomThisCycle)
        {
            Play("door");
        }

        switch (scene)
        {
            case WorldState.Scene.Bedroom:
                Play("clock");
                if (firstEnterBedRoomThisCycle)
                {
                    Play("gong");
                    firstEnterBedRoomThisCycle = false;
                    _musicManager.Play();
                }

                break;
            case WorldState.Scene.Street:
                Play("cityambience", WorldState.Instance.Time);
                break;
            case WorldState.Scene.Telephone:
                break;
            case WorldState.Scene.End:
                resetSoundActive = false; // its still playing but variable isn't needed at that point
                resetSoundTimePosition = 0f;
                break;
            case WorldState.Scene.Reset:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
        }
    }

    public void OnCall(TelephoneController.CallType callType)
    {
        switch (callType)
        {
            case TelephoneController.CallType.STOCK:
                break;
            case TelephoneController.CallType.KIOSK_OWNER:
                break;
            case TelephoneController.CallType.RANDOM0:
                break;
            case TelephoneController.CallType.RANDOM1:
                break;
            case TelephoneController.CallType.RANDOM2:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(callType), callType, null);
        }
    }

    public void OnButtonDialed(TelephoneController.Button dialButton)
    {
        string sound = dialButton.ToString().ToLower();
        if (dialButton != TelephoneController.Button.B_CALL)
        {
            sound = sound.Insert(1, "_");
        }

        Play(sound);
    }

    public AudioMixerGroup getAudioMixer(SoundGroup soundGroup)
    {
        switch (soundGroup)
        {
            case SoundGroup.Soundeffect:
                return soundeffectOutput;
            case SoundGroup.Music:
                return musicOutput;
            case SoundGroup.Ambience:
                return musicOutput;
            // case SoundGroup.MusicInterlude:
            //     return musicInterludeOutput;
            default:
                throw new ArgumentOutOfRangeException(nameof(soundGroup), soundGroup,
                    "Sound group not found + " + soundGroup);
        }
    }

    public void OnFootstep()
    {
        switch (WorldState.Instance.CurrentScene)
        {
            case WorldState.Scene.Bedroom:
                PlayRandomSoundStartingWith("foot_bed");
                break;
            case WorldState.Scene.Street:
            case WorldState.Scene.Telephone:
                PlayRandomSoundStartingWith("foot_str");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Play(string name, float startTime = 0f, bool stopWhenLeavingScene = true)
    {
        if (!mute)
        {
            Sound soundToPlay = Array.Find(sounds, sound => sound.name == name);
            if (soundToPlay == null)
            {
                Debug.LogWarning("Sound " + name + " not found");
            }
            else
            {
                soundToPlay.source.time = startTime;
                soundToPlay.source.Play();
                if (stopWhenLeavingScene)
                {
                    _onlyInThisSceneSounds.Add(name);
                }
            }
        }
    }

    public void PauseSound(string name)
    {
        Sound soundToPlay = Array.Find(sounds, sound => sound.name == name);
        if (soundToPlay == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
        }
        else
        {
            if (soundToPlay.source.isPlaying)
            {
                soundToPlay.source.Pause();
                resetSoundTimePosition = soundToPlay.source.time;
            }
        }
    }

    public void StopSound(string name)
    {
        Sound soundToPlay = Array.Find(sounds, sound => sound.name == name);
        if (soundToPlay == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
        }
        else
        {
            soundToPlay.source.Stop();
        }
    }

    public void toggleMute()
    {
        mute = !mute;
    }

    private void PlayRandomSoundStartingWith(string startString)
    {
        if (!_randomSoundCache.ContainsKey(startString))
        {
            _randomSoundCache[startString] = sounds.Where(sound => sound.name.StartsWith(startString))
                .Select(sound => sound.name).ToArray();
        }

        playRandomSound(_randomSoundCache[startString]);
    }

    private void playRandomSound(string[] selection)
    {
        int selectedId = Random.Range(0, selection.Length);
        string sound = selection[selectedId];
        Play(sound);
    }

    private void OnDialogOpened()
    {
        if (resetSoundActive)
        {
            PauseSound("resetsound");
        }
    }

    private void OnDialogClosed()
    {
        if (resetSoundActive)
        {
            Play("resetsound", resetSoundTimePosition, false);
        }
    }
}