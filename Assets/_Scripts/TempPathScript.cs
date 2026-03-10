using System.Collections.Generic;
using UnityEngine;

public class TempPathScript : MonoBehaviour
{
    [SerializeField] private Color lineColor;

    private List<Transform> nodes;

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Transform[] pathTransforms = GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();

        foreach(Transform pathTransform in pathTransforms)
        {
            if(pathTransform != transform)
            {
                nodes.Add(pathTransform);
            }
        }

        for(int i=0; i<nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 nextNode = nodes[(i+1) % nodes.Count].position;

            Gizmos.DrawLine(currentNode, nextNode);
            Gizmos.DrawWireSphere(currentNode, 1f);
        }
    }
}
