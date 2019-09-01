using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    Player player;
    public Rigidbody playerBody;
    public GameObject SarcasticHealthResponse;
    RaycastHit Hit;
    Yeet yeet;

    int EnemiesHit = 0;

    public bool collidedWithWall = false;
    public bool collisionEnded = false;

    PlayerHealth playerHealth;

    void Start()
    {
        SarcasticHealthResponse.SetActive(false);
        yeet = GetComponent<Yeet>();
        player = GetComponent<Player>();
        playerBody = player.GetComponent<Rigidbody>();
        playerHealth = player.GetComponent<PlayerHealth>();
        Hit = yeet.hit;
        EnemiesHit = 0;
    }

    public void BasicCollision(Collision collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "MovingPlatform")
        {
            if (yeet.wasYeeted)
            {
                yeet.wasYeeted = false;
            }
    
            player.isGrounded = true;
            player.isJumping = false;
            player.ResetJump();
        }

        if (collision.transform.tag == "MovingPlatform")
        {
            playerBody.transform.parent = collision.transform;
        }

        if(collision.transform.tag == "CollidableObject")
        {
            Vector3 direction = collision.contacts[0].point - transform.position;
            direction = -direction.normalized;

            playerBody.AddForce(direction * 4);
        }

        if(collision.transform.tag == "EnemyKeg")
        {
            playerHealth.TakeDamage(10);
            Vector3 direction = collision.contacts[0].point - transform.position;
            direction = -direction.normalized;

            playerBody.AddForce(direction * 10 + Vector3.up);
            if (EnemiesHit == 0)
            {
                SarcasticHealthResponse.SetActive(true);
            }
            else SarcasticHealthResponse.SetActive(false);
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

        if(collision.transform.tag == "EnemyKeg")
        {
            EnemiesHit++;
        }
    }
}
