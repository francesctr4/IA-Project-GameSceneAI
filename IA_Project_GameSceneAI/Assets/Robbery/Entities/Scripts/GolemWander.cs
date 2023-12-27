using UnityEngine;
using UnityEngine.AI;

public class GolemWander : MonoBehaviour
{
    [Header("Wander Settings")]
    public LayerMask navMeshLayer;
    public float range;
    public float waitingTime = 2f;

    // General Private Settings
    private NavMeshAgent agent;
    private Animator animator;

    // Wander Private Settings

        // Desination parameters
    private Vector3 destinationPoint;
    private bool walkpointSet;

        // Time parameters
    private float actualTime = 0f;
    private float waitingTimeStuck = 4f;
    private float actualTimeStuck = 0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Wander();
    }

    public void Wander()
    {
        agent.speed = 2;
        animator.SetBool("IsWalking", true);

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

            animator.SetBool("IsWalking", false);

            if (actualTime > waitingTime)
            {
                animator.SetBool("IsWalking", true);
                walkpointSet = false;

                actualTime = 0f;
                actualTimeStuck = 0f;
            }
        }

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
