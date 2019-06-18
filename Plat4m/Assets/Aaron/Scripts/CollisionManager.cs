using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    PlayerMovement Player;
    Rigidbody playerBody;
    RaycastHit Hit;
    Yeet yeet;

    public bool collidedWithWall = false;
    public bool collisionEnded = false;

    void Start()
    {
        yeet = new Yeet();
        Hit = yeet.hit;
    }

    public void InstatiatePlayer(PlayerMovement player)
    {
        Player = player;
        playerBody = player.GetComponent<Rigidbody>();
    }

    public void OnCollisionWithWall(Collision collision)
    {
        if (collision.transform.tag == "MovingWall")
        {
            collidedWithWall = true;
            //Debug.Log(collidedWithWall);
        }
    }

    public void PredictiveCollision()
    {
        if(Hit.distance <= 0.2)
        {
            playerBody.transform.Translate(new Vector3(0, -0.2f, 0));
        }
    }

    public void BasicCollision(Collision collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "MovingPlatform")
        {
            yeet.wasYeeted = false;
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

    public void OnCollisionEnd(Collision collision)
    {
        if (collision.transform.tag == "MovingPlatform" || collision.transform.tag == "Ground")
        {
            playerBody.transform.parent = null;
            //Player.isGrounded = false;
        }

        if (collision.transform.tag == "MovingWall")
        {
            collisionEnded = true;
            //playerBody.freezeRotation = true;
            //playerBody.useGravity = true;
            //collidedWithWall = false;
        }
    }
}
