using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAgent : MonoBehaviour
{
    [SerializeField] GameObject destination;
    Animator goatAnimator;

    NavMeshAgent goatsAgent;

    private void Start()
    {
        goatAnimator = GetComponent<Animator>();
    }

    public void WalkAround()
    {
        goatAnimator.SetBool("Walk", true);
        goatsAgent = GetComponent<NavMeshAgent>();
        goatsAgent.speed = 1.5f;
        goatsAgent.SetDestination(destination.transform.position);
    }
}
