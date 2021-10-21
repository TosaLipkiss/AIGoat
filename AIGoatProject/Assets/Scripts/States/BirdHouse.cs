using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FeedBirds();

public class BirdHouse : MonoBehaviour
{
    public static event FeedBirds feed;

    private void OnTriggerEnter(Collider other)
    {
        feed?.Invoke();
    }
}
