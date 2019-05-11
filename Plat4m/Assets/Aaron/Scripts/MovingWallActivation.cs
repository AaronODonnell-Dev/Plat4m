using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWallActivation : MonoBehaviour
{
    public RaycastHit hit;
    private bool activateLever = false;
    public GameObject Player;
    public GameObject movingWall;
    public GameObject instructionMessage;

    public Animation move;

    private void Start()
    {
        move.GetComponent<Animation>();
        move["SideMovingPlatform"].speed = 0.15f;
    }

    void Update()
    {
        var heading = Player.transform.position - this.transform.position;

        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.

        if (heading.sqrMagnitude < 7 * 7)
        {
            instructionMessage.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E))
            {
                activateLever = !activateLever;
                instructionMessage.SetActive(false);
            }
        }

        EnableAnimation();
    }

    void EnableAnimation()
    {
        if (activateLever)
        {
            move.Play("SideMovingPlatform");
            
        }
        else if (!activateLever)
        {
            move.Stop("SideMovingPlatform");
        }
    }
}
