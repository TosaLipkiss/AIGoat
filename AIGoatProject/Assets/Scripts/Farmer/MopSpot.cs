using UnityEngine;

public delegate void MopGround();

public class MopSpot : MonoBehaviour
{
    public static event MopGround sweep;
    bool mopOnCooldown = false;
    bool stillSweeping = false;
    float cooldownTimer = 0f;
    float sweepTimer = 0f;

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f && !stillSweeping)
        {
            mopOnCooldown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && mopOnCooldown == false)
        {
            cooldownTimer = 20f;

            stillSweeping = true;
            mopOnCooldown = true;

            sweep?.Invoke();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            stillSweeping = false;
        }
    }
}