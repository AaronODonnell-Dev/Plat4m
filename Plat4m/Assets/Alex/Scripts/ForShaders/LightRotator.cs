using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotator : MonoBehaviour
{

    public GameObject PointLight;
    public GameObject Sphere;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PointLight.transform.RotateAround(Sphere.transform.position, new Vector3(0, 1, 0), 1); 
    }
}
