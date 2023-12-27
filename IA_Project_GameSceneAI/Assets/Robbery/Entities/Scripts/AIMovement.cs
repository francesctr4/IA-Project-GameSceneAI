using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [Header("Wander Settings")]
    public LayerMask navMeshLayer;
    public float range;
    public float waitingTime = 2f;

    [Header("Seek Settings")]
    public GameObject seekTarget;

    [Header("Hide Settings")]
    public GameObject hideFromTarget;

    // General Private Settings
    private NavMeshAgent agent;
    private Animator animator;

    // Wander Private Settings

        // Desination parameters
    private Vector3 destinationPoint;
    private bool walkpointSet;

        // Time parameters
    private float actualTime = 0f;
    private float waitingTimeStuck = 2f;
    private float actualTimeStuck = 0f;

    private float actualHideTime = 0f;
    private float hideTime = 0.5f;

    // Hide Private Settings
    private GameObject[] hidingSpots;

    // Movement States
    public enum MovementState
    {
        WANDER,
        SEEK,
        HIDE
    }

    public MovementState currentState;

    // Start is called before the first frame update
    void Start()
    { 
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();

        hidingSpots = GameObject.FindGameObjectsWithTag("HidingSpot");

        animator.SetBool("IsWalking", true);
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRunning", false);

        currentState = MovementState.WANDER;
    }

    void Update()
    {
        switch (currentState)
        {
            case MovementState.WANDER: 
                Wander();
                break;

            case MovementState.SEEK:
                Seek(seekTarget.transform.position);
                break;

            case MovementState.HIDE:

                actualHideTime += Time.deltaTime;

                if (actualHideTime > hideTime)
                {
                    Hide();

                    actualHideTime = 0f;
                  
                }
      
                break;

        }
        
    }

    public void Wander()
    {
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRunning", false);

        if (!walkpointSet)
        {
            SearchForDestination();
        }

        if (walkpointSet)
        {
            agent.SetDestination(destinationPoint);
        }

        actualTimeStuck += Time.deltaTime;

        if (Vector3.Distance(transform.position, destinationPoint) < 0.2 || actualTimeStuck > waitingTimeStuck)
        {
            actualTime += Time.deltaTime;

            animator.SetBool("IsIdle", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);

            if (actualTime > waitingTime)
            {
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsRunning", false);
                walkpointSet = false;

                actualTime = 0f;
                actualTimeStuck = 0f;

            }

        }

    }

    public void Seek(Vector3 target)
    {
        agent.SetDestination(target);

        if(agent.remainingDistance < 0.2)
        {
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", false);
        }
    }

    public void Hide()
    {
        animator.SetBool("IsRunning", true);

        // Initialize variables
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = hidingSpots[0];

        // Loop through an array of hiding spots
        for (int i = 0; i < hidingSpots.Length; i++)
        {
            // Calculate the direction from the current hiding spot to the target
            Vector3 hideDir = hidingSpots[i].transform.position - hideFromTarget.transform.position;

            // Calculate a new position for the character to hide, which is 100 units in the direction of the hiding spot
            Vector3 hidePos = hidingSpots[i].transform.position + hideDir.normalized * 100;

            // Check if the distance between the target and the new hide position is less than the current minimum distance
            if (Vector3.Distance(hideFromTarget.transform.position, hidePos) < dist)
            {
                // Update the variables
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = hidingSpots[i];
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        // Get the collider component from the chosen hiding spot
        Collider hideCol = chosenGO.GetComponent<Collider>();

        // Create a ray that points backward from the chosen spot
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);

        // Initialize a RaycastHit object and a maximum distance for the ray to store collision information
        RaycastHit info;
        float distance = 250.0f;

        // Cast the ray from the chosen spot in the opposite direction to check for collisions
        hideCol.Raycast(backRay, out info, distance);
        
        // Seek to the point of collision adjusted by the chosen direction
        Seek(info.point + chosenDir.normalized);

    }

    private void SearchForDestination()
    {
        float z = UnityEngine.Random.Range(-range, range);
        float x = UnityEngine.Random.Range(-range, range);

        destinationPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destinationPoint, Vector3.down, navMeshLayer))
        {
            walkpointSet = true;
        }
    }

}
