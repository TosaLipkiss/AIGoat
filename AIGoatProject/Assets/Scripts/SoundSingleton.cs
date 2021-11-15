using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSingleton : MonoBehaviour
{
    public AudioSource goatSound;
    public AudioSource farmerSound;
    public AudioSource otherGoatSound;
    public AudioSource otherFarmerSound;

    public void GoatSound(AudioClip clip)
    {
        goatSound.clip = clip;
        goatSound.Play();
    }

    public void FarmerSound(AudioClip clip)
    {
        farmerSound.clip = clip;
        farmerSound.Play();
    }

    public void TurnOffGoatSound()
    {
        goatSound.Stop();
    }

    public void TurnOffFarmerSound()
    {
        farmerSound.Stop();
    }

    public void OtherFarmerSound(AudioClip clip)
    {
        otherFarmerSound.clip = clip;
        otherFarmerSound.Play();
    }

    public void OtherSound(AudioClip clip)
    {
        otherGoatSound.clip = clip;
        otherGoatSound.Play();
    }

    public void OneShotSound(AudioClip clip)
    {
        goatSound.clip = clip;
        goatSound.Play();
    }

    public void TurnOffOtherGoatSound()
    {
        otherGoatSound.Stop();
    }

    public void TurnOffOtherFarmerSound()
    {
        otherFarmerSound.Stop();
    }
}
