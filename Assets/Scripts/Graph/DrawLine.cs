//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DrawLine : MonoBehaviour
//{
//    public LineRenderer myLineRender;
//    [SerializeField] private int nodeCount;
//    private NodeManager myNodeManager;
//    private List<Node> nodeList;
//    [SerializeField] private Edge edge;


//    private void Awake()
//    {
//        myLineRender = GetComponent<LineRenderer>();
//        myNodeManager = FindAnyObjectByType<NodeManager>();
//    }
//    private void Start()
//    {
//        nodeList = myNodeManager.GetNodeList();
//        if (nodeList == null || nodeList.Count == 0)
//        {
//            Debug.LogError("Node List is empty");
//            return;
//        }
//        SetUpLine(nodeList);
//    }
//    private void Update()
//    {
//        for (int i = 0; i < nodeList.Count; i++)
//        {
//            myLineRender.SetPosition(i, nodeList[i].transform.position);
//        }
//    }

//    public void SetUpLine(List<Node> nodeList)
//    {
//        myLineRender.positionCount = nodeList.Count;
//        this.nodeList = nodeList;
//    }
//}
