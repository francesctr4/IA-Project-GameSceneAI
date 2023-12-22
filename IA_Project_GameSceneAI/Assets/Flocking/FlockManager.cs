using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject flockPrefab;
    public Transform flockTransform;
    public GameObject[] flockArray;

    [Header("Flock Settings")]

    public int numFlock = 20;
    public Vector3 flockLimits = new Vector3(5, 5, 5);

    [Range(0.0f, 20.0f)] public float minSpeed;
    [Range(0.0f, 20.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        flockArray = new GameObject[numFlock];

        for (int i = 0; i < numFlock; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-flockLimits.x, flockLimits.x),
                                                                Random.Range(-flockLimits.y, flockLimits.y), 
                                                                Random.Range(-flockLimits.z, flockLimits.z));

            flockArray[i] = Instantiate(flockPrefab, pos, Quaternion.identity);
            flockArray[i].GetComponent<Flock>().SetFlockManager(this);
        }

        flockTransform = transform;

    }

    // Update is called once per frame
    void Update()
    {
        flockTransform = this.transform;
    }
}
