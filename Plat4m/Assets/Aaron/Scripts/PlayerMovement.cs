using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float force = 10;
    Vector3 jumpForce;
    float angle;
    public int jumpLimit = 2;
    public bool isJumping = false;
    public bool isGrounded = true;

    Rigidbody p1Body;
    Rigidbody _p2body;
    Rigidbody _currentBody;

    CollisionManager collisionManager;
    WallMovementController wallMovement;

    GameObject mainCamera;

    public GameObject cameraPosP1;
    public GameObject cameraPosP2;

    enum PlayerIndex { PLAYERONE, PLAYERTWO};
    PlayerIndex _current;

    // Use this for initialization
    void Start()
    {
        p1Body = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        _p2body = GameObject.FindGameObjectWithTag("Player2").GetComponent<Rigidbody>();
        _current = PlayerIndex.PLAYERONE;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        collisionManager = new CollisionManager();
        wallMovement = new WallMovementController();
        jumpForce = Camera.main.transform.up * force * 10;
        collisionManager.InstatiatePlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        #region - Player Movement calls & Jump-
        if(isGrounded)
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
            if (Input.GetKeyDown(KeyCode.Space) && _current == PlayerIndex.PLAYERONE && jumpLimit > 0)
            {
                Jump();
            }
        }
        if(collisionManager.collidedWithWall)
        {
            jumpLimit = 1;
            p1Body.isKinematic = true;
            Debug.Log(p1Body.isKinematic);
            p1Body.AddForce(-10 * p1Body.mass * p1Body.transform.up);
            p1Body.freezeRotation = true;
            ResetJump();

            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                //reference wall movement later
                wallMovement.MoveRight();
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                wallMovement.MoveLeft();
            }
            if (Input.GetAxisRaw("Vertical") == 1)
            {
                wallMovement.MoveUp();
            }
            else if (Input.GetAxisRaw("Vertical") == -1)
            {
                wallMovement.MoveDown();
            }
        }

        #endregion

        #region -Player logic for camera
        if (_current == PlayerIndex.PLAYERONE && Input.GetKeyDown(KeyCode.P))
        {
            _current = PlayerIndex.PLAYERTWO;
            mainCamera.transform.position = cameraPosP2.transform.position;
            mainCamera.transform.LookAt(_p2body.transform);
        }
        else if (_current == PlayerIndex.PLAYERTWO && Input.GetKeyDown(KeyCode.P))
        {
            _current = PlayerIndex.PLAYERONE;
            mainCamera.transform.position = cameraPosP1.transform.position;
            mainCamera.transform.LookAt(p1Body.transform);
        }
        #endregion

        PlayerSwitch();
        CameraRotate();
    }

    void PlayerSwitch()
    {       
        switch(_current)
        {
            case PlayerIndex.PLAYERONE:
                mainCamera.transform.SetParent(p1Body.transform);
                _currentBody = p1Body;
                break;

            case PlayerIndex.PLAYERTWO:
                mainCamera.transform.SetParent(_p2body.transform);
                _currentBody = _p2body;
                break;
        }
    }

    void CameraRotate()
    {
        Camera.main.transform.RotateAround(_currentBody.transform.position, Vector3.up, Input.GetAxis("Mouse X"));
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
        //isGrounded = false;
        p1Body.velocity = new Vector3(0, 15, 0);
        jumpLimit--;
    }

    void MoveFoward()
    {
        _currentBody.AddForce(Camera.main.transform.forward * force, ForceMode.Force);
    }

    void MoveBackWard()
    {
        _currentBody.AddForce(-Camera.main.transform.forward * force, ForceMode.Force);
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
