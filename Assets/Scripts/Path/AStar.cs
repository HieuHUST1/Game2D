using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private static float Heuristic(Node start, Node end)
    {
        return Vector3.Distance(start.transform.position, end.transform.position);
    }

    public static List<Node> FindPath(Node startNode, Node endNode)
    {
        List<Node> openSet = new List<Node>() { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, float> gScore = new Dictionary<Node, float>();
        Dictionary<Node, float> fScore = new Dictionary<Node, float>();

        gScore[startNode] = 0;
        fScore[startNode] = Heuristic(startNode, endNode);

        while (openSet.Count > 0)
        {
            Node currentNode = GetNodeWithLowestFScore(openSet, fScore);

            if (currentNode == endNode)
            {
                Debug.Log("Found path to end node.");
                return ReconstructPath(cameFrom, currentNode);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (Node neighbor in currentNode.GetConnectedNodes())
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[currentNode] + Vector3.Distance(currentNode.Position, neighbor.Position);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = currentNode;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, endNode);
            }
        }

        Debug.Log("No path found.");
        return null;
    }


    private static Node GetNodeWithLowestFScore(List<Node> openSet, Dictionary<Node, float> fScore)
    {
        Node lowestFScoreNode = openSet[0];

        float lowestFScore = fScore.ContainsKey(lowestFScoreNode) ? fScore[lowestFScoreNode] : float.MaxValue;

        foreach (Node node in openSet)
        {
            float score = fScore.ContainsKey(node) ? fScore[node] : float.MaxValue;
            if (score < lowestFScore)
            {
                lowestFScore = score;
                lowestFScoreNode = node;
            }
        }

        return lowestFScoreNode;
    }

    private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node currentNode)
    {
        List<Node> totalPath = new List<Node> { currentNode };
       
        while (cameFrom.ContainsKey(currentNode))
        {
            currentNode = cameFrom[currentNode];
            totalPath.Add(currentNode);
        }
        
        totalPath.Reverse();

        //Debug.Log(totalPath.Count);
        return totalPath;
    }
}