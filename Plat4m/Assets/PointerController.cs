using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    private int index;

    private float xPosition, yPosition, zPosition;

    private float[] pointerPositions = { 1.04f, 0.28f, -0.36f };
    // Start is called before the first frame update
    void Start()
    {
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        zPosition = transform.position.z;

        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(index)
        {
            case 0:
                yPosition = pointerPositions[0];
                transform.position = new Vector3(xPosition, yPosition, zPosition);
                break;

            case 1:
                yPosition = pointerPositions[1];
                transform.position = new Vector3(xPosition, yPosition, zPosition);
                break;

            case 2:
                yPosition = pointerPositions[2];
                transform.position = new Vector3(xPosition, yPosition, zPosition);
                break;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
            index--;
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            index++;

        if (index >= 3)
        {
            index = 0;
        }
        else if(index < 0)
        {
            index = 3;
        }
    }
}
