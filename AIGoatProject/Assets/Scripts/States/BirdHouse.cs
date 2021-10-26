using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FeedBirds();

public class BirdHouse : MonoBehaviour
{
    public static event FeedBirds feed;
    bool birdHouseOnCooldown = false;
    bool stillFeedingBird = false;
    float cooldownTimer = 0f;

    private void Update()
    {
        Debug.Log(stillFeedingBird);
        cooldownTimer -= Time.deltaTime;

        if(cooldownTimer <= 0f)
        {
            birdHouseOnCooldown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Character") && birdHouseOnCooldown == false)
        {
            feed?.Invoke();

            cooldownTimer = 10f;

            stillFeedingBird = true;
            birdHouseOnCooldown = true;
        }
    }
}
