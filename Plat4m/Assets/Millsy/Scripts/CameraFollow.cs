using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    public GameObject objectTwoFollow;
    GameObject[] camColiders;
    Transform objectsTransform;
    Transform objectsTransform2;

    Vector3 camOffset;
    Vector3 camOffset2;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;
    [Range(1.0f, 10.0f)]
    public float rotateSpeed = 5.0f;

    public bool lookAtObject = false;
    public bool rotateAroundObject = false;
    public bool inPosition = false;

    public GameObject POneCamPos;
    public GameObject PTwoCamPos;
    Vector3 newPos;

    enum player
    {
        PLAYER1,
        PLAYER2
    }
    player currentPlayer = player.PLAYER1;

    void Start()
    {
        objectsTransform = objectToFollow.transform;
        camOffset = transform.position - objectsTransform.position;
        camColiders = GameObject.FindGameObjectsWithTag("GameCamPos");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && currentPlayer == player.PLAYER1)
        {
            currentPlayer = player.PLAYER2;
            inPosition = false;
        }
        else if (Input.GetKeyDown(KeyCode.P) && currentPlayer == player.PLAYER2)
        {
            currentPlayer = player.PLAYER1;
            inPosition = false;
        }

        if (camColiders[0].GetComponent<SphereCollider>().bounds.Contains(transform.position))
        {
            camColiders[1].GetComponent<SphereCollider>().enabled = true;
            inPosition = true;
            camColiders[0].GetComponent<SphereCollider>().enabled = false;
        }
        else if (camColiders[1].GetComponent<SphereCollider>().bounds.Contains(transform.position))
        {
            camColiders[0].GetComponent<SphereCollider>().enabled = true;
            inPosition = true;
            camColiders[1].GetComponent<SphereCollider>().enabled = false;
        }
    }

    void LerpBetweenPoints(Vector3 moveTo, float speed)
    {
        transform.position = Vector3.Lerp(transform.position, moveTo, speed);
    }

    void LateUpdate()
    {

        if (currentPlayer == player.PLAYER1 && inPosition == false)
        {
            objectsTransform = objectToFollow.transform;
            camOffset = transform.position - objectsTransform.position;
            LerpBetweenPoints(POneCamPos.transform.position,Time.deltaTime * 2);
            transform.LookAt(objectsTransform);
        }
        else if (currentPlayer == player.PLAYER2 && inPosition == false)
        {
            objectsTransform = objectTwoFollow.transform;
            camOffset = transform.position - objectsTransform.position;
            LerpBetweenPoints(PTwoCamPos.transform.position, Time.deltaTime * 2);
            transform.LookAt(objectsTransform);
        }

        if (inPosition)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (rotateAroundObject)
                {
                    Quaternion camTurnAngle = Quaternion.AngleAxis(
                        Input.GetAxis("Mouse X") * rotateSpeed,
                        Vector3.up);
                    camOffset = camTurnAngle * camOffset;
                }
                float horz = Input.GetAxis("Mouse X") * rotateSpeed;
                objectsTransform.Rotate(0, horz, 0);
            }
            
            newPos = objectsTransform.position + camOffset;
            transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

            if (lookAtObject)
            {
                transform.LookAt(objectsTransform);
            }
        }
    }
}
