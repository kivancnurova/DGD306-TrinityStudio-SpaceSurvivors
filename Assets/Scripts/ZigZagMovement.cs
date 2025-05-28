using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagMovement : MonoBehaviour, IMovement
{
    public float speed, amplitude, frequency;
    Vector3 startPos;
    public void Initialize(Transform self)
    {
        startPos = transform.position;
    }
    public void Tick()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        float offsetX = Mathf.Sin((Time.time + startPos.y) * frequency) * amplitude;
        transform.position = new Vector3(startPos.x + offsetX, transform.position.y, 0);
    }
    void Update() => Tick();
}