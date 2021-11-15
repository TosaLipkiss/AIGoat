using UnityEngine;
using UnityEngine.AI;

public class DestinationSwitch : MonoBehaviour
{
    public int positionX;
    public int positionZ;
    public NavMeshAgent goatNavmesh;
    public Rigidbody rb;
    public GameObject randomDestination;
    public GameObject birdHouseDestination;

    public bool isBirdHouseDestination = false;


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Character")
        {
            if (collider.gameObject.GetComponent<CharacterAgent>() != null)
            {
                if (collider.gameObject.GetComponent<StateMachine>().busy == false)
                {
                    SwitchDestination();
                }
            }
            else if (collider.gameObject.GetComponent<FarmerAgent>() != null)
            {
                if (collider.gameObject.GetComponent<StateMachineTwo>().busy == false)
                {
                    SwitchDestination();
                }
            }
        }

    }

    public void SwitchDestination()
    {
        goatNavmesh.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        goatNavmesh.enabled = true;

        positionX = Random.Range(-5, 18);
        positionZ = Random.Range(-13, 14);

        randomDestination.transform.position = new Vector3(positionX, 0, positionZ);
    }
}
