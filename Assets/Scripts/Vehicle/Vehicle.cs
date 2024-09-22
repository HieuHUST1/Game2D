using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    private VehicleController controller;

    public int vehicleId;
    public int vehicleStatus; // 0 = idle, 1 = moving

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

    public void SetVehicleId(int vehicleId)
    {
        this.vehicleId = vehicleId;
    }

    public void SetVehicleStatus(int  vehicleStatus)
    {
        this.vehicleStatus = vehicleStatus;
    }
}
