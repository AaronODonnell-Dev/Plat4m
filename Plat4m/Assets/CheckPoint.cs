using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log(Test());
		Debug.Log(Test1());
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	int Test()
	{
		
		return 100/ 200;
	}

	float Test1()
	{

		return 100f / 200f;
	}
	
}
