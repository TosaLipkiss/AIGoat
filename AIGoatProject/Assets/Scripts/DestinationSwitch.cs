using System.Collections;
using System.Collections.Generic;
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
        if(collider.tag == "Character")
        {
            SwitchDestination();
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

        positionX = Random.Range(-10, 12);
        positionZ = Random.Range(-20, 30);

       if(isBirdHouseDestination == true)
        {
            randomDestination.transform.position = new Vector3(birdHouseDestination.transform.position.x, 0, birdHouseDestination.transform.position.z + 0.4f);
        }
        else
        {
            randomDestination.transform.position = new Vector3(positionX, 0, positionZ);
        }
    }
}
