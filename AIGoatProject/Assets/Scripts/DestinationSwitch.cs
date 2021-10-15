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



    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Character")
        {
            goatNavmesh.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.isKinematic = true;
            rb.useGravity = false;
            goatNavmesh.enabled = true;

            positionX = Random.Range(-10, 12);
            positionZ = Random.Range(-20, 30);

            this.gameObject.transform.position = new Vector3(positionX, 0, positionZ);
        }
    }
}
