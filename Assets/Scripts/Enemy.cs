using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 30f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    private float timer;
    public int scoreWorth = 50;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            if (bulletPrefab && firePoint)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
            timer = fireRate;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage: " + damage + ", remaining health: " + health);
        if (health <= 0f)
        {
            GiveScoreToPlayer();
            Destroy(gameObject);
        }
    }

    void GiveScoreToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.score += scoreWorth;
                Debug.Log("Player gained " + scoreWorth + " score.");
                Debug.Log("Player's new score: " + playerStats.score);
            }
        }
    }
}
