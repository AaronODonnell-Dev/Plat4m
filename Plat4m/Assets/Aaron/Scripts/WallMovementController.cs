using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovementController : MonoBehaviour
{
    public Rigidbody playerBody;

    private void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    public void MoveUp()
    {
        playerBody.AddForce(-10 * playerBody.mass * playerBody.transform.up * 2);
    }

    public void MoveDown()
    {
        playerBody.AddForce(-10 * playerBody.mass * -playerBody.transform.up * 2);
    }

    public void MoveLeft()
    {
        playerBody.AddForce(-10 * playerBody.mass * new Vector3(0, 0,0) * 2);
    }

    public void MoveRight()
    {
        playerBody.transform.Translate(new Vector3(-0.2f, 0, 0));
    }
}
