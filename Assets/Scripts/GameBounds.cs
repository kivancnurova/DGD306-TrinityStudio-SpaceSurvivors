using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameBounds
{
    public static float yMid;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        yMid = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y;
    }
}
