using UnityEngine;



public delegate void InteractWithNeigbour();
public class NeighbourInteraction : MonoBehaviour
{
    public static event InteractWithNeigbour interactNeigbour;

    public bool interactingWithNeighbour = false;

    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Neighbour"))
        {
            if (interactingWithNeighbour == false)
            {
                interactingWithNeighbour = true;

                timer = 0f;
                Debug.Log("colliding with neighbour");
                interactNeigbour?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(timer > 10f)
        {
            interactingWithNeighbour = false;
        }
    }
}
