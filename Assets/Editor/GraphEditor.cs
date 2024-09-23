using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(NodeManager))]
public class GraphEditor : Editor
{
    private NodeManager nodeManager;

    private void OnEnable()
    {
        nodeManager = (NodeManager)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Clear All Edges"))
        {
            nodeManager.ClearAllEdge();
            EditorUtility.SetDirty(nodeManager);
        }
    }
    private void OnSceneGUI()
    {
        //foreach (Node node in _nodeManager.GetNodeList())
        //{

        //    Handles.color = Color.green;
        //    foreach (Node connectedNode in node.GetConnectedNodes())
        //    {
        //        Handles.DrawLine(node.transform.position, connectedNode.transform.position);
        //    }
        //}

        DrawNodeConnections();

        HandleNodeConnections();
    }

    private void DrawNodeConnections()
    {
        if (nodeManager != null) return;

        foreach (Node node in nodeManager.GetNodeList())
        {
            Handles.color = Color.green;
            foreach (Node connectedNode in node.GetConnectedNodes())
            {
                if (connectedNode != null)
                {
                    Handles.DrawLine(node.transform.position, connectedNode.transform.position);
                }
            }
        }
    }

    private void HandleNodeConnections()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Debug.Log($"Mouse Clicked: Button {e.button}");

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit an object");

                Node selectedNode = hit.collider.GetComponent<Node>();

                if (selectedNode != null)
                {
                    Debug.Log($"Hit Node: {selectedNode.name}");

                    if (e.control)
                    {
                        Node lastSelectedNode = Selection.activeGameObject?.GetComponent<Node>();

                        if (lastSelectedNode != null && lastSelectedNode != selectedNode)
                        {
                            Debug.Log($"Connecting {lastSelectedNode.name} with {selectedNode.name}");

                            lastSelectedNode.AddConnection(selectedNode);
                            selectedNode.AddConnection(lastSelectedNode);

                            EditorUtility.SetDirty(lastSelectedNode);
                            EditorUtility.SetDirty(selectedNode);
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        Selection.activeGameObject = selectedNode.gameObject;
                        Debug.Log($"Selected Node: {selectedNode.name}");
                    }
                }
                else
                {
                    Debug.LogWarning("No node found under the click.");
                }
            }
            else
            {
                Debug.LogWarning("Raycast did not hit anything.");
            }
        }
        if (e.type == EventType.MouseDown && e.button == 1)
        {
            Debug.Log($"Mouse Right Clicked: Button {e.button}");

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Node selectedNode = hit.collider.GetComponent<Node>();

                if (selectedNode != null)
                {
                    Debug.Log($"Hit Node: {selectedNode.name}");

                    if (e.control)
                    {
                        Node lastSelectedNode = Selection.activeGameObject?.GetComponent<Node>();

                        if (lastSelectedNode != null && lastSelectedNode != selectedNode)
                        {
                            Debug.Log($"Disconnecting {lastSelectedNode.name} from {selectedNode.name}");

                            lastSelectedNode.RemoveConnection(selectedNode);
                            selectedNode.RemoveConnection(lastSelectedNode);

                            Debug.Log($"{lastSelectedNode.name} disconnected from {selectedNode.name}");

                            

                            EditorUtility.SetDirty(lastSelectedNode);
                            EditorUtility.SetDirty(selectedNode);
                        }
                        else
                        {
                            Debug.Log("No previously selected node found.");
                        }
                    }
                    else
                    {
                        Debug.Log("Control key not pressed.");
                    }
                }
                else
                {
                    Debug.Log("No node found under right click.");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }

    }
}
