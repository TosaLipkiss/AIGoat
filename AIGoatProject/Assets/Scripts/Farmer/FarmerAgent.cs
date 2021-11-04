using UnityEngine;
using UnityEngine.AI;

public class FarmerAgent : MonoBehaviour
{
    public GameObject character;

    public Animator farmerAnimator;
    public bool stateIsWalking;
    public NavMeshAgent farmerAgent;

    public GameObject randomDestination;
    public GameObject destination;

    public int damping = 2;

    float timer;
    public bool timerFulfilled = false;

    public Vector3 targetAngles;

    public AudioSource goatAudio;
    public AudioSource goatOtherAudio;
    public AudioSource goatOneShotAudio;

    public AudioClip heyThereMate;
    public AudioClip whatYouUpToSound;
    public AudioClip canYouStopItPlease;
    public AudioClip gasp;
    public AudioClip perfect;
    public AudioClip youAreTall;
    public AudioClip really;

    public AudioClip walkSteps;
    public AudioClip bag;

    public float voiceTimer;
    public bool voiceOnCooldown;

    public SoundSingleton soundSingleton;

    RaycastForward raycastForward;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        raycastForward = GetComponent<RaycastForward>();
        farmerAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        soundSingleton = FindObjectOfType<SoundSingleton>();
        farmerAnimator = GetComponent<Animator>();
    }

    public bool CheckPlayerInfront()
    {
        return raycastForward.RaycastPlayer();
    }


    #region Resets

    public void ResetAgent()
    {
        farmerAgent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        farmerAgent.enabled = true;

        timerFulfilled = false;
        timer = 0f;
    }

    public void ResetDestination()
    {
        farmerAgent.enabled = true;
        farmerAgent.speed = 1.5f;
        destination = randomDestination;
        farmerAgent.SetDestination(destination.transform.position);
    }

    #endregion Resets

    #region Walking

    public void WalkAround()
    {
        farmerAgent.enabled = true;
        farmerAgent.speed = 1.5f;
        farmerAgent.SetDestination(destination.transform.position);
    }

    public void StopWalking()
    {
        farmerAgent.SetDestination(transform.position);
        farmerAgent.enabled = false;
    }

    #endregion Walking

    #region Sound
    public void PlayWalkSound()
    {
        soundSingleton.OtherSound(walkSteps);
    }

    public void IdleGaspSound()
    {
        soundSingleton.GoatSound(gasp);
    }

    public void WhatYouUpToSound()
    {
        voiceOnCooldown = true;
        soundSingleton.GoatSound(whatYouUpToSound);
    }

    public void YouAreTallSound()
    {
        voiceOnCooldown = true;
        soundSingleton.GoatSound(youAreTall);
    }

    public void DisturbedSound()
    {
        voiceOnCooldown = true;
        soundSingleton.GoatSound(canYouStopItPlease);
    }

    public void DisturbedReallySound()
    {
        voiceOnCooldown = true;
        soundSingleton.GoatSound(really);
    }

    public void GreetPlayerSound()
    {
        soundSingleton.GoatSound(heyThereMate);
    }

    public void StopOtherGoatSound()
    {
        soundSingleton.TurnOfOtherGoatSound();
    }

    public void StopGoatSound()
    {
        soundSingleton.TurnOfGoatSound();
    }

    #endregion

    #region Animation
    public void WalkAnimation()
    {
        farmerAnimator.SetTrigger("Walk");
    }

    public void IdleAnimation()
    {
        farmerAnimator.SetTrigger("Idle");
    }

    public void IdleGaspAnimation()
    {
        farmerAnimator.SetTrigger("IdleGasp");
    }

    public void IdleLookAroundpAnimation()
    {
        farmerAnimator.SetTrigger("IdleLookAround");
    }

    public void IdleHmmAnimation()
    {
        farmerAnimator.SetTrigger("IdleHmm");
    }


    public void PlayerInfrontAnimation()
    {
        farmerAnimator.SetTrigger("PlayerInfrontIdle");
    }

    public void WhatsYouUpToAnimation()
    {
        farmerAnimator.SetTrigger("WhatsUp");
    }

    public void YouAreTallAnimation()
    {
        farmerAnimator.SetTrigger("YouAreTall");
    }

    public void GreetPlayerAnimation()
    {
        farmerAnimator.SetTrigger("GreetPlayer");
    }

    public void DisturbedAnimation()
    {
        farmerAnimator.SetTrigger("Disturbed");
    }

    public void DisturbedReallyAnimation()
    {
        farmerAnimator.SetTrigger("DisturbedReally");
    }
    #endregion
}
