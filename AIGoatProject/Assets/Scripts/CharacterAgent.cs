using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAgent : MonoBehaviour
{
    public GameObject destination;

    public Animator goatAnimator;

    public bool stateIsWalking;

    [SerializeField] AudioClip walkingSound;

    public NavMeshAgent goatsAgent;

    private void Start()
    {
        goatAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(stateIsWalking == true)
        {
            WalkAround();
        }
    }

    public void WalkAround()
    {
        GetComponent<NavMeshAgent>().enabled = true;
        goatsAgent = GetComponent<NavMeshAgent>();
        goatsAgent.speed = 1.5f;
        goatsAgent.SetDestination(destination.transform.position);
        goatAnimator.SetBool("Walk", true);
    }
}
