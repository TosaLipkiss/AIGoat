using UnityEngine;
using UnityEngine.AI;

public class DestinationSwitch : MonoBehaviour
{
    public int positionX;
    public int positionZ;
    public Rigidbody rb;
    public GameObject randomDestination;
    public GameObject birdHouseDestination;

    float speed;
    Vector3 velocity;

    public GameObject characterAI;
    public NavMeshAgent navmesh;
    NavMeshPath navMeshPath;
    public Transform targetPosition;
    float elapsed = 0f;
    bool pathAvailable;

    public bool isBirdHouseDestination = false;

    private void Start()
    {
        navMeshPath = new NavMeshPath();
    }

    private void Update()
    {
        if (Vector3.Distance(characterAI.transform.position, transform.position) < 3f)
        {
            if (characterAI.gameObject.GetComponent<StateMachine>() != null)
            {
                if (characterAI.gameObject.GetComponent<StateMachine>().busy == false)
                {
                    SwitchDestination();
                }
            }
            if (characterAI.gameObject.GetComponent<StateMachineTwo>() != null)
            {
                if (characterAI.gameObject.GetComponent<StateMachineTwo>().busy == false)
                {
                    SwitchDestination();
                }
            }
        }

        if (CalculateNewPath() == true)
        {
            pathAvailable = true;
        }
        else
        {
            pathAvailable = false;
        }

        if (pathAvailable == false)
        {
            SwitchDestination();
        }
    }

    bool CalculateNewPath()
    {
        if (navmesh.isActiveAndEnabled)
        {
            navmesh.CalculatePath(targetPosition.position, navMeshPath);
        }

        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SwitchDestination()
    {
        navmesh.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        navmesh.enabled = true;

        positionX = Random.Range(-19, 18);
        positionZ = Random.Range(-16, 20);

        transform.position = new Vector3(positionX, 0, positionZ);
    }
}
