using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    PlayerMovement Player;
    Rigidbody playerBody;

    public Vector3 playerPosition;

    public bool collidedWithWall = false;

    public void InstatiatePlayer(PlayerMovement player)
    {
        Player = player;
        playerBody = player.GetComponent<Rigidbody>();
    }

    public void OnCollisionWithWall(Collision collision)
    {
        if (collision.transform.tag == "MovingWall")
        {
            playerPosition = playerBody.transform.position;
            playerBody.transform.position = collision.transform.position;

            //playerBody.AddForce(-10 * playerBody.mass * this.transform.up);
            this.GetComponent<Rigidbody>().useGravity = false;
            playerBody.freezeRotation = true;
            collidedWithWall = true;
            Player.ResetJump();
        }
    }

    public void BasicCollision(Collision collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "MovingPlatform")
        {
            playerBody.freezeRotation = true;
            Player.isGrounded = true;
            Player.isJumping = false;
            Player.ResetJump();
        }

        if (collision.transform.tag == "MovingPlatform")
        {
            playerBody.transform.parent = collision.transform;
        }
    }

    public void OnCollisionEnd(Collision collider)
    {
        if (collider.transform.tag == "MovingPlatform")
        {
            playerBody.transform.parent = null;
            playerBody.freezeRotation = true;
        }

        if (collider.transform.tag == "MovingWall")
        {
            playerBody.freezeRotation = true;
            this.GetComponent<Rigidbody>().useGravity = false;
            collidedWithWall = false;
        }
    }
}
