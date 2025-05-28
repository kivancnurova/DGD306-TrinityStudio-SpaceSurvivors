using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSideMovement : MonoBehaviour
{
    public float minX;
    public float maxX;

    public float speed = 2f;

    private int dir = 1;

    void Update()
    {
        transform.Translate(Vector2.right * dir * speed * Time.deltaTime);

        if (transform.position.x >= maxX) dir = -1;
        else if (transform.position.x <= minX) dir = 1;
    }
}