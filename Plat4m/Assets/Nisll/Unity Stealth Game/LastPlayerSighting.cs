using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerSighting : MonoBehaviour
{
    public Vector3 position = new Vector3(1000f, 1000f, 1000f);
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);
    public float lightHighIntensity = 0.25f;
    public float lightLowIntensity = 0f;
    public float fadeSpeed = 7f;
    public float musicFadeSpeed = 1f;

    //private Alarmlight alarm;
    private Light mainLight;
    private AudioSource panicAudio;
    private AudioSource[] sirens;

    private void Awake()
    {
        //alarm = GameObjectWithTag("alarm").GetComponent<AlarmLight>();
        //mainLight = GameObject.FindGameObjectWithTag("mainLight").light; //Use GetComponent<Light>();
        //panicAudio = transform.Find("SecondaryMusic").audio;
        //GameObject[] sirenGameObjects = GameObject.FindGameObjectWithTag("siren");
        //sirens = new AudioSource[sirenGameObject.Lenght];

        //for (int i = 0; i < sirens.Length; i++)
        //{
        //    sirens[i] = sirenGameObjects[i].audio;
        //}
    }

    private void Update()
    {
        //SwitchAlarms();
        //MusicFading();
    }

    void SwitchAlarms()
    {
        //alarm.alarmOn = (position != resetPosition);

        float newIntensity;

        if (position != resetPosition)
        {
            newIntensity = lightLowIntensity;
        }
        else
        {
            newIntensity = lightHighIntensity;
        }

        mainLight.intensity = Mathf.Lerp(mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);

        for (int i = 0; i < sirens.Length; i++)
        {
            if (position != resetPosition && !sirens[i].isPlaying)
            {
                sirens[i].Play();
            }
            else if (position == resetPosition)
            {
                sirens[i].Stop();
            }
        }
    }

    void MusicFading()
    {
        if (position != resetPosition)
        {
            //audio.volume = Math.Lerp(audio.volume, 0f, musicFadeSpeed * Time.deltaTime);
            //panicAudio.volume = Math.Lerp(panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
        }
        else
        {
            //audio.volume = Math.Lerp(audio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
            //panicAudio.volume = Math.Lerp(panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
        }
    }
}
