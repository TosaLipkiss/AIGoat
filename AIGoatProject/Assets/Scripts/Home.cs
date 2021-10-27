using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GoingHome();
public class Home : MonoBehaviour
{
    public static event GoingHome emptyPockets;
    public bool inventoryFull = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && inventoryFull == true)
        {
            inventoryFull = false;
            emptyPockets?.Invoke();
        }
    }
}
