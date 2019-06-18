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

        // statement to makse sure nothing happens unless can climb is true.
        if (canClimb && (check.hitFront.collider.tag == "Climbable" || check.hitFront.collider.tag == "MovingClimbable"))
        {
            HitDetectionLerpToPoint();
            WallMovement();
            check.MinDistance = 1;

            if (GetComponent<Rigidbody>().isKinematic == false)
            {
                KinimaticSwitch();
            }


            helper.transform.rotation = Quaternion.LookRotation(-check.hitFront.normal);

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
        else if (canClimb == false && hasRotatedOnWall == true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, 0, 1), RotateOnWallSpeed * 5);
            if (transform.rotation == new Quaternion(0, 0, 0, 1))
            {
                hasRotatedOnWall = false;
            }
        }

       
    }
    #endregion

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
