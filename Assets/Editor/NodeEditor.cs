using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.Common.GameUI;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    private Node node;

    private void OnEnable()
    {
        node = (Node)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set Node Index"))
        {
            SetNodeIndex();
        }
    }

    private void SetNodeIndex()
    {
        NodeManager nodeManager = FindObjectOfType<NodeManager>();
        if (nodeManager != null)
        {
            List<Node> nodeList = nodeManager.GetNodeList();
            int index = nodeList.IndexOf(node);
            Debug.Log(index);
            if (index >= 0)
            {
                node.SetIndex(index);
            }
        }
        else
        {
            Debug.LogError("Node Manager is not found");
        }
    }


    [MenuItem("Tools/Set Node Index Ctrl+Shift+I")]
    private static void SetIndexForAllNodes()
    {
        NodeManager nodeManager = FindObjectOfType<NodeManager>();
        if (nodeManager != null)
        {
            List<Node> nodeList = nodeManager.GetNodeList();
            for (int i = 0; i < nodeList.Count; i++)
            {
                nodeList[i].SetIndex(i);
                EditorUtility.SetDirty(nodeList[i]);
            }
        }
        else
        {
            Debug.LogError("Node Manager is not found");
        }
    }


}
