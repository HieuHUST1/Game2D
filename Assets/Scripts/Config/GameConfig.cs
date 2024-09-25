using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Game Config")]
public class GameConfig : SOSingleton<GameConfig>
{
    [field: SerializeField] public GUIStyle gizmosTextStyle { get; private set; }

    [field: SerializeField] public Color edgeLineColor { get; private set; }

    [field: SerializeField] public Color nodeColor { get; private set; }


}
