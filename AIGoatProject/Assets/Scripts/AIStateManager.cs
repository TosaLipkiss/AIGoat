using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIStateManager : MonoBehaviour
{
    public GameObject playFlute;
    public GameObject flute;
    public GameObject rayObject;
    public GameObject birdHouse;

    Animator goatAnimator;

    public NavMeshAgent goatNavmesh;
    public Rigidbody rb;

    int randomNumberState;
    float stateIntervall = 6f;

    bool changingState = false;
    bool playerInfront = false;
    bool isInteractingWithPlayer = false;
    bool alreadyGreeting = false;

    [SerializeField] AudioClip greetingsSound;
    [SerializeField] AudioClip whatYouUpToSound;
    [SerializeField] AudioClip walkingSound;
    [SerializeField] AudioClip fluteSound;
    public SoundSingleton soundSingleton;

    [SerializeField] CharacterAgent characterAgent;
    public DestinationSwitch destinationSwitch;

    state currentState;

    enum state
    {
        WalkRandom,
        Idle,
        PlayFlute,
        PlayerIsInfront
    }
    void Start()
    {
        goatAnimator = GetComponent<Animator>();
        Idle();

        BirdHouse.feed += FeedBirdHouse;

        currentState = state.Idle;
    }

    private void OnEnable()
    {
        StartCoroutine("SwitchState");
    }

    void Update()
    {
        if (changingState == true)
        {
            ResetStates();

            if (currentState == state.Idle)
            {
                Idle();
            }
            else if (currentState == state.PlayFlute)
            {
                PlayFlute();
            }
            else if (currentState == state.WalkRandom)
            {
                FindObjectOfType<SoundSingleton>().OtherSound(walkingSound);
                characterAgent.stateIsWalking = true;
            }

            else if (currentState == state.PlayerIsInfront)
            {
                PlayerInfront();
            }
        }

        changingState = false;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;

        //Physics.Raycast(rayObject.transform.position, forward, out hit, 10);

        if (Physics.Raycast(rayObject.transform.position, forward, out hit, 10))
        {
            if (hit.transform.CompareTag("Player"))
            {
                changingState = true;
                playerInfront = true;

                currentState = state.PlayerIsInfront;
            }
            else
            {
                playerInfront = false;
                isInteractingWithPlayer = false;

              //  currentState = state.WalkRandom;
            }
        }

        Debug.DrawRay(rayObject.transform.position, forward, Color.blue);
    }


    //Switching state in a couratine
    IEnumerator SwitchState()
    {
        while (true)
        {
            if(playerInfront == false)
            {
                yield return new WaitForSeconds(stateIntervall);

                changingState = true;

                randomNumberState = Random.Range(0, 5);

                if (randomNumberState <= 2)
                {
                    currentState = state.WalkRandom;
                }
                else if (randomNumberState == 3)
                {
                    currentState = state.PlayFlute;
                }
                else if (randomNumberState == 4)
                {
                    currentState = state.Idle;
                }

                Debug.Log(randomNumberState);
                Debug.Log("state is: " + currentState);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }


    //Reset all states to original before next state
    void ResetStates()
    {
        goatNavmesh.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        goatNavmesh.enabled = true;

        playFlute.SetActive(false);
        flute.SetActive(true);
        characterAgent.stateIsWalking = false;
        isInteractingWithPlayer = false;

        soundSingleton.otherGoatSound.Stop();


        //Ändra trigger i nya statemachine använd i "enter"
        goatAnimator.SetBool("PlayerIdle", false);
        goatAnimator.SetBool("Idle", false);
        goatAnimator.SetBool("Walk", false);
        goatAnimator.SetBool("PlayFlute", false);
    }


    //STATE METHODS

    void Idle()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        goatAnimator.SetBool("Idle", true);
    }

    void PlayFlute()
    {
        FindObjectOfType<SoundSingleton>().OtherSound(fluteSound);
        goatAnimator.SetBool("PlayFlute", true);
        playFlute.SetActive(true);
        flute.SetActive(false);
        GetComponent<NavMeshAgent>().enabled = false;
    }

    void FeedBirdHouse()
    {
/*        characterAgent.GetComponent<NavMeshAgent>().enabled = true;
        characterAgent.goatsAgent = GetComponent<NavMeshAgent>();
        characterAgent.goatsAgent.speed = 1.5f;
        characterAgent.goatAnimator.SetBool("Walk", true);*/
      //  characterAgent.destination = birdHouse;
    }

    void PlayerInfront()
    {
        if (playerInfront == true && isInteractingWithPlayer == false)
        {
            if(alreadyGreeting == false)
            {
                FindObjectOfType<SoundSingleton>().GoatSound(greetingsSound);
                goatAnimator.SetTrigger("GreetPlayer");
                alreadyGreeting = true;
            }

            GetComponent<NavMeshAgent>().enabled = false;
            goatAnimator.SetBool("PlayerIdle", true);

            isInteractingWithPlayer = true;
        }

     /*   else if(playerInfront == true && isInteractingWithPlayer == true)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            goatAnimator.SetBool("PlayerIdle", true);
        }*/
    }
}
