using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject player;
    public Canvas pauseMenu, mainMenu; 
    public static int pipLives = 9;
    public static int collectable;
    public static int score;
    public static bool togglePause = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainMenu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        mainMenu.enabled = true;
        pauseMenu.enabled = false;       
    }

    // Update is called once per frame
    void Update()
    {    
        if (mainMenu.isActiveAndEnabled == false)
        {           
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                pauseMenu.enabled = true;
                togglePause = !togglePause;                               
                Time.timeScale = 0;
            }
            if (!togglePause)
            {
                pauseMenu.enabled = false;
                Time.timeScale = 1;
            }
        }
    }
}
