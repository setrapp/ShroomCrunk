using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public AudioClip walkClick;
    private AudioSource characterAudio;
    public AudioClip jumpClip;

    void Start()
    {
        characterAudio = GameObject.Find("CharacterAudio").GetComponent<AudioSource>();
    }

    void WalkAudio()
    {
        //characterAudio.pitch = Random.Range(0.9f, 1.2f);
        //characterAudio.PlayOneShot(walkClick, 0.06f);
    }
    public void JumpAudio()
    {
        characterAudio.pitch = Random.Range(2.8f, 3.2f);
        characterAudio.PlayOneShot(jumpClip, 0.06f);
    }

}
