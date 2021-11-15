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
            if (other.gameObject.GetComponent<CharacterAgent>())
            {
                feed?.Invoke();

                cooldownTimer = 10f;

                stillFeedingBird = true;
                birdHouseOnCooldown = true;
            }
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
