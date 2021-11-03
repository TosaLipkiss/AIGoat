using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMushroom : MonoBehaviour
{
    public GameObject mushroomPrefab;
    public GameObject mushroomSpawnPositionPrefab;
    public GameObject mushroomSpawner;
    public GameObject platform;
    public GameObject[] mushroomSpots;
    private int mushroomCount;

    float spawnTimer;

    Vector3 spawnPosition;
    public int spawnCount = 10;
    public List<GameObject> mushroomList;

    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            spawnPosition = NavMeshUtil.GetRandomPoint(platform.transform.position, 20f);
            GameObject newMushroomSpot = Instantiate(mushroomSpawnPositionPrefab, spawnPosition, Quaternion.identity) as GameObject;

            mushroomList.Add(newMushroomSpot);

            newMushroomSpot.transform.parent = mushroomSpawner.transform;

            newMushroomSpot.SetActive(false);

        //    ActivateMushroom();
        }
    }


    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > 20f)
        {
            ActivateMushroom();
            spawnTimer = 0f;
        }
    }

    void ActivateMushroom()
    {
        for (int i = 0; i < mushroomList.Count; i++)
        {
            if (mushroomList[i].activeInHierarchy == false)
            {
                mushroomList[i].SetActive(true);
                break;
            }
        }
    }

    // Get Random Point on a Navmesh surface
    public static class NavMeshUtil
    {
        static int count = 0;

        public static Vector3 GetRandomPoint(Vector3 center, float maxDistance)
        {
            count++;
            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * maxDistance + center;

            NavMeshHit hit;

            NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

            if (hit.position.x == Mathf.Infinity) //lösning på infinity utanför banan, recursive counter
            {
                if (count == 10)
                {
                    throw new ArgumentException("MushroomSpawner not working!(infinity)");
                }

                return GetRandomPoint(center, maxDistance);
            }

            count = 0;

            return hit.position;
        }
    }
}
