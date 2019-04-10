using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float force;
    float angle;

    Rigidbody _body;

    // Use this for initialization
    void Start()
    {
        _body = GetComponent<Rigidbody>();
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

        CameraRotate();
    }

    void CameraRotate()
    {
        Camera.main.transform.RotateAround(_body.transform.position, Vector3.up, Input.GetAxis("Mouse X"));
    }

    #region-Movement Methods-
    void MoveFoward()
    {
        _body.AddForce(Camera.main.transform.forward * force, ForceMode.Force);
    }

    void MoveBackWard()
    {
        _body.AddForce(-Camera.main.transform.forward * force, ForceMode.Force);
    }

    void MoveLeft()
    {
        _body.AddForce(-Camera.main.transform.right * force, ForceMode.Force);

    }

    void MoveRight()
    {
        _body.AddForce(Camera.main.transform.right * force, ForceMode.Force);
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
