using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MopGround();

public class MopSpot : MonoBehaviour
{
    public static event FeedBirds mop;
    bool birdHouseOnCooldown = false;
    bool stillFeedingBird = false;
    float cooldownTimer = 0f;

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f && !stillFeedingBird)
        {
            birdHouseOnCooldown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && birdHouseOnCooldown == false)
        {
            mop?.Invoke();

            cooldownTimer = 10f;

            stillFeedingBird = true;
            birdHouseOnCooldown = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            stillFeedingBird = false;
        }
    }
}