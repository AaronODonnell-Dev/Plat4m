using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeet : MonoBehaviour
{
    Vector3 direction;
    public GameObject Player2;
    PlayerMovement playerMovement;
    public Transform arrow;
    float YeetForce = 17;
    Animation arrowMove;
    public bool isYeeting;

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
        GetComponent<Rigidbody>().AddForce(-direction * YeetForce * 85, ForceMode.Force);

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
        direction = - Vector3.up;

        float rot_z = 180;

        arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    //void OnDrawGizmos()
    //{

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 2f);

    //}
}
