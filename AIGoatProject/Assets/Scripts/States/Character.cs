using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject playFlute;
    public GameObject flute;

    public AudioSource goatAudio;
    public AudioSource goatOtherAudio;

    public AudioClip greetingsSound;
    public AudioClip whatYouUpToSound;
    public AudioClip gasp;

    public AudioClip walkSteps;
    public AudioClip fluteSound;

    public SoundSingleton soundSingleton;

    private void Start()
    {
        soundSingleton = FindObjectOfType<SoundSingleton>();
    }

}
