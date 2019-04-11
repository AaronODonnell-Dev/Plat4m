using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PathNode : MonoBehaviour
{
    public PathNode NextNode;

    private void OnDrawGizmos()
    {
        if (NextNode != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position,
                NextNode.transform.position);
        }
    }
}
