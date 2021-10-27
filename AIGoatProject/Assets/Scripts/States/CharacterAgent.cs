using UnityEngine;
using UnityEngine.AI;

public class CharacterAgent : MonoBehaviour
{
    public GameObject character;

    public Animator goatAnimator;
    public bool stateIsWalking;
    public NavMeshAgent goatsAgent;

    public FindMushrooms findMushrooms;
    public AIInventory aiInventory;
    public Home home;

    public GameObject randomDestination;
    public GameObject destination;
    public GameObject birdHouseDestination;
    public GameObject mushroomDestination;
    public GameObject homeDestination;

    public Transform birdHouseTarget;
    public int damping = 2;

    public int inventory;

    float timer;
    bool timerFulfilled = false;

    public Vector3 targetAngles;

    public GameObject playFlute;
    public GameObject flute;

    public AudioSource goatAudio;
    public AudioSource goatOtherAudio;
    public AudioSource goatOneShotAudio;

    public AudioClip heyThereMate;
    public AudioClip whatYouUpToSound;
    public AudioClip canYouStopItPlease;
    public AudioClip gasp;
    public AudioClip perfect;

    public AudioClip walkSteps;
    public AudioClip fluteSound;
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
        findMushrooms = GetComponent<FindMushrooms>();
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

    #region Resets

    public void ResetAgent()
    {
        goatsAgent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        goatsAgent.enabled = true;

        timerFulfilled = false;
        timer = 0f;
        ResetAnimationTriggers();
    }

    public void ResetDestination()
    {
        goatsAgent.enabled = true;
        goatsAgent.speed = 1.5f;
        destination = randomDestination;
        goatsAgent.SetDestination(destination.transform.position);
    }

    #endregion Resets

    #region Walking

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

    #endregion Walking

    #region ChangeDestination

    public void ChangeDestinationBirdHouse()
    {
        goatsAgent.enabled = true;
        goatsAgent.speed = 1.5f;
        destination = birdHouseDestination;
        goatsAgent.SetDestination(destination.transform.position);
    }


    public void ChangeDestinationMushroom()
    {
        if(findMushrooms.closestMushroom == null)
        {
            mushroomDestination = null;
        }
        else
        {
            mushroomDestination = findMushrooms.closestMushroom;
            destination = mushroomDestination;

            goatsAgent.SetDestination(destination.transform.position);

            goatsAgent.enabled = true;
            goatsAgent.speed = 1.5f;
        }
    }

    public void ChangeDestinationHome()
    {
        if (findMushrooms.closestMushroom == null)
        {
            mushroomDestination = null;
        }
        else
        {
            destination = homeDestination;

            goatsAgent.SetDestination(destination.transform.position);

            goatsAgent.enabled = true;
            goatsAgent.speed = 1.5f;
        }
    }

    #endregion ChangeDestination

    #region FeedBird
    public void FeedingBirds()
    {
        goatsAgent.enabled = false;

        Vector3 lookPosition = birdHouseTarget.position - transform.position;
        lookPosition.y = 0;

        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
    #endregion FeedBird

    #region Flute
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

    #endregion Flute

    #region Inventory
    public void AddMushroomInInventory()
    {
        timer += Time.deltaTime;

        if(timer > 1.4f && !timerFulfilled)
        {
            findMushrooms.DestroyClosestMushroom();
            aiInventory.newMushroomAdded = true;
            timerFulfilled = true;
        }
    }

    public void CheckInventoryStorage()
    {
        inventory = aiInventory.collectedMushrooms;

        if(inventory == 1)
        {
            home.inventoryFull = true;
        }
    }

    public void EmptyInventory()
    {
        aiInventory.collectedMushrooms = 0;
        inventory = 0;
        home.inventoryFull = false;
    }

    #endregion Inventory

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

    public void BagSound()
    {
        soundSingleton.OneShotSound(bag);
    }

    public void PerfectSound()
    {
        voiceOnCooldown = true;
        soundSingleton.GoatSound(perfect);
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

    public void FeedBirdAnimation()
    {
        goatAnimator.SetTrigger("FeedBird");
    }

    #endregion
}
