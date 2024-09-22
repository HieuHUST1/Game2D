using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrderInfo 
{
    public int OrderId;

    public Food food;
    public Vector3 foodPosition;

    public Client client;

    public Vector3 clientPosition;

    public Vehicle vehicle;

    public int VehicleId;

    public int orderStatus;

    public int price;// 0 = pending, 1 = preparing, 2 = delivering, 3 = delivered    //Enum


    public void SetOrderId(int id) 
    {
        this.OrderId = id;
    }

    public void SetFood(Food foodItem, Vector3 foodPos)
    {
        this.food = foodItem;
        this.foodPosition = foodPos;
    }

    public void SetClient(Client clientItem, Vector3 clientPos)
    {
        this.client = clientItem;
        this.clientPosition = clientPos;
    }

    public void SetVehicleId(Vehicle vehicle)
    {
        this.vehicle = vehicle;
        this.VehicleId = vehicle.vehicleId;
    }

    public void SetOrderStatus(int status)
    {
        this.orderStatus = status;
    }

    public void SetPrice(int price)
    {
        this.price = price;
    }

    
}

public enum OrderStatus{
    pending,
    preparing,
    delivering,
    delivered
}
