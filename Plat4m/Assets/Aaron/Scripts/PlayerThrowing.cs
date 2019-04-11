using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    Rigidbody _p1body;
    Rigidbody _p2body;

    float YeetForce;

    // Start is called before the first frame update
    void Start()
    {
        Player1 = GameObject.FindGameObjectWithTag("Player");
        Player2 = GameObject.FindGameObjectWithTag("Player2");

        _p1body = Player1.GetComponent<Rigidbody>();
        _p2body = Player2.GetComponent<Rigidbody>();

        YeetForce = 100 * 25;
    }

    // Update is called once per frame
    void Update()
    {
        var heading = Player1.transform.position - Player2.transform.position;

        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.

        if (heading.sqrMagnitude < 5 * 5 && Input.GetKeyDown(KeyCode.Y))
        {
            _p1body.AddForce(Vector3.up * YeetForce, ForceMode.Force);
            // Target is within range.
        }
    }
}
