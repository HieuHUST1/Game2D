using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private NodeManager nodeManager;

    [System.Serializable]
    public class NodeData
    {
        public int nodeId;
        public Vector3 position;
        public List<Node> connections = new List<Node>();
    }

    [System.Serializable]
    public class EdgeData
    {
        public Node nodeA;
        public Node nodeB;
        public float weight;
    }

    [System.Serializable]
    public class GraphData
    {
        public List<NodeData> nodes = new List<NodeData>();
        public List<EdgeData> edges = new List<EdgeData>();
    }


    public GraphData SaveGraphData()
    {
        GraphData graphData = new GraphData();
        
        
        nodeManager = FindObjectOfType<NodeManager>();
        
        if (nodeManager != null)
        {
            List<Node> nodeList = nodeManager.GetNodeList();
            
            
            foreach (Node node in nodeList)
            {
                NodeData nodeData = new NodeData();
                nodeData.nodeId = node.GetIndex();
                nodeData.position = node.transform.position;
                
                foreach (Node connection in node.GetConnectedNodes())
                {
                    nodeData.connections.Add(connection);
                }
                graphData.nodes.Add(nodeData);

            }

            foreach (Node node in nodeList)
            {
                foreach (Edge edge in node.GetConnectedEdges())
                {
                    EdgeData edgeData = new EdgeData();
                    edgeData.nodeA = edge.nodeA;
                    edgeData.nodeB = edge.nodeB;
                    edgeData.weight = edge.weight;          
                    
                    graphData.edges.Add(edgeData);
                }
            }

        }
       
        return graphData;
    }
}
