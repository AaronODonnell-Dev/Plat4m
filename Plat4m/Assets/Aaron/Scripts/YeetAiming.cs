using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetAiming : MonoBehaviour
{
    public GameObject Ground;
    public GameObject PlayerBody;
    public Canvas Landing;

    public PlayerThrowing player;

    Vector3 groundHit;

    void Start()
    {
        PlayerBody = GameObject.FindGameObjectWithTag("Player");
        Ground = GameObject.FindGameObjectWithTag("Ground");
        player = new PlayerThrowing();
    }

    public void PredictedGroundHit(GameObject ground, GameObject player1)
    {
        Debug.Log(player.wasYeeted);

        Rigidbody playerBody = player1.GetComponent<Rigidbody>();

        float h = ground.transform.position.y - (transform.position.y + playerBody.transform.localScale.y);
        float g = Physics.gravity.magnitude;
        float vel = playerBody.velocity.y;

        float t = vel / g + Mathf.Sqrt(vel * vel / (g * g) - 2 * h / g);

        float x = transform.position.x + playerBody.velocity.x * t;
        float z = transform.position.z + playerBody.velocity.z * t;

        Debug.Log("Setting Ground position");
        groundHit = new Vector3(x, Ground.transform.position.y, z);
    }

    void Update()
    {
        if (player.wasYeeted)
        {
            PredictedGroundHit(Ground, PlayerBody);
        }

        if (player.wasYeeted && groundHit != null)
        {
            Debug.Log("Into If");
            Landing.enabled = true;
            Landing.transform.position = groundHit;
            player.wasYeeted = false;
        }
    }
}
