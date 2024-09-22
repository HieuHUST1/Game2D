using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public List<Food> foodList;
   
    private void Awake()
    {
        
    }

    public Food GetRandomFood()
    {

        Food randomFoodPrefab = foodList[Random.Range(0, foodList.Count)];

        return randomFoodPrefab;
    }

}
