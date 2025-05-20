using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public PlayerStats playerStats;
    public float lifeTime = 2f;


    void Awake()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }
    }


    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * playerStats.playerBulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(playerStats.playerAttackDamage);
            }
            Destroy(gameObject);
        }
    }
}
