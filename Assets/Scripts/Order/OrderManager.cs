using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Profiling;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private List<OrderInfo> orderInfoList;
    //[SerializeField] private GameObject orderInfoPrefab;
    
    private FoodManager foodManager;
    private ClientManager clientManager;
    private VehicleManager vehicleManager;
    private NodeManager nodeManager;
    [SerializeField] private int price = 100;
    private UIManager uiManager;
    private EffectTakeMoney effectTakeMoney;
    [SerializeField] private float timeToSpawnOrder = 0;

    private void Awake()
    {
        orderInfoList = new List<OrderInfo>();
        foodManager = FindObjectOfType<FoodManager>();
        clientManager = FindObjectOfType<ClientManager>();
        nodeManager = FindObjectOfType<NodeManager>();
        vehicleManager = FindObjectOfType<VehicleManager>();
        uiManager = FindObjectOfType<UIManager>();
        effectTakeMoney = FindObjectOfType<EffectTakeMoney>();
    }
    private void Start()
    {
        for(int i = 0; i < 1; i++)
        {
            CreateOrder();
        }

        StartCoroutine(TimeDelayCreateOrder());
    }

    private void Update()
    {
        AssignPendingOrdersToVehicles();
    }


    [SerializeField] OrderInfo newOrder;
    public void CreateOrder()
    {

        newOrder = new OrderInfo();

        int orderId = orderInfoList.Count + 1;

        newOrder.SetOrderId(orderId);

        Food food = foodManager.GetRandomFood();

        Vector3 foodPosition = nodeManager.RandomFoodAndClientPoint()[0];

        newOrder.SetFood(RespawnFood(food, foodPosition + Vector3.right * 0.1f), foodPosition);

        Client client = clientManager.GetRandomClient(); 

        Vector3 clientPosition = nodeManager.RandomFoodAndClientPoint()[1];

        newOrder.SetClient(RespawnClient(client, clientPosition + Vector3.right * 0.1f), clientPosition);

        newOrder.SetPrice(price);

        Vehicle vehicle = vehicleManager.GetAvailableVehicle();

        if (vehicle != null)
        {
            newOrder.SetVehicleId(vehicle);

            vehicle.GetComponent<VehicleController>().ReceiveOrder(newOrder);

            orderInfoList.Add(newOrder);
            //Debug.Log("preparing");
            //Debug.Log("Order Created: ID = " + newOrder.OrderId);
            return;
            
        }
        else
        {
            newOrder.SetOrderStatus(0); // 0 = pending, 1 = preparing, 2 = delivering, 3 = delivered
            //Debug.Log("peding");
            orderInfoList.Add(newOrder);
        }
           
    }

    public void CompleteOrder(OrderInfo orderInfo)
    {
        orderInfoList.Remove(orderInfo);
        effectTakeMoney.SpawnMoneyEffect(orderInfo.clientPosition, price);

        //uiManager.GetMoneyWhenCompletdOrder(price);
    }

    private void AssignPendingOrdersToVehicles()
    {
        foreach (OrderInfo order in orderInfoList)
        {
            if (order.orderStatus == 0)
            {
                Vehicle vehicle = vehicleManager.GetAvailableVehicle();

                if (vehicle != null)
                {
                    order.SetVehicleId(vehicle);

                    vehicle.GetComponent<VehicleController>().ReceiveOrder(order);

                    order.SetOrderStatus(1);
                }
            }
        }
    }

    private Food RespawnFood(Food food, Vector3 position)
    {
        return Instantiate(food, position, Quaternion.identity);
    }

    private Client RespawnClient(Client client, Vector3 position)
    {
        return Instantiate(client, position, Quaternion.identity);
    }

    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float reductionRate = 0.98f;
    private IEnumerator TimeDelayCreateOrder()
    {
        while (true)
        {        
            yield return new WaitForSeconds(timeToSpawnOrder);
            CreateOrder();

            timeToSpawnOrder = Mathf.Max(timeToSpawnOrder * reductionRate, minSpawnTime);
        }
    }

}
