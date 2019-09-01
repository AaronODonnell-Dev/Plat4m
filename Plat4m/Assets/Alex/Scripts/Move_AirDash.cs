using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_AirDash : PlayerMovement
{
	GameObject airdashPlayer;
	Rigidbody  airdashP1Body;
	Transform  airdashP1Trans;
	// Start is called before the first frame update
	void Start()
    {
		airdashPlayer = GameObject.FindGameObjectWithTag("Player");
		airdashP1Body = airdashPlayer.GetComponent<Rigidbody>();
		
	}

    // Update is called once per frame
    void Update()
    {
		airdashP1Trans = airdashPlayer.GetComponent<Transform>();
		if (Input.GetKeyDown(KeyCode.Q))
		{
			airdashP1Body.AddForce(airdashP1Trans.forward * 10, ForceMode.VelocityChange);
			
		}
    }
}
