using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isJumping = false;
    public bool isGrounded = true;

    Rigidbody p1Body;
    Rigidbody _p2body;
    public Rigidbody currentBody;

    CollisionManager collisionManager;
    MovingPlatform movingPlatform;

    public PlayerHealth playerHealth;
    public GameObject HealthCanvas;
    public GameObject lever;

    public float jumpLimit = 2;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        p1Body = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        _p2body = GameObject.FindGameObjectWithTag("Player2").GetComponent<Rigidbody>();
        currentBody = p1Body;

        collisionManager = GetComponent<CollisionManager>();
    }

    #region methods for Player Switching and Jumping (called from Movement Script)
    public void PlayerSwitch()
    {
        if (currentBody == p1Body)
        {
            currentBody = _p2body;
        }
        else if (currentBody == _p2body)
        {
            currentBody = p1Body;
        }
    }

    public void Jump()
    {
        isJumping = true;
        isGrounded = false;
        p1Body.velocity = new Vector3(p1Body.velocity.x, 0, p1Body.velocity.z) + new Vector3(0, 11, 0);
        jumpLimit--;
    }

    public void ResetJump()
    {
        jumpLimit = 2;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        count++;
        if(count == 100)
        {
            HealthCanvas.SetActive(true);
        }

        if(count == 250)
        {
            HealthCanvas.SetActive(false);
        }

        #region Moving Platform Activation
        var heading = this.transform.position - lever.transform.position;

        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.

        if (heading.sqrMagnitude < 7 * 7)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                movingPlatform.ChangeTarget();
            }
        }

        #endregion
    }

    private void OnCollisionEnter(Collision collider)
    {
        collisionManager.BasicCollision(collider);
    }

    private void OnCollisionExit(Collision collider)
    {
        collisionManager.OnCollisionEnd(collider);
    }
}
