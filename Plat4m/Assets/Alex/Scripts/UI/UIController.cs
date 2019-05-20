using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : GameController
{	
	GameObject player;
    public Canvas pauseMenu, mainMenu;    
    public static bool togglePause;
 
    
    // Start is called before the first frame update
    public UIController()	{}

    void Start()
    {
	
        player = GameObject.FindGameObjectWithTag("Player");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<Canvas>();
        mainMenu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        mainMenu.enabled = true;
        pauseMenu.enabled = false;
        togglePause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainMenu.isActiveAndEnabled == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                togglePause = !togglePause;
                pauseMenu.enabled  = togglePause;                               
            }
            if (pauseMenu.isActiveAndEnabled == true)
            {
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = Time.timeScale * Time.deltaTime;
            }
            else if(pauseMenu.isActiveAndEnabled == false)
            {
                Time.timeScale = 1;
                togglePause = false;
                Time.fixedDeltaTime = Time.deltaTime;
            }
        }

    }
}
