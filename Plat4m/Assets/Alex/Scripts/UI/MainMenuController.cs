using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : UIController
{
    Canvas mMCCanvas;
    void Start()
    {
        mMCCanvas = mainMenu;
    }
    void Update()
    {
       
    }

    public void Start_OnClick()
    {
        mMCCanvas.enabled = false;        
    }

    public void Load_OnClick()
    {

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
