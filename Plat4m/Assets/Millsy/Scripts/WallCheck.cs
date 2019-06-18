using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public RaycastHit hitFront;
    public RaycastHit hitRight;
    public RaycastHit hitLeft;
    public RaycastHit HitDown;

    // public trigger for debugging with the ray.
    public bool debugLine = true;

    // used for the min disance for the cast to be red and max for the ray to be blue
    // colour used for debugging
    [Range(0, 5)]
    public float MinDistance = 0.7f;
    [Range(5, 20)]
    public float MaxDistance = 10;

    Vector3 ForwardOffset;
    Vector3 RightOffset;

    // for external use in the climbing script
    public bool WithinClimbingRange = false;
    public bool wallToRight;
    public bool WallToLeft;
    public bool GroundCheck;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // used for debugging the cast.
        Raycast(transform.position, transform.forward, Color.red, Color.yellow, Color.blue,ref hitFront,ref WithinClimbingRange);

        // needs to be done so that grounded is set to true rather than climbing.
        Raycast(transform.position + -transform.up/2, -transform.up, Color.red, Color.yellow, Color.blue,ref hitRight,ref GroundCheck);


        Raycast(transform.position, transform.right, Color.red, Color.yellow, Color.blue,ref hitRight,ref wallToRight);
        Raycast(transform.position, -transform.right, Color.red, Color.yellow, Color.blue,ref hitLeft,ref WallToLeft);
    }

    public void Raycast(Vector3 StartOffset, Vector3 Direction,
        Color NearColor, Color MidColour, Color FarColor,
        ref RaycastHit hit,ref bool check)
    {
        // gets the transform. positon and takes away an offset of the objects local scale,
        // this is used to draw the Ray from the feet of the player character.
        // the if else statement then draws the line from the feet but is onle used if
        // the raycast hits within the distance, the sidtance is curently set to Mathf.Infinate
        // which is a infanite line. the draw method uses information from the raycats hit to
        // only draw for the distance that the raycast travels before contact.

        if (Physics.Raycast(StartOffset, Direction, out hit, Mathf.Infinity))
        {
            // Contact with wall is acheivable.
            if (hit.distance < MinDistance)
            {
                if (debugLine)
                {
                    Debug.DrawLine(StartOffset,
                    hit.point, NearColor);
                }
                check = true;
            }
            // close to objects
            else if (hit.distance > MinDistance && hit.distance < MaxDistance)
            {
                if (debugLine)
                {
                    Debug.DrawLine(StartOffset,
                    hit.point, MidColour);
                }
                check = false;
            }
            // objects far away
            else if (hit.distance > MaxDistance)
            {
                if (debugLine)
                {
                    Debug.DrawLine(StartOffset,
                    hit.point, FarColor);
                }
                check = false;
            }
        }
        else
        {
            // anything in hear will happen when the the ray hits nothing.
            check = false;
        }
    }
}