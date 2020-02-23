using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomSoundScript : MonoBehaviour
{
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.loop = false;
    }

    public void bounceSound()
    {
        source.Play();
    }
}
