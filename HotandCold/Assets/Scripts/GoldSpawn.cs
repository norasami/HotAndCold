using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSpawn : MonoBehaviour
{
    Vector3[] goldLocation = new Vector3[] {
    new Vector3(-16, 9, 0),
    new Vector3(0, 9, 0),
    new Vector3(16, 9, 0),
    new Vector3(-16, -9, 0),
    new Vector3(0, -9, 0),
    new Vector3(16, -9, 0)
        };




    // Start is called before the first frame update
    void Start()
    {

        this.transform.position = goldLocation[Random.Range(0, 5)];

    }
}
