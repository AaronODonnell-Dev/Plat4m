using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeet : MonoBehaviour
{
    public GameObject Player2;
    public GameObject arrow;
    public bool isYeeting;
    public GameObject ArrowTip;

    float YeetForce = 17;
    Animation arrowMove;
    PlayerMovement playerMovement;
    Vector3 direction;

    // Use this for initialization
    void Start()
    {
        playerMovement = new PlayerMovement();
        isYeeting = false;
        arrow.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            possibleYeets();
        }
        else if(Input.GetKeyUp(KeyCode.Y) && isYeeting)
        {
            Throw();
        }
        
    }

    public void Throw()
    {
        GetComponent<Rigidbody>().AddForce(- direction * YeetForce * 100, ForceMode.Force);

        isYeeting = false;
        arrow.gameObject.SetActive(false);

        Time.timeScale = 1;
    }

    public void possibleYeets()
    {
        if (!isYeeting)
        {
            var heading = transform.position - Player2.transform.position;

            var distance = heading.magnitude;
            var direction = heading / distance; // This is now the normalized direction.

            if (heading.sqrMagnitude < 3 * 3)
            {
                isYeeting = true;
                arrow.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else CalculateDirection();
    }

    public void CalculateDirection()
    {
        direction = arrow.transform.up;

        float rot_z = Input.GetAxis("Mouse X") * 2;

        arrow.transform.RotateAround(ArrowTip.transform.position, new Vector3(0,0,1), - rot_z);
    }
}
