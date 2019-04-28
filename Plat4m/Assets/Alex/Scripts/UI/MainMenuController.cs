using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : UIController
{  
  
    void Update()
    {
       
    }

    public void Start_OnClick()
    {
        mainMenu.SetActive(false);
        Camera.main.transform.LookAt(player.transform);       
        Camera.main.transform.RotateAround(player.transform.position, Vector3.up, 180f);       
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
