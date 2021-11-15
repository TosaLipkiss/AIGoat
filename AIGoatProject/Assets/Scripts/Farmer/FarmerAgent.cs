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
    public GameObject goatDestination;

    public int damping = 2;

    float timer;
    public bool timerFulfilled = false;

    public Vector3 targetAngles;

    public GameObject forkInHand;
    public GameObject forkIdle;

    public GameObject mopInHand;
    public GameObject mopIdle;

    public AudioSource goatAudio;
    public AudioSource goatOtherAudio;
    public AudioSource goatOneShotAudio;

    public AudioClip howdyHowdy;
    public AudioClip doYourWantSomething;
    public AudioClip canYouStopItPlease;
    public AudioClip gasp;
    public AudioClip betterBeImportant;
    public AudioClip watchYourBack;
    public AudioClip fineWeatherWeHave;

    public AudioClip walkSteps;
    public AudioClip bag;
    public AudioClip mop;

    public float voiceTimer;
    public bool voiceOnCooldown;

    public SoundSingleton soundSingleton;
    public DestinationSwitch destinationSwitch;

    public Transform neighbourTarget;

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

    public void DismissMop()
    {
        mopInHand.SetActive(false);
        mopIdle.SetActive(true);

        forkInHand.SetActive(true);
        forkIdle.SetActive(false);
    }

    public void UseMop()
    {
        mopInHand.SetActive(true);
        mopIdle.SetActive(false);

        forkInHand.SetActive(false);
        forkIdle.SetActive(true);
    }

    #region ChangeDestination

    public void ChangeDestinationGoat()
    {
        farmerAgent.enabled = true;
        farmerAgent.speed = 1.5f;
        destination = goatDestination;
        farmerAgent.SetDestination(destination.transform.position);
    }

    public void ChangeDestination()
    {
        destinationSwitch.SwitchDestination();
    }

    public void RotateTowardsNeighbour()
    {
        farmerAgent.enabled = false;

        Vector3 lookPosition = neighbourTarget.position - transform.position;
        lookPosition.y = 0;

        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    #endregion

    #region Sound
    public void PlayWalkSound()
    {
        soundSingleton.OtherFarmerSound(walkSteps);
    }

    public void IdleGaspSound()
    {
        soundSingleton.FarmerSound(gasp);
    }

    public void DoYourWantSomething()
    {
        voiceOnCooldown = true;
        soundSingleton.FarmerSound(doYourWantSomething);
    }

    public void BetterBeImportant()
    {
        voiceOnCooldown = true;
        soundSingleton.FarmerSound(betterBeImportant);
    }

    public void WatchYourBack()
    {
        voiceOnCooldown = true;
        soundSingleton.FarmerSound(watchYourBack);
    }

    public void DisturbedSound()
    {
        voiceOnCooldown = true;
        soundSingleton.FarmerSound(canYouStopItPlease);
    }

    public void DisturbedReallySound()
    {
        voiceOnCooldown = true;
        soundSingleton.FarmerSound(watchYourBack);
    }

    public void GreetPlayerSound()
    {
        soundSingleton.FarmerSound(howdyHowdy);
    }

    public void StopOtherFarmerSound()
    {
        soundSingleton.TurnOffOtherFarmerSound();
    }

    public void StopFarmerSound()
    {
        soundSingleton.TurnOffFarmerSound();
    }

    public void TalkToNeighborSound()
    {
        voiceOnCooldown = true;
        soundSingleton.FarmerSound(fineWeatherWeHave);
    }

    public void MopSound()
    {
        voiceOnCooldown = true;
        soundSingleton.OtherFarmerSound(mop);
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

    public void TalkToNeighborAnimation()
    {
        farmerAnimator.SetTrigger("TalkToNeighbor");
    }

    public void MopAnimation()
    {
        farmerAnimator.SetTrigger("Mop");
    }
    #endregion
}
