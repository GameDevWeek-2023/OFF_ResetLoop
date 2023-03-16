﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interaction;
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
        OnSceneChange(WorldState.Instance.CurrentScene);
    }

    public void OnSceneChange(WorldState.Scene scene)
    {
        foreach (string onlyInThisSceneSound in _onlyInThisSceneSounds)
        {
            StopSound(onlyInThisSceneSound);
        }
        _onlyInThisSceneSounds.Clear();
        if (scene != WorldState.Scene.Telephone && !firstCall)
        {
            Play("door");
        }

        firstCall = false;

        switch (scene)
        {
            case WorldState.Scene.Bedroom:
                Play("clock");
                break;
            case WorldState.Scene.Street:
                Play("cityambience", WorldState.Instance.Time);
                break;
            case WorldState.Scene.Telephone:
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
    
    public void Play(string name, float startTime = 0f)
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
                _onlyInThisSceneSounds.Add(name);
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
                .Select(sound => sound.name).
                ToArray();
        }
        playRandomSound(_randomSoundCache[startString]);
    }

    private void playRandomSound(string[] selection)
    {
        int selectedId = Random.Range(0, selection.Length);
        string sound = selection[selectedId];
        Play(sound);
    }
}