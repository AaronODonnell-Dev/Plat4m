using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Nodes))]
public class AIController : MonoBehaviour
{
    public GameObject ObjectToMoveTo;
    public GameObject FieldOfView;
    private Nodes nodes;

    void Start()
    {
        nodes = GetComponent<Nodes>();
    }

    bool canMove = true;
    void Update()
    {
        if (canMove)
            nodes.MoveTo(ObjectToMoveTo);

    }

    private void SeePlayer()
    {
        if (FieldOfView)
        {

        }
    }

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
