using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayScript : MonoBehaviour
{
    Vector3 startAngle;   //Reference to the object's original angle values

    public float rotationSpeed = 1f;  //Speed variable used to control the animation

    public float rotationOffset = 50f; //Rotate by 50 units

    float finalAngle;  //Keeping track of final angle to keep code cleaner

    void Start()
    {
        startAngle = transform.eulerAngles;  // Get the start position
    }

    void Update()
    {
        finalAngle = startAngle.y + (float)Math.Sin(Environment.TickCount * rotationSpeed * rotationOffset);  //Calculate animation angle
        transform.eulerAngles = new Vector3(startAngle.x, finalAngle, startAngle.z); //Apply new angle to object
    }
}
