using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIStateManager : MonoBehaviour
{
    public GameObject playFlute;
    public GameObject flute;
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
                characterAgent.WalkAround();
            }
        }

        changingState = false;
    }

    IEnumerator WaitForNewState(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

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
    }

    void PlayFlute()
    {
        goatAnimator.SetBool("PlayFlute", true);
        playFlute.SetActive(true);
        flute.SetActive(false);
        GetComponent<NavMeshAgent>().enabled = false;
    }

    void ResetStates()
    {
        goatAnimator.SetBool("Walk", false);
        goatAnimator.SetBool("PlayFlute", false);
    }
}
