using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    Player player;
    Rigidbody playerBody;
    RaycastHit Hit;
    Yeet yeet;

    public bool collidedWithWall = false;
    public bool collisionEnded = false;

    void Start()
    {
        yeet = GetComponent<Yeet>();
        player = GetComponent<Player>();
        playerBody = GetComponent<Rigidbody>();
        Hit = yeet.hit;
    }

    public void BasicCollision(Collision collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "MovingPlatform")
        {
            if (yeet.wasYeeted)
            {
                yeet.wasYeeted = false;
            }
            playerBody.freezeRotation = true;
            player.isGrounded = true;
            player.isJumping = false;
            player.ResetJump();
        }

        if (collision.transform.tag == "MovingPlatform")
        {
            playerBody.transform.parent = collision.transform;
        }
    }

    public void OnCollisionEnd(Collision collision)
    {
        if (collision.transform.tag == "MovingPlatform" || collision.transform.tag == "Ground")
        {
            playerBody.transform.parent = null;
            //playerBody.freezeRotation = false;
            player.isGrounded = false;
        }
    }
}
