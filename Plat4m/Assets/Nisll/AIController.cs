using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Nodes))]
public class AIController : MonoBehaviour
{
    public GameObject Target;
    public GameObject ObjectToMoveTo;
    public GameObject FieldOfView;
    private Nodes nodes;

    //GameObject obj;
    public Collider objCollider;

    public Camera cam;
    Plane[] planes;

    void Start()
    {
        nodes = GetComponent<Nodes>();

        //cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        objCollider = GetComponent<Collider>();
    }

    bool canMove = true;
    void Update()
    {

        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {
            Debug.Log(Target.name + " has been detected!");
        }
        else
        {
            Debug.Log("Nothing has been detected");
        }

        if (canMove)
        {
            nodes.MoveTo(ObjectToMoveTo);
        }
    }

    //private void SeePlayer()
    //{
    //    if (FieldOfView)
    //    {

    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            nodes.Stop();
            canMove = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canMove = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Node")
        {
            var node = other.gameObject.GetComponent<PathNode>();

            if (node.NextNode != null)
                ObjectToMoveTo = node.NextNode.gameObject;
            else
                canMove = false;
        }
    }

    //private void SeePlayer(Collider player)
    //{
    //    if (player.gameObject.tag == "Player")
    //    {
    //        var node = player.gameObject.GetComponent<PathNode>();
    //    }
    //}

    /*
     
     using UnityEngine;
    using System.Collections;

    public class ExampleScript : MonoBehaviour {
    public Camera camera;

    void Start(){
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
            
            // Do something with the object that was hit by the raycast.
        }
    }
}
     
     
     */
}
