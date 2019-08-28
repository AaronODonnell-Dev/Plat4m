using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Range(1, 100)]
    public float force = 35;
    float angle;
    int count = 0;
    float slopeRayHeight;

    public GameObject Ground;
    Player player;
    Rigidbody _currentBody;

    WallCheck wallCheck;

    GameObject Instructions;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Player>();
        Instructions = GameObject.FindGameObjectWithTag("InstructionCanvas");

        wallCheck = GetComponent<WallCheck>();

        //slopeRayHeight = 
    }

    bool checkMoveableTerrain(Vector3 position, Vector3 desiredDirection, float distance)
    {
        float steepSlopeAngle = 40f;
        float slopeThreshold = 0.01f;
        Ray myRay = new Ray(position, desiredDirection); // cast a Ray from the position of our gameObject into our desired direction. Add the slopeRayHeight to the Y parameter

        if (Physics.Raycast(myRay, out wallCheck.HitDown, distance))
        {
            if (wallCheck.HitDown.collider.gameObject.tag == "Ground") // Our Ray has hit the ground
            {
                float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, wallCheck.HitDown.normal); // Here we get the angle between the Up Vector and the normal of the wall we are checking against: 90 for straight up walls, 0 for flat ground.

                float radius = Mathf.Abs(slopeRayHeight / Mathf.Sin(slopeAngle)); // slopeRayHeight is the Y offset from the ground you wish to cast your ray from.

                if (slopeAngle >= steepSlopeAngle * Mathf.Deg2Rad) //You can set "steepSlopeAngle" to any angle you wish.
                {
                    if (wallCheck.HitDown.distance - GetComponent<CapsuleCollider>().radius > Mathf.Abs(Mathf.Cos(slopeAngle) * radius) + slopeThreshold) // Magical Cosine. This is how we find out how near we are to the slope / if we are standing on the slope. as we are casting from the center of the collider we have to remove the collider radius.
                                                                                                                     // The slopeThreshold helps kills some bugs. ( e.g. cosine being 0 at 90° walls) 0.01 was a good number for me here
                    {
                        return true; // return true if we are still far away from the slope
                    }

                    return false; // return false if we are very near / on the slope && the slope is steep
                }

                return true; // return true if the slope is not steep
            }

            return false;
        }

        return false;
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

        //if (checkMoveableTerrain(player.position, new Vector3(desiredVelocity.x, 0, desiredVelocity.z), 10f)) // filter the y out, so it only checks forward... could get messy with the cosine otherwise.
        //{
        //    rigidbody.velocity = desiredVelocity;
        //}

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

        if (Input.GetKeyDown(KeyCode.Space) && player.jumpLimit > 0)
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
