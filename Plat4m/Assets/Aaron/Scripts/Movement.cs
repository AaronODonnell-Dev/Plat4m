using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Range(1, 100)]
    public float force = 5;
    float angle;
    int count = 0;

    Player player;
    Rigidbody _currentBody;

    GameObject Instructions;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Player>();
        Instructions = GameObject.FindGameObjectWithTag("InstructionCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        #region count for instructions
        if (count == 180)
        {
            Instructions.SetActive(false);
            count = 0;
        }
        else count++;
        #endregion

        _currentBody = player.currentBody;

        #region - Player Movement calls & Jump-

        if (Input.GetAxisRaw("Horizontal") == 1 && player.isGrounded)
        {
            MoveRight();
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 && player.isGrounded)
        {
            MoveLeft();
        }
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            MoveFoward();
        }
        else if (Input.GetAxisRaw("Vertical") == -1 && player.isGrounded)
        {
            MoveBackWard();
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.jumpLimit > 0 && player.isGrounded)
        {
            player.Jump();
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.P))
        {
            player.PlayerSwitch();
        }
    }

    #region-Movement Methods-

    void MoveFoward()
    {
        _currentBody.AddForce(_currentBody.transform.forward * force, ForceMode.Force);
        //for rotating the player. slerp is slower than lerp
        //transform.rotation = mainCamera.transform.rotation;
    }

    void MoveBackWard()
    {
        _currentBody.AddForce(-_currentBody.transform.forward * force, ForceMode.Force);
        // allows for the rotation but the parented camera rotates with the object and
        // causes a continious loop of rotationg!
        //transform.rotation = Quaternion.LookRotation(-mainCamera.transform.forward, transform.up);
    }

    void MoveLeft()
    {
        _currentBody.AddForce(-_currentBody.transform.right * force, ForceMode.Force);
    }

    void MoveRight()
    {
        _currentBody.AddForce(_currentBody.transform.right * force, ForceMode.Force);
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
