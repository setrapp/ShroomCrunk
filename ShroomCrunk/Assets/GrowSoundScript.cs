using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GrowSoundScript : MonoBehaviour
{
    public AudioClip[] beeps;
    public AudioSource source;
    private float initialPitch;

    private bool isBeeping = false;

    public void startGrowingSounds(){ isBeeping = true; }
    public void stopGrowingSounds() { isBeeping = false; }

    private void Awake()
    {
        initialPitch = source.pitch;
        source.loop = false;
    }
    private void Update()
    {
        if(isBeeping && !source.isPlaying)
        {
            source.clip = getRandomBeep();
            source.pitch = initialPitch + Random.Range(-.01f, .01f);
            source.Play();
        }        
    }

    private AudioClip getRandomBeep()
    {
        int index = Random.Range(0, beeps.Length);
        return beeps[index];
    }
}
