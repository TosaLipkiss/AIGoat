using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAgent : MonoBehaviour
{
    [SerializeField] GameObject destination;

    NavMeshAgent goatsAgent;

    void Start()
    {
        goatsAgent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        goatsAgent.SetDestination(destination.transform.position);
    }
}
