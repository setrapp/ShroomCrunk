using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAudio : MonoBehaviour
{
    private AudioSource mainAudio;
    private bool audioSpeeding;
    private AudioSource poundAudio;
    public AudioClip poundClip;
    int changeSpeed;

    public AudioSource otherBackAudio;

    void Start()
    {
        mainAudio = GetComponent<AudioSource>();
        poundAudio = transform.Find("Pound").GetComponent<AudioSource>();
        otherBackAudio = transform.Find("OtherBackAudio").GetComponent<AudioSource>();
    }

    public void AudioSpeed()
    {
        if (!audioSpeeding)
        {
            if (mainAudio.volume > 0.5f)
            {
                 StartCoroutine(AudioSpeedUp(mainAudio, 1f));
            }
            else
            {
                StartCoroutine(AudioSpeedUp(otherBackAudio, 1.4f));
            }
        }
    }
    IEnumerator AudioSpeedUp(AudioSource audioJump, float mainPitchSpeed)
    {
        audioSpeeding = true;
        changeSpeed++;
        if (changeSpeed > 10)
        {
            if (otherBackAudio.volume < 1)
            {
                StartCoroutine(AudioVolUp(otherBackAudio));
                StartCoroutine(AudioVolDown(mainAudio));

            }
            else
            {
                StartCoroutine(AudioVolDown(otherBackAudio));
                StartCoroutine(AudioVolUp(mainAudio));
            }
            changeSpeed = 0;
        }
        poundAudio.pitch = Random.Range(1.9f, 2.2f);
        poundAudio.PlayOneShot(poundClip);

        while (audioJump.pitch < 2.5f)
        {
            audioJump.pitch += 0.6f;
            yield return new WaitForSeconds(0.05f);
        }
        while (audioJump.pitch > mainPitchSpeed)
        {
            audioJump.pitch -= 0.6f;
            yield return new WaitForSeconds(0.05f);
        }
        audioSpeeding = false;
    }
    IEnumerator AudioVolUp(AudioSource audioTurnUp)
    {
        audioTurnUp.Play();
        while (audioTurnUp.volume < 1)
        {
            audioTurnUp.volume += 0.005f;
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator AudioVolDown(AudioSource audioTurnDown)
    {
        while (audioTurnDown.volume > 0)
        {
            audioTurnDown.volume -= 0.005f;
            yield return new WaitForSeconds(0.05f);
        }
        audioTurnDown.Pause();
    }

}
