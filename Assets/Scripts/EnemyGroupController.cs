using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float boundaryX = 8f;
    public float moveDownAmount = 0.5f;

    public float speedIncreaseInterval = 10f;
    public float speedIncreaseAmount = 0.5f;

    private float speedTimer;
    private int direction = 1;

    private bool isMovingDown = false;
    private Vector3 targetPosition;
    public float smoothDownSpeed = 3f;

    void Start()
    {
        speedTimer = speedIncreaseInterval;
    }

    void Update()
    {
        if (isMovingDown)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, smoothDownSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMovingDown = false;
            }
            return;
        }

        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        float rightMost = float.MinValue;
        float leftMost = float.MaxValue;

        foreach (Transform enemy in transform)
        {
            if (enemy != null)
            {
                float x = enemy.position.x;
                if (x > rightMost) rightMost = x;
                if (x < leftMost) leftMost = x;
            }
        }

        if (direction == 1 && rightMost >= boundaryX)
        {
            direction = -1;
            SetMoveDownTarget();
        }
        else if (direction == -1 && leftMost <= -boundaryX)
        {
            direction = 1;
            SetMoveDownTarget();
        }

        speedTimer -= Time.deltaTime;
        if (speedTimer <= 0f)
        {
            moveSpeed += speedIncreaseAmount;
            speedTimer = speedIncreaseInterval;
        }
    }

    void SetMoveDownTarget()
    {
        targetPosition = transform.position + new Vector3(0, -moveDownAmount, 0);
        isMovingDown = true;
    }
}
