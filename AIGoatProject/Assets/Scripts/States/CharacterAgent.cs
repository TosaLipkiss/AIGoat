using UnityEngine;
using UnityEngine.AI;

public class CharacterAgent : MonoBehaviour
{
    public Animator goatAnimator;
    public bool stateIsWalking;
    public NavMeshAgent goatsAgent;

    public GameObject destination;
    public GameObject birdHouseDestination;

    public GameObject playFlute;
    public GameObject flute;

    public AudioSource goatAudio;
    public AudioSource goatOtherAudio;

    public AudioClip heyThereMate;
    public AudioClip whatYouUpToSound;
    public AudioClip canYouStopItPlease;
    public AudioClip gasp;

    public AudioClip walkSteps;
    public AudioClip fluteSound;

    public SoundSingleton soundSingleton;

    RaycastForward raycastForward;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        raycastForward = GetComponent<RaycastForward>();
        goatsAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        soundSingleton = FindObjectOfType<SoundSingleton>();
        goatAnimator = GetComponent<Animator>();
    }

    public bool CheckPlayerInfront()
    {
        return raycastForward.RaycastPlayer();
    }

    public void ResetAgent()
    {
        goatsAgent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        goatsAgent.enabled = true;

        ResetAnimationTriggers();
    }

    public void WalkAround()
    {
        goatsAgent.enabled = true;
        goatsAgent.speed = 1.5f;
        goatsAgent.SetDestination(destination.transform.position);
    }

    public void StopWalking()
    {
        goatsAgent.SetDestination(transform.position);
        goatsAgent.enabled = false;
    }

    public void ChangeDestinationBirdHouse()
    {
        goatsAgent.enabled = true;
        goatsAgent.speed = 1.5f;
        destination = birdHouseDestination;
        goatsAgent.SetDestination(destination.transform.position);//hjälp
    }

    public void PlayFlute()
    {
        playFlute.SetActive(true);
        flute.SetActive(false);
    }

    public void DisturbedByPlayer()
    {
        StopWalking();
        PlayerInfrontWhileFluteSound();
        playFlute.SetActive(false);
        flute.SetActive(true);
    }

    public void StopPlayFlute()
    {
        playFlute.SetActive(false);
        flute.SetActive(true);
    }



    #region Sound
    /// <summary>
    /// /SOUND
    /// </summary>
    public void PlayWalkSound()
    {
        soundSingleton.OtherSound(walkSteps);
    }

    public void PlayFluteSound()
    {
        soundSingleton.OtherSound(fluteSound);
    }

    public void IdleGaspSound()
    {

        soundSingleton.GoatSound(gasp);

    }

    public void InfrontPlayerSound()
    {
        int randomInfrontPlayerSound = Random.Range(0, 2);

        if (randomInfrontPlayerSound == 0)
        {
            soundSingleton.GoatSound(whatYouUpToSound);
        }
        else if (randomInfrontPlayerSound == 1)
        {
            soundSingleton.GoatSound(whatYouUpToSound);
        }
    }

    public void PlayerInfrontWhileFluteSound()
    {
        soundSingleton.GoatSound(canYouStopItPlease);
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
    /// <summary>
    /// /Animations
    /// </summary>
    /// 
    public void ResetAnimationTriggers()
    {
        goatAnimator.ResetTrigger("Walk");
        goatAnimator.ResetTrigger("Idle");
        goatAnimator.ResetTrigger("PlayFlute");
        goatAnimator.ResetTrigger("PlayerInfrontIdle");
        goatAnimator.ResetTrigger("GreetPlayer");
    }

    public void WalkAnimation()
    {
        goatAnimator.SetTrigger("Walk");
    }

    public void IdleAnimation()
    {
        goatAnimator.SetTrigger("Idle");
    }

    public void IdleGaspAnimation()
    {
        goatAnimator.SetTrigger("IdleGasp");
    }

    public void IdleLookAroundpAnimation()
    {
        goatAnimator.SetTrigger("IdleLookAround");
    }

    public void IdleHmmAnimation()
    {
        goatAnimator.SetTrigger("IdleHmm");
    }

    public void PlayFluteAnimation()
    {
        goatAnimator.SetTrigger("PlayFlute");
    }

    public void PlayerInfrontAnimation()
    {
        goatAnimator.SetTrigger("PlayerInfrontIdle");
    }

    public void WhatsYouUpToAnimation()
    {
        goatAnimator.SetTrigger("WhatsUp");
    }

    public void GreetPlayerAnimation()
    {
        goatAnimator.SetTrigger("GreetPlayer");
    }

    public void DisturbedAnimation()
    {
        goatAnimator.SetTrigger("Disturbed");
    }

    #endregion
}
