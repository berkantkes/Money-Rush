using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject player;

    float deltaZ;

    void Start()
    {
        deltaZ = transform.position.z - target.position.z;
    }

    void Update()
    {
        bool ifGameContinue = player.GetComponent<Player>().gameContinue;

        if (ifGameContinue)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z + deltaZ);
        }
    }
}
