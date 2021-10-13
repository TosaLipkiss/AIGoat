using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationSwitch : MonoBehaviour
{
    public int positionX;
    public int positionZ;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Character")
        {
            positionX = Random.Range(-10, 12);
            positionZ = Random.Range(-20, 30);

            this.gameObject.transform.position = new Vector3(positionX, 0, positionZ);
        }
    }
}
