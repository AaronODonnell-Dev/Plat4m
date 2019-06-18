using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMoveZ : MonoBehaviour
{
    bool movingFoward;
    float moveTime;
    public float changeDirection = 8;
    // Start is called before the first frame update
    void Start()
    {
        moveTime = changeDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingFoward)
        {
            transform.position += transform.forward * Time.deltaTime;
        }
        else if (!movingFoward)
        {
            transform.position += -transform.forward * Time.deltaTime;
        }

        moveTime -= Time.deltaTime;
        if (moveTime <= 0)
        {
            movingFoward = !movingFoward;
            moveTime = changeDirection;
        }
    }
}
