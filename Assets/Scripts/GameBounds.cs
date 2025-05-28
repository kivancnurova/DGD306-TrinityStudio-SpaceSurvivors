using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBounds : MonoBehaviour
{
    public static float yMid;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        yMid = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y;
    }
}
