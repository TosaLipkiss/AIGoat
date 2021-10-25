using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMushrooms : MonoBehaviour
{
    public GameObject closestMushroom;
    public LayerMask mushroomLayer;

    public float distance;
    public float maxAngle;

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Collider[] hits = Physics.OverlapSphere(transform.position, distance, mushroomLayer);

        Collider bestHit = null;

        float bestHitDistance = float.MaxValue;

        for(int i = 0; i < hits.Length; i++)
        {
            Vector3 directionToMushroom = hits[i].transform.position - transform.position;

            if(Vector3.Angle(transform.forward, directionToMushroom) < maxAngle)
            {
                if(bestHit == null || directionToMushroom.sqrMagnitude < bestHitDistance)
                {
                    bestHit = hits[i];
                    bestHitDistance = directionToMushroom.sqrMagnitude;
                }
            }
        }

        if(bestHit != null)
        {
            closestMushroom = bestHit.gameObject;
            Debug.Log("closest muchroom: " + closestMushroom.transform.position);
        }
    }
}
