using UnityEngine;

public class RaycastForward : MonoBehaviour
{
    public GameObject rayObject;
    public LayerMask playerLayer;

    public float rayThickness = 1f;
    public float rayLenght = 10f;

    public bool RaycastPlayer()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Debug.DrawRay(rayObject.transform.position, forward * rayLenght, Color.blue);

        return Physics.SphereCast(rayObject.transform.position, rayThickness, forward, out hit, rayLenght, playerLayer);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(rayObject.transform.position, rayThickness);
    }
}
