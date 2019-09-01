using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PointerController : MonoBehaviour
{
    private int index;

    private float xPosition, yPosition, zPosition;

    private float multiplier = 2.0f;

    private float[] pointerPositions = { 0.63f, -0.05f };

    public string level;

    public AudioClip click;

    private AudioSource source;
    private AudioSource backGroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        xPosition = transform.position.x;
        yPosition = pointerPositions[0];
        zPosition = transform.position.z;


        source = GetComponent<AudioSource>();
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(index)
        {
            case 0:
                transform.position = new Vector3(xPosition, 65.47f, -85.35f);
                break;

            case 1:
                transform.position = new Vector3(xPosition, 64.9f, -85.89f);
                break;
        }

        //Inputs
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            source.PlayOneShot(click);
            index--;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            source.PlayOneShot(click);
            index++;
        }

        if (index >= 2)
        {
            index = 0;
        }
        else if(index < 0)
        {
            index = 1;
        }

        if (index == 0)
            if (Input.GetKeyUp(KeyCode.Return))
            {
                source.PlayOneShot(click);
                Initiate.Fade(level, Color.black, multiplier);
                backGroundMusic.Stop();
            }

        if (index == 1)
            if (Input.GetKeyUp(KeyCode.Return))
            {
                source.PlayOneShot(click);

                Application.Quit();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            }


    }
}
