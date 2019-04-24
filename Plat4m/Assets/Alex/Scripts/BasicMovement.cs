using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            ball.transform.Translate(new Vector3(0,0,1f*Time.deltaTime));
        if (Input.GetKey(KeyCode.S))
            ball.transform.Translate(new Vector3(0, 0, -1f * Time.deltaTime));
        if (Input.GetKey(KeyCode.A))
            ball.transform.Translate(new Vector3(-1f* Time.deltaTime,0,0));
        if (Input.GetKey(KeyCode.D))
            ball.transform.Translate(new Vector3(1f * Time.deltaTime, 0, 0));
        if (Input.GetKey(KeyCode.Q))
            ball.transform.Rotate(new Vector3(0,100f * Time.deltaTime,0));
        if (Input.GetKey(KeyCode.E))
            ball.transform.Rotate(new Vector3(0, -100f * Time.deltaTime, 0));

    }
}
