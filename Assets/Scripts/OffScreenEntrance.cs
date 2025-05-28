using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OffScreenEntrance : MonoBehaviour
{
    public Transform target;
    public float speed = 4f;
    private Action onArrive;

    public void Initialize(Transform targetPoint, Action onArriveCallback)
    {
        target = targetPoint;
        onArrive = onArriveCallback;
    }

    void Update()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            onArrive?.Invoke();
            Destroy(this);
        }
    }
}