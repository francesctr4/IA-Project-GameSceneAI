using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    private FlockManager FM;

    private float speed;
    private bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(FM.minSpeed, FM.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Bounds bounds = new Bounds(FM.transform.position, FM.flockLimits * 2);

        if (!bounds.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = FM.transform.position - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  FM.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10) // Probability of 10% to change speed
            {
                speed = Random.Range(FM.minSpeed, FM.maxSpeed);
            }

            if (Random.Range(0, 100) < 50) // Probability of 50% to apply flock rules
            {
                ApplyRules();
            }
        }
        
        transform.Translate(0,0,speed * Time.deltaTime);
    }

    public void SetFlockManager(FlockManager manager)
    {
        FM = manager;
    }

    private void ApplyRules()
    {
        GameObject[] gameObjects;
        gameObjects = FM.flockArray;

        Vector3 vCentre = Vector3.zero; 
        Vector3 vAvoid = Vector3.zero;
        float groupSpeed = 0.01f;
        float neighbourDistance;
        int groupSize = 0;

        foreach (GameObject go in gameObjects)
        {
            if (go != gameObject)
            {
                neighbourDistance = Vector3.Distance(go.transform.position, transform.position);

                if (neighbourDistance <= FM.neighbourDistance)
                {
                    vCentre += go.transform.position;
                    groupSize++;

                    if (neighbourDistance < 5.0f)
                    {
                        vAvoid = vAvoid + (transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    groupSpeed = groupSpeed + anotherFlock.speed;
                }

            }

        }

        if (groupSize > 0)
        {
            vCentre = vCentre / groupSize + (FM.flockTransform.position - this.transform.position);
            speed = groupSpeed / groupSize;

            if (speed > FM.maxSpeed)
            {
                speed = FM.maxSpeed;
            }

            Vector3 direction = (vCentre + vAvoid) - transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                      Quaternion.LookRotation(direction), 
                                                      FM.rotationSpeed * Time.deltaTime);
            }

        }

    }

}
