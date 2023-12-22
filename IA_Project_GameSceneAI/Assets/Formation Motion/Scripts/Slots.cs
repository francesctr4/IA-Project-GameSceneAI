using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour
{
    public int rows;
    public int entitiesInRow;
    public GameObject entityPrefab;

    public GameObject ghost;

    void Start()
    {
        for (int i = 0; i < rows; i++)
        {
            CreateRow(entitiesInRow, -2f - 2 * i, entityPrefab);
        }
    }

    void CreateRow(int num, float z, GameObject pf)
    {
        float pos = 1 - num;
        for (int i = 0; i < num; ++i)
        {
            Vector3 position = ghost.transform.TransformPoint(new Vector3(pos, 0f, z));
            GameObject temp = (GameObject)Instantiate(pf, position, ghost.transform.rotation);
            temp.AddComponent<Formation>();
            temp.GetComponent<Formation>().pos = new Vector3(pos, 0, z);
            temp.GetComponent<Formation>().target = ghost;
            pos += 2f;
        }
    }
}
