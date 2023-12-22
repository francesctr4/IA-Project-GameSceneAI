using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFM : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float velocity = 5.0f;

    private float startTime;
    private float journeyLength;

    void Start()
    {
        // Calculate the distance between point A and point B
        journeyLength = Vector3.Distance(pointA.position, pointB.position);

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
        transform.position = Vector3.Lerp(pointA.position, pointB.position, fractionOfJourney);

        // Check if the GameObject has reached point B
        if (fractionOfJourney >= 1.0f)
        {
            // Optionally, you can perform actions when the object reaches point B
            // For example, destroy the GameObject or trigger some event.
            // In this example, we'll just disable the script.
            enabled = false;
        }
    }
}
