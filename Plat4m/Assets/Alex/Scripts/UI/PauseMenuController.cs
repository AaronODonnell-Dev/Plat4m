using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : UIController
{
    private void Start()    {
    }
    void Update()
    { }
    // When the Resume Button is Pressed
    public void Resume_OnClick()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    // When the Options Button is Pressed
    public void Options_OnClick()
    {
     
    }
    // When the Save Button is Pressed
    public void Save_OnClick()
    {
       // BinaryFormatter bf = new BinaryFormatter();
       // FileStream file = File.Open(Application.persistentDataPath + "/PlayerInfo.dat", FileMode.Open);


        //bf.Serialize(file,  );
    }
    // When the Quit Button is pressed
    public void Quit_OnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
