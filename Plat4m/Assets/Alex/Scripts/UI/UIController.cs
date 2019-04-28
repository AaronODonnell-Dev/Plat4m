using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject pauseMenu, mainMenu, player;
    public static int pipLives = 9;
    public static int collectable;
    public static int score;
    bool togglePause = false;
    Vector3 menuCamPos;
    // Start is called before the first frame update
    void Start()
    {
        menuCamPos = new Vector3(1f, 2f, -3.5f);            
        player = GameObject.FindGameObjectWithTag("Player");       
        mainMenu.SetActive(true);
        pauseMenu.SetActive(togglePause);
        Camera.main.transform.position = player.transform.position + menuCamPos;  

    }

    // Update is called once per frame
    void Update()
    {
        if (mainMenu.activeInHierarchy == false)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                togglePause = !togglePause;
                pauseMenu.SetActive(togglePause);                
                Time.timeScale = 0;
            }
            if (!togglePause)
            {
                Time.timeScale = 1;
            }
        }
    }
}
