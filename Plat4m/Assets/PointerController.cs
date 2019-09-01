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

    private float[] pointerPositions = { 1.04f, 0.28f, -0.36f };

    public string level;

    public AudioClip click;

    private AudioSource source;
    private AudioSource backGroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        xPosition = transform.position.x;
        yPosition = transform.position.y;
        zPosition = transform.position.z;

        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(index)
        {
            case 0:
                yPosition = pointerPositions[1];
                transform.position = new Vector3(xPosition, yPosition, zPosition);
                break;

            case 1:
                yPosition = pointerPositions[2];
                transform.position = new Vector3(xPosition, yPosition, zPosition);
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
