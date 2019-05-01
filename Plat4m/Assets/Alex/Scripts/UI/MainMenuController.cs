using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : UIController
{

    public MainMenuController()
    {
    }
    void Start()
    {        
    }
    void Update()
    {       
    }
    public void Start_OnClick()
    {
        mainMenu.enabled = false;
    }
    public void Quit_OnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

}
