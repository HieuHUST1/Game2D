using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public Node nodeA;
    public Node nodeB;

    public float weight;

    public LineRenderer lineRenderer;

    public void Initialized(Node nodeA, Node nodeB, LineRenderer lineRendererPrefabs, GameObject edgeStorage)
    {
        this.nodeA = nodeA;
        this.nodeB = nodeB;


        //this.lineRenderer = Instantiate(lineRendererPrefabs, edgeStorage.transform);  

        if (lineRendererPrefabs == null)
        { 
            Debug.LogError("LineRendererPrefab is null! Please assign a valid LineRenderer prefab.");
            return;
        }

        if (edgeStorage == null)
        {
            Debug.LogError("EdgeStorage is null! Please assign a valid GameObject for edge storage.");
            return;
        }

        //// Instantiate LineRenderer and set its parent
        //this.lineRenderer = Instantiate(lineRendererPrefabs) as LineRenderer;
        //if (this.lineRenderer == null)
        //{
        //    Debug.LogError("Failed to instantiate LineRenderer.");
        //    return;
        //}
        //this.lineRenderer.transform.SetParent(edgeStorage.transform);
        
        UpdateEdge();
    }

    public void UpdateEdge()
    {
        //if (lineRenderer == null)
        //{
        //    Debug.LogError("LineRenderer is missing.");
        //    return;
        //}
         
        if (nodeA != null && nodeB != null)
        {
            //lineRenderer.SetPosition(0, nodeA.transform.position); 
            //lineRenderer.SetPosition(1, nodeB.transform.position);
            this.weight = Vector3.Distance(nodeA.transform.position, nodeB.transform.position);
        }
    }

    public void DestroyEdge()
    {
        if(lineRenderer != null)
            DestroyImmediate(lineRenderer.gameObject);

        DestroyImmediate(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GameConfig.Instance.edgeLineColor;

        Gizmos.DrawLine(nodeA.Position, nodeB.Position);
    }
}
