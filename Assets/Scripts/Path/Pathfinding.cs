using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private VehicleController vehicleController;
    private AStar AStar;
    private NodeManager nodeManager;

    Vector3 startPoint;
    Vector3 endPoint;

    private Node startNode;
    private Node endNode;

    private void Awake()
    {
        AStar = new AStar();
        nodeManager = FindAnyObjectByType<NodeManager>();
    }

    private void Start()
    {
        //UpdateNodeInGraph();
      
        //List<Node> path = AStar.FindPath(startNode, endNode);
        //if (path != null)
        //{
        //   vehicleController.SetPath(path);
        //}      
    }
    public void SetVehicleController(VehicleController controller)
    {
        vehicleController = controller;
    }
    public List<Node> GetPath(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint= startPoint;
        this.endPoint = endPoint;

        UpdateNodeInGraph();

        List<Node> path = AStar.FindPath(startNode, endNode);

        if (vehicleController != null)
        {
            StartCoroutine(RemoveTemporaryNodesAtEnd());
        }
        else
        {
            Debug.LogError("VehicleController not set!");
        }

        return path;
    }

    private void UpdateNodeInGraph()
    {
        List<Node> nodeList = nodeManager.GetNodeList();

        startNode = GetNodeOrTemporaryNode(startPoint, nodeList);

        endNode = GetNodeOrTemporaryNode(endPoint, nodeList);


        //StartCoroutine(RemoveTemporaryNodesAtEnd());
        
    }

    private Node GetNodeOrTemporaryNode(Vector3 position, List<Node> nodeList)
    {
        foreach (Node node in nodeList)
        {
            if (Vector3.Distance(position, node.Position) < 0.2f)
            {
                  return node;                
            }
        }

        Edge closestEdge = FindEdgeContainingPoint(position, nodeList);
        if (closestEdge != null)
        {       
            Node tempNode = nodeManager.CreateTemporaryNodeOnEdge(position, closestEdge);

            if (tempNode != null)
            {
                //nodeList.Add(tempNode);
            }

            return tempNode;
        }

        Debug.Log("No valid node or edge found at this position");

        return null;
    }

    private Edge FindEdgeContainingPoint(Vector3 point, List<Node> nodeList)
    {
        foreach (Node node in nodeList)
        {
            foreach (Edge edge in node.GetConnectedEdges())
            {
                if (IsPointOnEdge(point, edge))
                {
                    //Debug.Log(edge.name);
                    return edge;
                }
            }
        }
        return null;
    }

    private bool IsPointOnEdge(Vector3 point, Edge edge)
    {
        Vector3 pointA = edge.nodeA.Position;
        Vector3 pointB = edge.nodeB.Position;

        float edgeLength = edge.weight;

        float distanceToA = Vector3.Distance(point, pointA);
        float distanceToB = Vector3.Distance(point, pointB);

        return Mathf.Abs(edgeLength - (distanceToA + distanceToB)) < 0.1f;
    }

    
    private IEnumerator RemoveTemporaryNodesAtEnd()
    {

        while (vehicleController != null && vehicleController.IsMoving() && vehicleController.HaveNewOrder())
        {
            yield return null;
        }

        //Debug.Log("remove temporart node");

        //if (nodeManager.IsTemporaryNode(endNode))
        //{
        //    Debug.Log("Removing temporary end node");
        //    nodeManager.RemoveTemporaryNodeToGraph(endNode);
        //}
        //else
        //{
        //    Debug.Log("End node is not temporary");
        //}
        //if (nodeManager.IsTemporaryNode(startNode))
        //{
        //    Debug.Log("Removing temporary start node");
        //    nodeManager.RemoveTemporaryNodeToGraph(startNode);
        //}
        //else
        //{
        //    Debug.Log("Start node is not temporary");
        //} 



        //error
        //if (vehicleController.HasCompletedOrder())
        //{
        //    Debug.Log("Removing temporary nodes after order completion");

        //    if (nodeManager.IsTemporaryNode(endNode))
        //    {
        //        Debug.Log("Removing temporary end node");
        //        nodeManager.RemoveTemporaryNodeToGraph(endNode);
        //    }
        //    else
        //    {
        //        Debug.Log("End node is not temporary");
        //    }

        //    if (nodeManager.IsTemporaryNode(startNode))
        //    {
        //        Debug.Log("Removing temporary start node");
        //        nodeManager.RemoveTemporaryNodeToGraph(startNode);
        //    }
        //    else
        //    {
        //        Debug.Log("Start node is not temporary");
        //    }
        //    vehicleController.ResetOrderStatus();
        //}
        //else
        //{
        //    Debug.LogWarning("Order not completed yet. Nodes will not be removed.");
        //}
    }


}
