﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float force = 10;
    float angle;

    Rigidbody _p1body;
    Rigidbody _p2body;
    Rigidbody _currentBody;
    GameObject mainCamera;

    public GameObject cameraPosP1;
    public GameObject cameraPosP2;

    enum PlayerIndex { PLAYERONE, PLAYERTWO};
    PlayerIndex _current;

    // Use this for initialization
    void Start()
    {
        _p1body = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        _p2body = GameObject.FindGameObjectWithTag("Player2").GetComponent<Rigidbody>();
        _current = PlayerIndex.PLAYERONE;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
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

        if(_current == PlayerIndex.PLAYERONE && Input.GetKeyDown(KeyCode.P))
        {
            _current = PlayerIndex.PLAYERTWO;
            mainCamera.transform.position = cameraPosP2.transform.position;
            mainCamera.transform.LookAt(_p2body.transform);
        }
        else if (_current == PlayerIndex.PLAYERTWO && Input.GetKeyDown(KeyCode.P))
        {
            _current = PlayerIndex.PLAYERONE;
            mainCamera.transform.position = cameraPosP1.transform.position;
            mainCamera.transform.LookAt(_p1body.transform);
        }

        PlayerSwitch();
        CameraRotate();
    }

    void PlayerSwitch()
    {       
        switch(_current)
        {
            case PlayerIndex.PLAYERONE:
                mainCamera.transform.SetParent(_p1body.transform);
                _currentBody = _p1body;
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

    #region-Movement Methods-
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
