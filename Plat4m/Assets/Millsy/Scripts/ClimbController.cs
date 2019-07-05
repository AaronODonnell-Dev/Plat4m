using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    bool canClimb = false;
    [Range(0.001f, 1)]
    public float ClimbSpeed;
    public float RotateOnWallSpeed;
    WallCheck check;
    GameObject helper;
    bool hasRotatedOnWall = false;
    public bool ParentOrNot = false;

    public float WallJumps = 1;
    [Range(10,100)]
    public int WallJumpForce = 10;
    bool HasWallJumped;
    float Remounttime = 0.3f;
    bool WallJumpCorutine;
    public float WallMountTimer = 2f;

    void Start()
    {
        check = GetComponent<WallCheck>();
        helper = new GameObject();
        helper.name = "HelperObject";
    }


    #region--Updates--
    void Update()
    {
        // the constant test on the bool inside the WallCheck.
        canClimb = check.WithinClimbingRange;

        Dismount();
        GroundDetection();

        // statement to makse sure nothing happens unless can climb is true.
        if (canClimb && (check.hitFront.collider.tag == "Climbable" || check.hitFront.collider.tag == "MovingClimbable"))
        {
            helper.transform.rotation = Quaternion.LookRotation(-check.hitFront.normal);
            Quaternion FallRotation = new Quaternion(0, helper.transform.rotation.y, 0, 0);

            WallJumps = 1;

            HitDetectionLerpToPoint();
            WallMovement();
            check.MinDistance = 1;
            WallJump();
            RemountTimer();
            GroundDetection();

            if (GetComponent<Rigidbody>().isKinematic == false && HasWallJumped == false)
            {
                KinimaticSwitch();
            }

            if (!check.wallToRight && !check.WallToLeft)
            {
                if (transform.rotation != helper.transform.rotation)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, helper.transform.rotation, RotateOnWallSpeed);
                    hasRotatedOnWall = true;
                }
            }
            else if (check.WallToLeft)
            {
                MovingAroundAngleWalls(check.hitLeft);
            }
            else if (check.wallToRight)
            {
                MovingAroundAngleWalls(check.hitRight);
            }
            
        }
        else if (GetComponent<Rigidbody>().isKinematic == true && canClimb == false)
        {
            KinimaticSwitch();
            check.MinDistance = 0.7f;
        }

        if (WallJumpCorutine)
        {
            StartCoroutine(RotateOffWalJump(Vector3.up * 180, 0.3f));
            WallJumpCorutine = false;
        }



        //else if (canClimb == false && hasRotatedOnWall == true)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, 0, 1), RotateOnWallSpeed * 5);
        //    if (transform.rotation == new Quaternion(0, 0, 0, 1))
        //    {
        //        hasRotatedOnWall = false;
        //    }
        //}

       
    }
    #endregion

    void WallJump()
    {
        if (canClimb == true && WallJumps > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            HasWallJumped = true;
            WallJumpCorutine = true;
            KinimaticSwitch();
            GetComponent<Rigidbody>().AddForce((-transform.forward + Vector3.up) * WallJumpForce, ForceMode.VelocityChange);
            WallJumps--;
        }
    }



    // for testing
    IEnumerator RotateOffWalJump(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles - new Vector3(transform.eulerAngles.x,0,transform.eulerAngles.z));
        for (var t = 0f; t <= 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
        transform.rotation = toAngle;
    }

    void GroundDetection()
    {
        if (check.GroundCheck == true)
        {
            GetComponent<Player>().isGrounded = true;
            GetComponent<Player>().jumpLimit = 2;
            WallJumps = 1;
            if (transform.rotation != Quaternion.Euler(0,transform.eulerAngles.y,0))
            {
                StartCoroutine(RotateOffWalJump(new Vector3(0, transform.eulerAngles.y, 0), 0.1f));
            }
            
        }
        else
        {
            GetComponent<Player>().isGrounded = false;
        }
    }

    void Dismount()
    {
        if (check.GroundCheck == true && check.WithinClimbingRange == true)
        {
            canClimb = false;
        }
    }

    void RemountTimer()
    {
        if (HasWallJumped == true)
        {
            canClimb = false;
            Remounttime -= Time.deltaTime;
        }
        

        if (Remounttime  <= 0)
        {
            HasWallJumped = false;
            Remounttime = 0.3f;
        }
    }

    //turning kinimatic so gravity is no longer applyed
    void HitDetectionLerpToPoint()
    {
        // test to makse sure component works.
        //Debug.Log(GetComponent<WallCheck>().hit.point);

        if (check.hitFront.collider != null && check.hitFront.collider.CompareTag("Climbable"))
        {
            Vector3 positionToClingTo = check.hitFront.point;
            // lerp to position half the fowar direction
            // change later based on character size
            transform.position = Vector3.Lerp(
                transform.position,
                positionToClingTo - (transform.forward / 2),
                .05f);
        }

        if (check.hitFront.collider != null && check.hitFront.collider.CompareTag("MovingClimbable"))
        {
            Vector3 positionToClingTo = check.hitFront.point;

            // Two Choices. the parenting works but needs a condition added to it to make sure that once the
            // front cast is not longer touching it then it will not be parented
            if (ParentOrNot)
            {
                transform.SetParent(check.hitFront.transform);
            }
            else
            {
                transform.position = Vector3.Lerp(
                transform.position,
                positionToClingTo - (transform.forward / 2),
                1f);
            }
        }
    }

    void KinimaticSwitch()
    {
        GetComponent<Rigidbody>().isKinematic = !GetComponent<Rigidbody>().isKinematic;
    }

    void WallMovement()
    {

        if ((Input.GetAxisRaw("Vertical") > 0.1 && Input.GetAxisRaw("Vertical") != 0) || (Input.GetAxisRaw("Vertical") < 0.1 && Input.GetAxisRaw("Vertical") != 0))
        {
            Vector3 moveVert = transform.up * (Input.GetAxisRaw("Vertical") * ClimbSpeed);
            transform.position += moveVert;
        }
        if ((Input.GetAxisRaw("Horizontal") > 0.1 && Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Horizontal") < 0.1 && Input.GetAxisRaw("Horizontal") != 0))
        {
            // come back to. needs rotation set up first.
            //transform.localPosition += new Vector3((Input.GetAxisRaw("Horizontal") * ClimbSpeed), 0, 0);
            Vector3 moveHoz = transform.right * (Input.GetAxisRaw("Horizontal") * ClimbSpeed);
            transform.position += moveHoz;
        }
    }

    void MovingAroundAngleWalls(RaycastHit hit)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(-hit.normal) , .1f);
    }
}
