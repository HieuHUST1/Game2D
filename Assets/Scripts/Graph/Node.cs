using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node : MonoBehaviour
{
    [SerializeField] private int indexOfNode;
    public LineRenderer lineRendererPrefab;
    public GameObject edgeStorage;

    public Vector3 Position => transform.position;


    public List<Node> connectedNodes = new List<Node>();
    public List<Edge> connectedEdges = new List<Edge>();


    private void Awake()
    {


    }


    public void SetIndex(int index)
    {
        this.indexOfNode = index;
    }

    public int GetIndex()
    {
        return indexOfNode;
    }

    public List<Node> GetConnectedNodes()
    {
        return connectedNodes;
    }

    public List<Edge> GetConnectedEdges() 
    { 
        return connectedEdges; 
    }
   
    

    public void AddConnection(Node node)
    {
        if(!connectedNodes.Contains(node))
        {
            connectedNodes.Add(node);

            Edge newEdge = CreateEdge(node);

            connectedEdges.Add(newEdge);
        }
    }

    public void RemoveConnection(Node node)
    {
        if (connectedNodes.Contains(node))
        {
            connectedNodes.Remove(node);

            Edge edgeToRemove = connectedEdges.
                Find(edge =>

                (edge.nodeA == this && edge.nodeB == node) ||
                (edge.nodeA == node && edge.nodeB == this));
            if (edgeToRemove != null)
            {
                edgeToRemove.DestroyEdge();
                connectedEdges.Remove(edgeToRemove);
            }
        }
    }

    public void RemoveAllConnectionsOfTemporaryNode()
    {
        List<Edge> edgesToRemove = new List<Edge>(connectedEdges);

        foreach (Edge edge in edgesToRemove)
        {
            Node connectedNode = edge.nodeA == this ? edge.nodeB : edge.nodeA;
            //Debug.Log(connectedNode.name);
            //RemoveConnection(this);
            RemoveConnection(connectedNode);
        }
    }

    private Edge CreateEdge(Node node)
    {
        GameObject edgeObject = new GameObject($"Data of Edge from {this.indexOfNode} to {node.indexOfNode}");
        if (edgeStorage != null)
        {
            edgeObject.transform.SetParent(edgeStorage.transform);
        }
        else
        {
            Debug.LogError("EdgeStorage is null! Make sure it is assigned.");
        }
        Edge newEdge = edgeObject.AddComponent<Edge>();

        newEdge.Initialized(this, node, lineRendererPrefab, edgeStorage);
        return newEdge;
    }

    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = GameConfig.Instance.nodeColor;
        Gizmos.DrawSphere(transform.position, 0.05f);
        
#if UNITY_EDITOR
        
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.2f, "Node: " + indexOfNode, GameConfig.Instance.gizmosTextStyle);

        if (connectedEdges != null)
        {
            foreach (Edge edge in connectedEdges)
            {
                edge.UpdateEdge();
            }
        }
#endif
    }
}
