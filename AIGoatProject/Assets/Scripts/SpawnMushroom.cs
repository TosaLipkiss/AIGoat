using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMushroom : MonoBehaviour
{
    public GameObject mushroomPrefab;
    private int mushroomCount;

    private void Start()
    {
        {
            GameObject[] mushroomSpots = GameObject.FindGameObjectsWithTag("MushroomSpot");

            for (int i = 0; i < mushroomSpots.Length; i++)
            {
                Instantiate(mushroomPrefab, mushroomSpots[i].transform.position, Quaternion.identity);
            }

            Debug.Log(mushroomSpots.Length);
        }
    }
}
