using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    PlayerMovement Player;
    Rigidbody playerBody;
    WallMovementController wallMovement;

    public Vector3 playerPosition;

    bool collidedWithWall = false;

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
            playerBody.transform.position = collision.transform.position + new Vector3(0, 0, 0.25f);
            collidedWithWall = true;
            playerBody.AddForce(-10 * playerBody.mass * this.transform.up);
            this.GetComponent<Rigidbody>().useGravity = false;
            playerBody.freezeRotation = true;
            Player.ResetJump();

            if (Input.GetKeyDown(KeyCode.Q) && collidedWithWall)
            {
                if(Input.GetKeyDown(KeyCode.Alpha8))
                {
                    wallMovement.MoveUp();
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    wallMovement.MoveDown();
                }

                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    wallMovement.MoveRight();
                }

                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    wallMovement.MoveLeft();
                }
            }
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
            this.GetComponent<Rigidbody>().useGravity = true;
            collidedWithWall = false;
        }
    }
}
