using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetAiming : MonoBehaviour
{
    public GameObject Ground;
    public GameObject Landing;

    public Yeet player;

    Vector3 groundHit;

    void Start()
    {
        Ground = GameObject.FindGameObjectWithTag("Ground");
        player = GetComponent<Yeet>();
    }

    public void PredictedGroundHit(GameObject ground)
    {
        float h = ground.transform.position.y - (transform.position.y + transform.localScale.y);
        float g = Physics.gravity.magnitude;
        float vel = GetComponent<Rigidbody>().velocity.y;

        float t = vel / g + Mathf.Sqrt(vel * vel / (g * g) - 2 * h / g);

        float x = transform.position.x + GetComponent<Rigidbody>().velocity.x * t;
        float z = transform.position.z + GetComponent<Rigidbody>().velocity.z * t;

        groundHit = new Vector3(x, ground.transform.position.y, z);
    }

    void Update()
    {
        if (player.wasYeeted)
        {
            PredictedGroundHit(Ground);
        }

        if (player.wasYeeted && groundHit != null)
        {
            Landing.transform.position = groundHit;
            Landing.transform.localPosition = groundHit;
            Landing.SetActive(true);
            Debug.Log(Landing.activeSelf);
            player.wasYeeted = false;
        }
    }
}
