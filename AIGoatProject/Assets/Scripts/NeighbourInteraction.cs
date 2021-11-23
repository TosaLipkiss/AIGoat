using UnityEngine;



public delegate void InteractWithNeigbour();
public class NeighbourInteraction : MonoBehaviour
{
    public static event InteractWithNeigbour interactNeigbour;

    public bool interactingWithNeighbour = false;

    public StateMachine stateMachine;
    public StateMachineTwo stateMachineTwo;

    float timer = 10f;

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Neighbour") && timer > 15 && other.transform.root.gameObject != gameObject)
        {
            if (stateMachine.busy == false && stateMachineTwo.busy == false)
            {
                other.transform.root.gameObject.GetComponent<NeighbourInteraction>().timer = 0f;

                timer = 0f;
                interactNeigbour?.Invoke();
            }
        }
    }
}
