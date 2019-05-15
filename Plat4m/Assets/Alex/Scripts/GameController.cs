using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController controller;
    public int pipHealth;
    public float squeakHealth;

    // Start is called before the first frame update
    void Awake()
    {
        if (controller)
        {
            DontDestroyOnLoad(this.gameObject);
            controller = this;
        }
        else if(controller != this)
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
