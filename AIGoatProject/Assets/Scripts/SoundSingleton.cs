using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSingleton : MonoBehaviour
{
    public AudioSource goatSound;
    public AudioSource otherGoatSound;

    public void GoatSound(AudioClip clip)
    {
        goatSound.clip = clip;
        goatSound.Play();
    }

    public void TurnOfGoatSound()
    {
        goatSound.Stop();
    }

    public void OtherSound(AudioClip clip)
    {
        otherGoatSound.clip = clip;
        otherGoatSound.Play();
    }

    public void TurnOfOtherGoatSound()
    {
        otherGoatSound.Stop();
    }
}
