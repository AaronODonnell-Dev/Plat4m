using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeet : MonoBehaviour
{
    Vector3 direction;
    public GameObject Player2;
    PlayerMovement playerMovement;
    public float speed = 20f;
    public float maxtime = 1f;
    public Transform arrow;
    public GameObject effect;
    float YeetForce;
    Animation arrowMove;
    public bool wasYeeted;
    public bool canYeet;

    // Use this for initialization
    void Start()
    {
        playerMovement = new PlayerMovement();
        arrowMove = arrow.GetComponent<Animation>();
        arrowMove["YeetArrow"].speed = 0.15f;
        arrow.gameObject.SetActive(false);

        YeetForce = 100 * 17;
    }


    IEnumerator Counter()
    {
        //pauses time 
        float stopTime = Time.realtimeSinceStartup + maxtime;

        while (Time.realtimeSinceStartup < stopTime)
        {
            yield return null;
        }

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
            canYeet = false;
            arrow.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        var heading = transform.position - Player2.transform.position;

        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.

        StartCoroutine("Counter");

        Time.timeScale = 0;

        canYeet = true;
        arrow.position = transform.position + (Camera.main.transform.forward * 2);
        //arrow.Translate(0, 0, 10);

        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        arrowMove.cullingType = AnimationCullingType.BasedOnRenderers;
        //Instantiate(effect, transform.position, Quaternion.identity);

        if (heading.sqrMagnitude < 3 * 3)
        {
            arrow.gameObject.SetActive(true);

            if(Input.GetKeyDown(KeyCode.Y))
            {
                transform.GetComponent<Rigidbody>().AddForce(-arrow.transform.forward + Vector3.up * YeetForce, ForceMode.Force);
                arrow.gameObject.SetActive(false);
                playerMovement.jumpLimit--;
                wasYeeted = true;
            }
        }
            //break;
    }

    //void OnDrawGizmos()
    //{

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 2f);

    //}
}
