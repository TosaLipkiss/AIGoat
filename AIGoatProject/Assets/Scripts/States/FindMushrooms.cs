using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void PickMushroom();
public class FindMushrooms : MonoBehaviour
{
    public static event PickMushroom pick;
    public SpawnMushroom spawnMushroom;
    public AIInventory aiInventory;

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
            pick?.Invoke();
      //     Debug.Log("closest muchroom: " + closestMushroom.transform.position);
        }
    }

    public void DestroyClosestMushroom()
    {
        for(int i = 0; i < spawnMushroom.mushroomList.Count; i++)
        {
            if(spawnMushroom.mushroomList[i] == closestMushroom)
            {
                if (spawnMushroom.mushroomList[i].activeInHierarchy == true)
                {
                    spawnMushroom.mushroomList[i].transform.position = SpawnMushroom.NavMeshUtil.GetRandomPoint(spawnMushroom.platform.transform.position, 20f);
                    spawnMushroom.mushroomList[i].SetActive(false);
                    break;
                }
            }
        }
    }
}
