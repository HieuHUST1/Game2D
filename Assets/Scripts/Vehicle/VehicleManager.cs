using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [SerializeField] private List<Vehicle> vehicleList;
    [SerializeField] private Vehicle vehiclePrefab;
    [SerializeField] private int vehicleCount = 0;
    public Transform spawnPoint;

    private Pathfinding pathfinding;
    private VehicleController controller;
    private UIManager uiManager;
    private LineRenderer lineRenderer;


    private void Awake()
    {       
        pathfinding = FindAnyObjectByType<Pathfinding>();
        controller = FindAnyObjectByType<VehicleController>();
        uiManager = FindAnyObjectByType<UIManager>();    
    }

    
    private void Start()
    {
        for (int i = 0; i < vehicleCount; i++)
        {
            CreateVehicle();
        }
    }
    private void Update()
    {

        //CreateVehicle();
    }

    public void CreateVehicle()
    {
        Vehicle newVehicle = Instantiate(vehiclePrefab, spawnPoint.position, spawnPoint.rotation);
        //Vehicle newVehicle = Instantiate(vehiclePrefab);


        if (newVehicle != null)
        {
            newVehicle.SetVehicleId(vehicleList.Count + 1);
            newVehicle.vehicleStatus = 0;
            vehicleList.Add(newVehicle);

            

            if (controller != null && pathfinding != null)
            {
                pathfinding.SetVehicleController(controller);
            }
        }
        else
        {
            Debug.LogError("newVehicle is null");
        }
    }

    public Vehicle GetAvailableVehicle()
    {
        foreach (Vehicle vehicle in vehicleList)
        {
            if (vehicle.vehicleStatus == 0)
            {
                VehicleController controller = vehicle.GetComponent<VehicleController>();

                if (pathfinding != null && controller != null)
                {
                    pathfinding.SetVehicleController(controller);
                    vehicle.SetVehicleStatus(1);
                    return vehicle;
                }
                if (pathfinding == null)
                {
                    Debug.Log("SetController2");
                }
                if (controller == null)
                {
                    Debug.Log("SetController3");
                }
                Debug.Log("SetController4");
            }
        }
        return null;
    }
}
