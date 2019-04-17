using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorActivation : MonoBehaviour
{
    public RaycastHit hit;
    private bool activateLever = false;
    public GameObject Player;
    public GameObject elevator;

    public Animation move;

    private void Start()
    {
        move.GetComponent<Animation>();
        move["Elevator"].speed = 0.15f;
    }

    void Update()
    {
        var heading = Player.transform.position - this.transform.position;

        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.

        if (heading.sqrMagnitude < 7 * 7 && Input.GetKeyDown(KeyCode.E))
        {
            activateLever = !activateLever;
        }

        EnableAnimation();
    }

    void EnableAnimation()
    {
        if (activateLever)
        {
            move.Play("Elevator");
            
        }
        else if (!activateLever)
        {
            move.Stop("Elevator");
        }
    }
}
