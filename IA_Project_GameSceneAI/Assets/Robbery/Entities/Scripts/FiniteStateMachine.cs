using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FiniteStateMachine : MonoBehaviour
{
    public Transform enemy;
    public GameObject item;
    public float dist2Steal = 10f;
    private AIMovement movement;
    private AIVision golemVision;

    private WaitForSeconds wait = new WaitForSeconds(0.05f); // == 1/20

    delegate IEnumerator State();
    private State state;
    private NavMeshAgent agent;

    IEnumerator Start()
    {
        // Get references to the AI's movement script and NavMeshAgent component
        movement = gameObject.GetComponent<AIMovement>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        golemVision = enemy.GetComponent<AIVision>();

        yield return wait;

        state = Wander;

        // Continuously run the current state
        while (enabled)
            yield return StartCoroutine(state());
    }

    IEnumerator Wander()
    {
        Debug.Log("Wander state");

        agent.speed = 2;

        // While the enemy is not far enough to the item to steal it...
        while (Vector3.Distance(enemy.position, item.transform.position) < dist2Steal || golemVision.playerDetected)
        {
            movement.currentState = AIMovement.MovementState.WANDER;

            yield return wait;
        };

        // If the enemy is far enough to be able to stole the item, transition to the Approaching state.
        if (Vector3.Distance(enemy.position, item.transform.position) >= dist2Steal && !golemVision.playerDetected)
        {
            state = Approaching;
        }
        
    }

    IEnumerator Approaching()
    {
        Debug.Log("Approaching state");

        movement.currentState = AIMovement.MovementState.SEEK;
        agent.speed = 5;

        bool stolen = false;

        // While the enemy is far away enough to steal the item...
        while (Vector3.Distance(enemy.position, item.transform.position) >= dist2Steal && !golemVision.playerDetected)
        {
            // If the AI is very close to the item, mark it as stolen and break the loop.
            if (Vector3.Distance(item.transform.position, transform.position) < 4f)
            {
                stolen = true;
                break;
            };

            yield return wait;
        };

        if (stolen)  // If the item was stolen, hide it and transition to the Hiding state.
        {
            item.GetComponent<Renderer>().enabled = false;
            Debug.Log("Stolen");
            state = Hiding;
        }
        else // If the item wasn't stolen, go back to the Wander state.
        {
            state = Wander;
        }

    }

    IEnumerator Hiding()
    {
        Debug.Log("Hiding state");

        agent.speed = 4;

        while (true)
        {
            movement.currentState = AIMovement.MovementState.HIDE;

            yield return wait;
        };
    }

}
