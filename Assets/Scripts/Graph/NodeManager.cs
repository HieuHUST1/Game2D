using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private List<Node> nodeList;
    [SerializeField] private LineRenderer lineRendererPrefab;

    [SerializeField] private GameObject edgeStorage;
    private List<Edge> edges;

    public List<Node> temporaryNodeList = new List<Node>();
    private void Awake()
    {

    }
 
    public List<Node> GetNodeList()
    {
        return new List<Node>(nodeList);
    }

    public List<Edge> GetEdges()
    {
        return new List<Edge>(edges);
    }



    public void RemoveTemporaryNodeToGraph(Node tempNode)
    {
        if (tempNode == null) return;
        Node nodeA = tempNode.connectedNodes[0];
        Node nodeB = tempNode.connectedNodes[1];

        nodeA.RemoveConnection(tempNode);
        nodeB.RemoveConnection(tempNode);
        tempNode.RemoveAllConnectionsOfTemporaryNode();
        

        nodeList.Remove(tempNode);
        temporaryNodeList.Remove(tempNode);

        Destroy(tempNode.gameObject);
    }


    private void AddTemporaryNodeToGraph(Node tempNode)
    {
        if (tempNode == null)
        {
            Debug.Log("Cannot add a null node to the graph");
            return;
        }

        if (!nodeList.Contains(tempNode))
        {
            nodeList.Add(tempNode);
        } 
    }
    

    

    public void ClearAllEdge()
    {
        foreach (Node node in nodeList)
        {
            node.connectedNodes.Clear();
            node.connectedEdges.Clear();
        }

        
        if (edgeStorage != null) 
        {
            for (int i = edgeStorage.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = edgeStorage.transform.GetChild(i);

                DestroyImmediate(child.gameObject);
            }

        }

    }

    public bool IsTemporaryNode(Node tempNode)
    {
        if (temporaryNodeList.Contains(tempNode))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Node CreateTemporaryNodeOnEdge(Vector3 position, Edge edge)
    {
        GameObject tempNodeObject = new GameObject($"TemporaryNode_OnEdge_{edge.nodeA.name}_to_{edge.nodeB.name}");
        Node tempNode = tempNodeObject.AddComponent<Node>();
        tempNode.transform.position = position;

        if (edgeStorage == null)
        {
            Debug.LogError("EdgeStorage is not assigned!");
            return null;
        }
        if (lineRendererPrefab == null)
        {
            Debug.LogError("LineRendererPrefab is not assigned!");
            return null;
        }

        tempNode.edgeStorage = edgeStorage;
        tempNode.lineRendererPrefab = lineRendererPrefab;


        tempNode.AddConnection(edge.nodeA);
        tempNode.AddConnection(edge.nodeB);

        edge.nodeA.AddConnection(tempNode);
        edge.nodeB.AddConnection(tempNode);

        AddTemporaryNodeToGraph(tempNode);


        temporaryNodeList.Add(tempNode);

        return tempNode;
    }



    [SerializeField] private float radiusCircleOfRandomPoint = 4f;
    public Vector3 GetRandomPointInGraph()
    {       
        Vector3 randomPoint = Vector3.zero;

        int randomNodeIndex = Random.Range(0, nodeList.Count);
        Node randomNode = nodeList[randomNodeIndex];

        int randomEdgeIndex = Random.Range(0, randomNode.connectedEdges.Count);
        Edge randomEdge = randomNode.connectedEdges[randomEdgeIndex];

        float randomRatio = Random.Range(0, 3);

        //Debug.Log("randomratio" +  randomRatio);
 
        if (randomRatio == 0)
        {
            randomPoint = randomEdge.nodeA.Position;
        }
        if (randomRatio == 2)
        {
            randomPoint = randomEdge.nodeB.Position;
        }
        if (randomRatio == 1)
        {
            randomPoint = (randomEdge.nodeA.Position + randomEdge.nodeB.Position) / 2;
        }

        return randomPoint;
    }


    public List<Vector3> RandomFoodAndClientPoint()
    {
        Vector3 randomPointOfFood = GetRandomPointInGraph();

        Vector3 randomPointOfClient = Vector3.zero;

        List<Node> nodeOutCircleList = new List<Node>();

        foreach (Node node in nodeList)
        {
            float distance = (randomPointOfFood.x - node.Position.x) * (randomPointOfFood.x - node.Position.x) + (randomPointOfFood.y - node.Position.y) * (randomPointOfFood.y - node.Position.y);

            if (distance >= radiusCircleOfRandomPoint * radiusCircleOfRandomPoint)
            {
                nodeOutCircleList.Add(node);
            }
        }

        if (nodeOutCircleList.Count > 0)
        {
            Node randomNodeOutCircle = nodeOutCircleList[Random.Range(0, nodeOutCircleList.Count)];

            Edge randomEdgeOutCircle = randomNodeOutCircle.connectedEdges[Random.Range(0, randomNodeOutCircle.connectedEdges.Count)];

            float randomRatio = Random.Range(0, 3);

            if (randomRatio == 0)
            {
                randomPointOfClient = randomEdgeOutCircle.nodeA.Position;
            }
            if (randomRatio == 2)
            {
                randomPointOfClient = randomEdgeOutCircle.nodeB.Position;
            }
            if (randomRatio == 1)
            {
                randomPointOfClient = (randomEdgeOutCircle.nodeA.Position + randomEdgeOutCircle.nodeB.Position) / 2;
            }
        }

        return new List<Vector3> { randomPointOfFood, randomPointOfClient };
    }
}
