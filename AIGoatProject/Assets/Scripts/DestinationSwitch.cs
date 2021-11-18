using UnityEngine;
using UnityEngine.AI;

public class DestinationSwitch : MonoBehaviour
{
    public int positionX;
    public int positionZ;
    public NavMeshAgent navmesh;
    public Rigidbody rb;
    public GameObject randomDestination;
    public GameObject birdHouseDestination;

    public GameObject characterAI;

    public bool isBirdHouseDestination = false;


    private void Update()
    {
        if (Vector3.Distance(characterAI.transform.position, transform.position) < 3f)
        {
            if(characterAI.gameObject.GetComponent<StateMachine>() != null)
            {
                if (characterAI.gameObject.GetComponent<StateMachine>().busy == false)
                {
                    Debug.Log("goat switch destination");
                    SwitchDestination();
                }
            }
            if (characterAI.gameObject.GetComponent<StateMachineTwo>() != null)
            {
                if (characterAI.gameObject.GetComponent<StateMachineTwo>().busy == false)
                {
                    Debug.Log("farmer switch destination");
                    SwitchDestination();
                }
            }
        }
    }

    /*    void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Character")
            {
                if (collider.gameObject.GetComponent<CharacterAgent>() != null)
                {
                    if (collider.gameObject.GetComponent<StateMachine>().busy == false)
                    {
                        Debug.Log("trigger new destination for goat");
                        SwitchDestination();
                    }
                }
                else if (collider.gameObject.GetComponent<FarmerAgent>() != null)
                {
                    if (collider.gameObject.GetComponent<StateMachineTwo>().busy == false)
                    {
                        Debug.Log("trigger new destination for farmer");
                        SwitchDestination();
                    }
                }
            }

        }*/

    public void SwitchDestination()
    {
        navmesh.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        navmesh.enabled = true;

        positionX = Random.Range(-5, 18);
        positionZ = Random.Range(-13, 14);

        transform.position = new Vector3(positionX, 0, positionZ);
    }
}
