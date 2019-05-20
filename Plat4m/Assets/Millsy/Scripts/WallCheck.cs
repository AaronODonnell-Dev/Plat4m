using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    RaycastHit hit;
    Vector3 ForwardOffset;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // used for debugging the cast.
        RaycatsFromFeet();
    }

    public void RaycatsFromFeet()
    {
        // gets the transform. positon and takes away an offset of the objects local scale,
        // this is used to draw the Ray from the feet of the player character.
        // the if else statement then draws the line from the feet but is onle used if
        // the raycast hits within the distance, the sidtance is curently set to Mathf.Infinate
        // which is a infanite line. the draw method uses information from the raycats hit to
        // only draw for the distance that the raycast travels before contact.
        ForwardOffset = transform.position + new Vector3(0, 0, transform.localScale.z - 0.5f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.distance < 2)
            {
                Debug.DrawLine(ForwardOffset,
                    hit.point, Color.red);
            }
            else if (hit.distance > 2 && hit.distance < 5)
            {
                Debug.DrawLine(ForwardOffset,
                    hit.point, Color.blue);
            }
        }
    }
}
