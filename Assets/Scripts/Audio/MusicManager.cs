using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;


public class MusicManager : MonoBehaviour
{
    private List<AudioSource> musicTracksBedroomScene = new List<AudioSource>();
    

    private Dictionary<MusicScene, List<AudioSource>> soundtrackPicker = new Dictionary<MusicScene, List<AudioSource>>();
    
    private bool _musicIsPlaying = false;
    private int currentlyPlayingTrackId = 0;
    
    
    [SerializeField] private float pauseBetweenSongs = 2f;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private AudioMixerSnapshot fullMusicVolume;
    [SerializeField] private AudioMixerSnapshot maxMusicVolume;
    [SerializeField] private AudioMixerSnapshot musicOff;

    public MusicScene currentMusicScene = MusicScene.Bedroom;
    private AudioSource _currentlyPlayingTrack;
    private Coroutine _waitForSongFinishCoroutine;

    private void Awake()
    {
        soundtrackPicker.Add(MusicScene.Bedroom, musicTracksBedroomScene);
    }

    public enum MusicScene {
        Bedroom, Street, Telephone
    }

    public void OnMusicSceneChange(MusicScene musicScene)
    {
        if (musicScene != currentMusicScene)
        {
            currentMusicScene = musicScene;
            Stop();
            // if (musicScene == MusicScene.SteamRock)
            // {
            //     maxMusicVolume.TransitionTo(0.01f);
            // }
            currentlyPlayingTrackId = 0;
            Play();
        }
    }
    
    public void AddMusicTrack(AudioSource track, MusicScene musicScene)
    {
        switch (musicScene)
        {
            case MusicScene.Bedroom:
                musicTracksBedroomScene.Add(track);
                break;
            // case MusicScene.Street:
            //     musicTracksEndgameScene.Add(track);
            //     break;            
            // case MusicScene.SteamRock:
            //     musicTracksSteamRockScene.Add(track);
            //     break;
            default:
                throw new ArgumentOutOfRangeException(nameof(musicScene), musicScene, null);
        }
        
    }

    public void Play()
    {
        _currentlyPlayingTrack = soundtrackPicker[currentMusicScene][currentlyPlayingTrackId];
        _currentlyPlayingTrack.Play();
        _waitForSongFinishCoroutine = StartCoroutine(waitForSoundToFinish());
    }

    public void Stop()
    {
        _currentlyPlayingTrack.Stop();
        StopCoroutine(_waitForSongFinishCoroutine);
    }
    
    public void SetNextSong()
    {
        currentlyPlayingTrackId = (currentlyPlayingTrackId+1)%soundtrackPicker[currentMusicScene].Count;
    }

    public void OnMusicTrackFinished()
    {
        SetNextSong();
        Play();
    }
    
    private IEnumerator waitForSoundToFinish()
    {
        float musicLength = _currentlyPlayingTrack.clip.length;
        yield return new WaitForSeconds(musicLength + pauseBetweenSongs);
        OnMusicTrackFinished();
    }

    public void fadeInMusic()
    {
        fullMusicVolume.TransitionTo(fadeTime);
    }
    
    public void fadeOutMusic()
    {
        musicOff.TransitionTo(fadeTime);
    }
    
}