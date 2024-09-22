using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorPath : MonoBehaviour
{
    public float G { get; set; }
    public float H { get; set; }
    public float F => G + H;
    
    public Node previousNode;

    public CalculatorPath()
    {
        G = 0;
        H = 0;
        previousNode = null; 
    }

    public void ResetData()
    {
        G = 0;
        H = 0;
        previousNode = null;
    }
}
