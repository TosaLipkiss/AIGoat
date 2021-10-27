using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInventory : MonoBehaviour
{
    public int collectedMushrooms;
    public bool newMushroomAdded = false;

    private void Update()
    {
        if(newMushroomAdded == true)
        {
            collectedMushrooms++;
            newMushroomAdded = false;
            Debug.Log("inventory: " + collectedMushrooms);
        }
    }
}
