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
        playerBody.transform.Translate(new Vector3(0, 0.2f, 0));
    }

    public void MoveDown()
    {
        playerBody.transform.Translate(new Vector3(0, -0.2f, 0));
    }

    public void MoveLeft()
    {
        playerBody.transform.Translate(new Vector3(0.2f, 0, 0));
    }

    public void MoveRight()
    {
        playerBody.transform.Translate(new Vector3(-0.2f, 0, 0));
    }
}
