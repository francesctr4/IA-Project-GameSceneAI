using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSM : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float idleTime = 2f;
    public AnimationClip[] idleAnimations;
    public AnimationClip wanderAnimation;

    private enum State
    {
        Idle,
        Wander
    }

    private State currentState = State.Idle;

    private NavMeshAgent agent;
    private Animator animator;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Idle:
                    yield return StartCoroutine(IdleState());
                    break;
                case State.Wander:
                    yield return StartCoroutine(WanderState());
                    break;
            }
        }
    }

    private IEnumerator IdleState()
    {
        animator.SetBool("IsIdle", true);

        // Play a random idle animation
        PlayRandomIdleAnimation();

        yield return new WaitForSeconds(idleTime);
        
        ChangeState(State.Wander);
    }

    private void PlayRandomIdleAnimation()
    {
        if (idleAnimations.Length > 0 && animator != null)
        {
            int randomIndex = Random.Range(0, idleAnimations.Length);
            animator.Play(idleAnimations[randomIndex].name);
        }
    }

    private IEnumerator WanderState()
    {
        animator.SetBool("IsIdle", false);

        animator.Play(wanderAnimation.name);

        Vector3 randomPoint = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(randomPoint);

        // Wait until the agent reaches the destination
        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            yield return null;
        }

        ChangeState(State.Idle);
    }

    private void ChangeState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Idle:
                // Additional idle state setup can be added here
                
                break;
            case State.Wander:
                // Additional wander state setup can be added here
               
                break;
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
