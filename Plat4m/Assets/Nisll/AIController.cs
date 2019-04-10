using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Nodes))]
public class AIController : MonoBehaviour
{
    public GameObject ObjectToMoveTo;
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
}
