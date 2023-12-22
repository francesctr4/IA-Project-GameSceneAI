using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFlocking : MonoBehaviour
{
    public Transform[] patrolPoints;  // Array of patrol points
    public float velocity = 5.0f;
    public float delayBetweenPoints = 2.0f;  // Time delay between reaching a point and moving to the next one

    private int currentPatrolIndex = 0; // Index of the current patrol point
    private float startTime;
    private float journeyLength;

    void Start()
    {
        SetDestination(patrolPoints[currentPatrolIndex]);  // Set the initial destination

        // Record the start time
        startTime = Time.time;
    }

    void Update()
    {
        // Calculate the distance covered
        float distanceCovered = (Time.time - startTime) * velocity;

        // Calculate the fraction of the journey completed
        float fractionOfJourney = distanceCovered / journeyLength;

        // Move the GameObject along the path
        transform.position = Vector3.Lerp(patrolPoints[currentPatrolIndex].position, patrolPoints[(currentPatrolIndex + 1) % patrolPoints.Length].position, fractionOfJourney);

        // Check if the GameObject has reached the current patrol point
        if (fractionOfJourney >= 1.0f)
        {
            // Wait for the specified delay before moving to the next point
            if (Time.time - startTime >= delayBetweenPoints)
            {
                // Move to the next patrol point
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

                // Set the next destination and reset start time
                SetDestination(patrolPoints[currentPatrolIndex]);
                startTime = Time.time;
            }
        }
    }

    // Function to set the destination and calculate journey length
    void SetDestination(Transform destination)
    {
        journeyLength = Vector3.Distance(transform.position, destination.position);
    }
}
