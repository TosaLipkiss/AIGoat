using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIStateManager : MonoBehaviour
{
    public GameObject playFlute;
    public GameObject flute;
    public GameObject rayObject;

    Animator goatAnimator;

    public NavMeshAgent goatNavmesh;
    public Rigidbody rb;

    int randomNumberState;
    float stateIntervall = 10f;

    bool changingState = false;
    bool playerInfront = false;
    bool isInteractingWithPlayer = false;

    [SerializeField] CharacterAgent characterAgent;

    state currentState;
    IEnumerator newState;
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
    //    newState = WaitForNewState(10f);
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

            if(currentState == state.Idle)
            {
                Idle();
            }
            else if (currentState == state.PlayFlute)
            {
                PlayFlute();
            }
            else if (currentState == state.WalkRandom)
            {
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

        if ((Physics.Raycast(rayObject.transform.position, forward, out hit, 10)))
        {
            if (hit.transform.tag == "Player")
            {
                Debug.Log("Player Detected");
                changingState = true;
                currentState = state.PlayerIsInfront;
                playerInfront = true;
            }
            else
            {
                playerInfront = false;
                isInteractingWithPlayer = false;
            }
        }

        Debug.DrawRay(rayObject.transform.position, forward, Color.blue);
    }

    /*    IEnumerator WaitForNewState(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            //    Debug.Log("Changing state");
            changingState = true;
            randomNumberState = Random.Range(0, 2);

            if(randomNumberState == 0)
            {
                currentState = state.PlayFlute;
            }
            else if(randomNumberState == 1)
            {
                currentState = state.WalkRandom;
            }

            Debug.Log("state is: " + currentState);
        }*/


    //Switching state in a couratine
    IEnumerator SwitchState()
    {
        while (true)
        {
            yield return new WaitForSeconds(stateIntervall);

            Debug.Log("Changing state");

            changingState = true;
            randomNumberState = Random.Range(0, 3);

            if(randomNumberState == 0)
            {
                currentState = state.Idle;
            }
            else if (randomNumberState == 1)
            {
                currentState = state.PlayFlute;
            }
            else if (randomNumberState == 2)
            {
                currentState = state.WalkRandom;
            }

            Debug.Log("state is: " + currentState);

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
        goatAnimator.SetBool("PlayFlute", true);
        playFlute.SetActive(true);
        flute.SetActive(false);
        GetComponent<NavMeshAgent>().enabled = false;
    }

    void PlayerInfront()
    {
        if (playerInfront == true && isInteractingWithPlayer == false)
        {
            Debug.Log("Doing PlayerInfront method");

            playerInfront = true;
            GetComponent<NavMeshAgent>().enabled = false;
            goatAnimator.SetTrigger("GreetPlayer");
            goatAnimator.SetBool("PlayerIdle", true);

            isInteractingWithPlayer = true;
        }
    }
}
