using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    private float rotateSpeed = 5;
    private bool ifTurn = false;

  

    public void FixedUpdate()
    {

    }

    public void TurnLeft()
    {
        ifTurn = true;
        Quaternion newRotation = Quaternion.Euler(0, -15f, 90);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotateSpeed);

    }
    public void TurnRight()
    {
        ifTurn = true;
        Quaternion newRotation = Quaternion.Euler(0, +15, 90);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotateSpeed);


    }
    public void Idle()
    {
        if (ifTurn)
        {
            Quaternion newRotation = Quaternion.Euler(0, 0, 90);
            this.transform.rotation = newRotation;
            ifTurn = false;
        }
        transform.RotateAround(transform.position, Vector3.right, 90f * Time.deltaTime);

    }

}