using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float timeSpeed = 1;

    void Update()
    {
        Time.timeScale = timeSpeed;
    }
}
