using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Order Config")]
public class OrderConfig : SOSingleton<OrderConfig>
{
    [field: SerializeField] public List<OrderInfo> orderList = new List<OrderInfo>();

    [field: SerializeField] public int price { get; private set; }

    [field: SerializeField] public float timeToSpawnOrder { get; private set; }

    [field: SerializeField] public float timeToSpawnOrderMin { get; private set; }

    [field: SerializeField] public float reductionRate { get; private set; }

}
