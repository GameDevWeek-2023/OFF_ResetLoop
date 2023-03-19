using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    
    // Start is called before the first frame update
    void Start()
    {
        // audioSource.PlayOneShot(clip, 1f);
        Invoke(nameof(SceneChange), 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SceneChange()
    {
        SceneManager.LoadScene("BedRoom");
    }
}
