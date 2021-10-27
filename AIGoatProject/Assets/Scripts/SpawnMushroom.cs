using UnityEngine;
using System;
using UnityEngine.AI;

public class SpawnMushroom : MonoBehaviour
{
    public GameObject mushroomPrefab;
    public GameObject mushroomSpawnPositionPrefab;
    public GameObject platform;
    private int mushroomCount;

    float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if(spawnTimer > 2f)
        {
            Vector3 spawnPosition = NavMeshUtil.GetRandomPoint(platform.transform.position, 20f);
            Debug.Log(spawnPosition);

            Instantiate(mushroomSpawnPositionPrefab, spawnPosition, Quaternion.identity);
            spawnTimer = 0f;

            GameObject[] mushroomSpots = GameObject.FindGameObjectsWithTag("MushroomSpot");

            for (int i = 0; i < mushroomSpots.Length; i++)
            {
                Instantiate(mushroomPrefab, mushroomSpots[i].transform.position, Quaternion.identity);
            }
        }     
    }
}

public static class NavMeshUtil
{
    static int count = 0;
    // Get Random Point on a Navmesh surface
    public static Vector3 GetRandomPoint(Vector3 center, float maxDistance)
    {
        count++;
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit; 

        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

        if(hit.position.x == Mathf.Infinity) //lösning på infinity utanför banan, recursive counter
        {
            if(count == 10)
            {
                throw new ArgumentException("MushroomSpawner not working!(infinity)");
            }

            return GetRandomPoint(center, maxDistance);
        }

        count = 0;

        return hit.position;
    }
}
