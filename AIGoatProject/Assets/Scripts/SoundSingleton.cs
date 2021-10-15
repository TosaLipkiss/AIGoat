using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSingleton : MonoBehaviour
{
    public AudioSource goatSpeaking;
    public AudioSource other;

    public void Goat(AudioClip clip)
    {
        goatSpeaking.clip = clip;
        goatSpeaking.Play();
    }

    public void OtherSound(AudioClip clip)
    {
        goatSpeaking.clip = clip;
        goatSpeaking.Play();
    }
}
