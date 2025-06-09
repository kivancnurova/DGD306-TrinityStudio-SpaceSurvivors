using System;
using UnityEngine;

public class DestroyNotifier : MonoBehaviour
{
    public Action onDestroyed;

    void OnDestroy()
    {
        onDestroyed?.Invoke();
    }
}
