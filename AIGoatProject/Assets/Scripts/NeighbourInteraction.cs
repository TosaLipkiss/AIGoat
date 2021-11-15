using UnityEngine;



public delegate void InteractWithNeigbour();
public class NeighbourInteraction : MonoBehaviour
{
    public static event InteractWithNeigbour interactNeigbour;

    public bool interactingWithNeighbour = false;

    public StateMachine stateMachine;
    public StateMachineTwo stateMachineTwo;

    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Neighbour") && timer > 15f)
        {
            if (interactingWithNeighbour == false && stateMachine.busy == false && stateMachineTwo.busy == false)
            {
                interactingWithNeighbour = true;

                timer = 0f;
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
