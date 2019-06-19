using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] camPositions;
    Transform currentCamPos;
    Transform player;
    UIController setCamPos;
    public float transistionSpeed = 10f;
    void Start()
    {
        camPositions[0] = GameObject.FindGameObjectWithTag("MenuCamPos").GetComponent<Transform>();
        camPositions[1] = GameObject.FindGameObjectWithTag("GameCamPos").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        setCamPos = gameObject.GetComponentInChildren(typeof(UIController), true) as UIController;
        Camera.main.transform.SetPositionAndRotation(camPositions[0].position, camPositions[0].rotation);
        currentCamPos = Camera.main.transform;
    }
    void Update()
    {

       // When the Pause Menu is on or the Main menu the the cameras position is in the Menu Cam Position 
        if (setCamPos.mainMenu.isActiveAndEnabled == true || setCamPos.pauseMenu.isActiveAndEnabled == true)
        {
            transistionSpeed = 10f;
            // Rotates slowly to the position
            transform.position = Vector3.Lerp(transform.position, camPositions[0].position, Time.deltaTime * transistionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, camPositions[0].rotation, Time.deltaTime * transistionSpeed);
            
        }
        // when the menus are not running
        else
        {
            transistionSpeed = 1f;
            transform.position = Vector3.Lerp(transform.position, camPositions[1].position, Time.deltaTime * transistionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, camPositions[1].rotation, Time.deltaTime * transistionSpeed);
            transform.LookAt(player.position - new Vector3(-1, -1, -1));

        }
    }
}
