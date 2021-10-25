using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FeedBirds();

public class BirdHouse : MonoBehaviour
{
    public static event FeedBirds feed;
    public bool birdHouseOnCooldown = true;
    float cooldownTimer = 120f;

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

/*        if(cooldownTimer <= 0f)
        {
            birdHouseOnCooldown = false;
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Character") && birdHouseOnCooldown == false)
        {
            Debug.Log("Trigger Feed bird");
            feed?.Invoke();
            cooldownTimer = 120f;
        //    birdHouseOnCooldown = true;
        }
    }
}
