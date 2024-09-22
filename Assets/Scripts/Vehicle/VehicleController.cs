using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float waitTimeAtDestination = 1f;
    [SerializeField] private LineRenderer lineRenderer
    {
        get
        {
            if (_lineRenderer == null)
            {
                _lineRenderer = GetComponent<LineRenderer>();
                if (_lineRenderer == null)
                    _lineRenderer = gameObject.AddComponent<LineRenderer>();

            }
            return _lineRenderer;
        }
        set { _lineRenderer = value; }
    }
    private LineRenderer _lineRenderer;

    private NodeManager nodeManager;
    private List<Node> path;
    private int currentTargetIndex = 0;

    private Pathfinding pathfinding;
    private OrderManager orderManager;
    private VehicleManager vehicleManager;
    private UIManager buttonManager;
    

    public bool isMoving = false;
    private bool hasCompletedOrder = false;


    private void Awake()
    {
        nodeManager = FindAnyObjectByType<NodeManager>();
        pathfinding = FindAnyObjectByType<Pathfinding>();
        orderManager = FindAnyObjectByType<OrderManager>();
        vehicleManager = FindAnyObjectByType<VehicleManager>();
        buttonManager = FindAnyObjectByType<UIManager>();
    }
    private void Start()
    {
        this.transform.position = Vector3.zero;
        //this.transform.position = vehicleManager.spawnPoint.position;

        //if (lineRenderer == null)
        //{
        //    lineRenderer = gameObject.AddComponent<LineRenderer>();
        //}

        lineRenderer.startWidth = 0.015f;
        lineRenderer.endWidth = 0.015f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 0;
    }

    private void Update()
    {
        this.speed = buttonManager.SetNewSpeedForVehicle();

        if (isMoving)
        {
            MoveALongPath();
        }
    }
    private void DrawLineFullPath()
    {
        if (path == null || path.Count == 0) return;
        

        lineRenderer.positionCount = path.Count + 1;
        lineRenderer.SetPosition(0, transform.position);
        
        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, path[i].Position);
        }
    }
    public void ReceiveOrder(OrderInfo orderInfo)
    {
        if (orderInfo == null)
        {
            Debug.LogError("Received OrderInfo is null.");
            return;
        }

        isMoving = true;
        path = pathfinding.GetPath(this.transform.position, orderInfo.foodPosition);
        DrawLineFullPath();
        orderInfo.SetOrderStatus(1);
        currentTargetIndex = 0;
        StartCoroutine(DeliverOrder(orderInfo));
    }

    public IEnumerator DeliverOrder(OrderInfo orderInfo)
    {
        yield return new WaitUntil(() => currentTargetIndex >= path.Count);


        isMoving = true;
        path = pathfinding.GetPath(orderInfo.foodPosition, orderInfo.clientPosition);
        orderInfo.SetOrderStatus(2);
        Destroy(orderInfo.food.gameObject);
        currentTargetIndex = 0;


        yield return new WaitUntil(() => currentTargetIndex >= path.Count);


        orderInfo.SetOrderStatus(3);
        orderManager.CompleteOrder(orderInfo);
        Destroy(orderInfo.client.gameObject);
        CompleteOrderVehicle();
        hasCompletedOrder = true;

        Debug.Log("Order Delivery");
        isMoving = false;
        //path = null;
    }

    private void CompleteOrderVehicle()
    {
        //isMoving = false;
        GetComponent<Vehicle>().SetVehicleStatus(0);
    }

    public void ResetOrderStatus()
    {
        hasCompletedOrder = false;
    }

    public bool HasCompletedOrder()
    {
        return hasCompletedOrder;
    }

    public void SetPath(List<Node> newPath)
    {
        path = newPath;
        currentTargetIndex = 0;
        isMoving = true;

        if (path != null && path.Count > 0)
        {
            transform.position = path[0].Position;
        }
    }

    private void MoveALongPath()
    {
        if (path == null || path.Count == 0)
        {
            isMoving = false;
            return;
        }
         
        Node currentNode = path[currentTargetIndex];

        Vector3 targetPosition = currentNode.Position;

        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle - 90f), 1500f * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        //UpdateLineRenderer();

        RemoveCompletedLine();

        if (Vector3.Distance(transform.position, targetPosition) < 0.03f)
        {
            currentTargetIndex++;

        }
    }

    private void RemoveCompletedLine()
    {
        if (currentTargetIndex >= path.Count)
            return; 

        int remainingPoints = path.Count - currentTargetIndex;

        lineRenderer.positionCount = remainingPoints + 1;
        lineRenderer.SetPosition(0, transform.position);

        for (int i = 0; i < remainingPoints; i++)
        {
             lineRenderer.SetPosition(i + 1, path[currentTargetIndex + i].Position);
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
    public bool HaveNewOrder()
    {
        if (GetComponent<Vehicle>().vehicleStatus == 1)
            return true;

        return false;
    }


    private void OnDrawGizmos()
    {
       if (path != null)
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < path.Count - 1; i++ )
            {
                if (path[i] == null || path[i + 1] == null)
                {
                    continue; 
                }
               
                Gizmos.DrawLine(path[i].Position, path[i + 1].Position);
            }
        } 
    }
}
