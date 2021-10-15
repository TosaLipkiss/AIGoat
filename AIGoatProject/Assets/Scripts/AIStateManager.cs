using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIStateManager : MonoBehaviour
{
    public GameObject playFlute;
    public GameObject flute;
    public GameObject rayObject;

    Animator goatAnimator;

    int randomNumberState;
    bool changingState = false;

    [SerializeField] CharacterAgent characterAgent;

    state currentState;
    IEnumerator newState;
    enum state
    {
        WalkRandom,
        Idle,
        PlayFlute
    }
    void Start()
    {
        goatAnimator = GetComponent<Animator>();
        newState = WaitForNewState(10f);
        currentState = state.Idle;
    }

    void Update()
    {
        StartCoroutine(newState);

        if (changingState == true)
        {
            ResetStates();

            if (currentState == state.PlayFlute)
            {
                PlayFlute();
            }

            else if (currentState == state.WalkRandom)
            {
                characterAgent.stateIsWalking = true;
            }
        }

        changingState = false;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;

        if ((Physics.Raycast(rayObject.transform.position, forward, out hit, 15)))
        {
            if(hit.transform.tag == "Player")
            {
                Debug.Log("Player detected");
            }
        }

        Debug.DrawRay(rayObject.transform.position, forward, Color.blue);
    }

    IEnumerator WaitForNewState(float waitTime)
    {

        Debug.Log("Changing state");
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

        yield return new WaitForSeconds(waitTime);
    }






    //STATE METHODS
    void PlayFlute()
    {
        goatAnimator.SetBool("PlayFlute", true);
        playFlute.SetActive(true);
        flute.SetActive(false);
        GetComponent<NavMeshAgent>().enabled = false;
    }

    void ResetStates()
    {
        characterAgent.stateIsWalking = false;
        goatAnimator.SetBool("Walk", false);
        goatAnimator.SetBool("PlayFlute", false);
    }
}
