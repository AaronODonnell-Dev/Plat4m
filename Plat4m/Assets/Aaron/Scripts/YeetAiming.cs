using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script by Swati Patel Feb 2014

public class YeetAiming : MonoBehaviour
{
    // TrajectoryPoint and Ball will be instantiated
    public GameObject TrajectoryPointPrefeb;
    public GameObject Player1;

    public bool isYeeted;
    private float power = 25;
    private int numOfTrajectoryPoints = 30;
    private List<GameObject> trajectoryPoints;
    //---------------------------------------    
    public void StartYeet()
    {
        trajectoryPoints = new List<GameObject>();
        Player1 = GameObject.FindGameObjectWithTag("Player");
        TrajectoryPointPrefeb = GameObject.FindGameObjectWithTag("PredictiveDot");
        //isYeeted = false;
        //TrajectoryPoints are instantiated
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            TrajectoryPointPrefeb.GetComponent<Renderer>().enabled = false;
            trajectoryPoints.Insert(i, TrajectoryPointPrefeb);
        }

        YeetAim();
    }
    //---------------------------------------    
    public void YeetAim()
    {
        Debug.Log(isYeeted);

        // when mouse button is pressed, cannon is rotated as per mouse movement and projectile trajectory path is displayed.
        if (isYeeted)
        {
            Debug.Log("Was Yeeted");
            Vector3 vel = GetForceFrom(Player1.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            Player1.transform.eulerAngles = new Vector3(0, 0, angle);
            setTrajectoryPoints(transform.position, vel / Player1.GetComponent<Rigidbody>().mass);
        }
    }
    //---------------------------------------    
    // Following method returns force by calculating distance between given two points
    //---------------------------------------    
    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * power;
    }
    //---------------------------------------    
    // Following method displays projectile trajectory path. It takes two arguments, start position of object(ball) and initial velocity of object(ball).
    //---------------------------------------    
    void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        Debug.Log("Setting traj points");
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.1f;
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 2);
            trajectoryPoints[i].transform.position = pos;
            trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
            trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += 0.1f;
        }
    }
}
