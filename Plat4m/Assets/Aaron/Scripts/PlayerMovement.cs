using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1,100)]
    public float force = 10;
    float angle;
    int count = 0;
    public int jumpLimit = 2;
    public bool isJumping = false;
    public bool isGrounded;

    Rigidbody p1Body;
    Rigidbody _p2body;
    Rigidbody _currentBody;

    CollisionManager collisionManager;

    GameObject Instructions;

    public GameObject cameraPosP1;
    public GameObject cameraPosP2;

    // Use this for initialization
    void Start()
    {
        p1Body = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        _p2body = GameObject.FindGameObjectWithTag("Player2").GetComponent<Rigidbody>();
        _currentBody = p1Body;
        //Instructions = GameObject.FindGameObjectWithTag("InstructionCanvas");
        collisionManager = new CollisionManager();
        collisionManager.InstatiatePlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 180)
        {
            //Instructions.SetActive(false);
            count = 0;
        }
        else count++;

        #region - Player Movement calls & Jump-

        if (isGrounded)
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                MoveRight();
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                MoveLeft();
            }
            if (Input.GetAxisRaw("Vertical") == 1)
            {
                MoveFoward();
            }
            else if (Input.GetAxisRaw("Vertical") == -1)
            {
                MoveBackWard();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && _currentBody == p1Body && jumpLimit > 0)
        {
            Jump();
        }

        #endregion
        if(Input.GetKeyDown(KeyCode.P))
        {
            PlayerSwitch();
        }
    }



    void PlayerSwitch()
    {
        if(_currentBody == p1Body)
        {
                _currentBody = _p2body;
        }
        else if(_currentBody == _p2body)
        {
            _currentBody = p1Body;
        }
        else if(_currentBody == _p2body)
        {
            _currentBody = p1Body;
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        collisionManager.OnCollisionWithWall(collider);
        collisionManager.BasicCollision(collider);
    }

    private void OnCollisionExit(Collision collider)
    {
        collisionManager.OnCollisionEnd(collider);
    }

    public void ResetJump()
    {
        jumpLimit = 2;
    }

    #region-Movement Methods-
    void Jump()
    {
        isJumping = true;
        isGrounded = false;
        p1Body.velocity = new Vector3(p1Body.velocity.x,0,p1Body.velocity.z) + new Vector3(0, 12, 0);
        jumpLimit--;
    }

    void MoveFoward()
    {
        _currentBody.AddForce(Camera.main.transform.forward * force, ForceMode.Force);
        //for rotating the plapyer. slerp is slower than lerp
        //transform.rotation = mainCamera.transform.rotation;
    }

    void MoveBackWard()
    {
        _currentBody.AddForce(-Camera.main.transform.forward * force, ForceMode.Force);
        // allows for the rotation but the parented camera rotates with the object and
        // causes a continious loop of rotationg!
        //transform.rotation = Quaternion.LookRotation(-mainCamera.transform.forward, transform.up);
    }

    void MoveLeft()
    {
        _currentBody.AddForce(-Camera.main.transform.right * force, ForceMode.Force);
    }

    void MoveRight()
    {
        _currentBody.AddForce(Camera.main.transform.right * force, ForceMode.Force);
    }
    #endregion

    #region-Look At Where RIght Analog Stick is facing Method-
    void Aim()
    {

        angle = Mathf.Atan2(Input.GetAxis("Right Stick X"), Input.GetAxis("Right Stick Y")) * Mathf.Rad2Deg;

        if (transform.rotation != Quaternion.Euler(new Vector3(0, angle, 0)))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }

    }
    #endregion

}
