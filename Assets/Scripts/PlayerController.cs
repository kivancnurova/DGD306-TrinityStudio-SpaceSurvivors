using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float fireTimer;

    public float health = 100f;

    Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireTimer = fireRate;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            if (bulletPrefab != null && firePoint != null)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
            fireTimer = fireRate;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement.normalized * moveSpeed;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player took damage: " + damage + ", remaining health: " + health);
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
