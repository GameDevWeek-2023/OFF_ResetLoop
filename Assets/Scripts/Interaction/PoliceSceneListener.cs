using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSceneListener : MonoBehaviour
{
    [SerializeField] private GameObject policeLight;
    private float timer;
    private bool timerIsRunning;

    // Start is called before the first frame update
    private void Start()
    {
        GameEvents.Instance.OnKeyEvent += OnKeyEvent;
        policeLight.SetActive(false);
        timer = 0;
        timerIsRunning = false;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            timer += Time.deltaTime;

            if(timer >= 3f)
            {
                StopPoliceScene();
            }
        }
    }

    private void OnKeyEvent(WorldState.KeyEvent keyEvent)
    {
        switch (keyEvent)
        {
            case WorldState.KeyEvent.MURDER:
                StartPoliceScene();
                break;
            case WorldState.KeyEvent.SUICIDE:
                StartPoliceScene();
                break;
        }
    }

    private void StartPoliceScene()
    {
        Debug.Log("START POLICE SCENE");
        timer = 0;
        timerIsRunning = true;
        policeLight.SetActive(true);
    }

    private void StopPoliceScene()
    {
        Debug.Log("STOP POLICE SCENE");
        timer = 0;
        timerIsRunning = false;
        policeLight.SetActive(false);
        GameEvents.Instance.OnWorldReset?.Invoke();
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnKeyEvent -= OnKeyEvent;
    }

}
