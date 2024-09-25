using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MBSingleton<T> : MonoBehaviour where T : MonoBehaviour 
{
    public bool canDestroy = true;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null) Debug.Log("Create the " + typeof(T).ToString() + "!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != null && _instance != this) 
        {
            Destroy(gameObject);
        }    
    }
}
