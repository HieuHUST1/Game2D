using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public List<Client> clientList;

    private void Start()
    {
        
    }
    
    public Client GetRandomClient()
    {

        Client randomClientPrefab = clientList[Random.Range(0, clientList.Count)];

        return randomClientPrefab;
    }
}
