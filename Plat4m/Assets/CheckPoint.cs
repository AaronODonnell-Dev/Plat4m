using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : GameController
{
	void Start()
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		// if the player collides with the Checkpoint
		// call the save function of the game controller
		if (other.tag == "Player")
		{
			Save();
		}
	}
}
