using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAudio : MonoBehaviour
{
    private AudioSource mainAudio;
    private bool audioSpeeding;
    private AudioSource poundAudio;
    public AudioClip poundClip;


    void Start()
    {
        mainAudio = GetComponent<AudioSource>();
        poundAudio = transform.Find("Pound").GetComponent<AudioSource>();
    }

    public void AudioSpeed()
    {
        if (!audioSpeeding)
            StartCoroutine(AudioSpeedUp());
    }
    IEnumerator AudioSpeedUp()
    {
        audioSpeeding = true;
        poundAudio.pitch = Random.Range(1.9f, 2.2f);
        poundAudio.PlayOneShot(poundClip);
        while (mainAudio.pitch < 2.5f)
        {
            mainAudio.pitch += 0.6f;
            yield return new WaitForSeconds(0.05f);
        }
        while (mainAudio.pitch > 1)
        {
            mainAudio.pitch -= 0.6f;
            yield return null;
        }
        audioSpeeding = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
