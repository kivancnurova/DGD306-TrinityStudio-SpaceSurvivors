using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public float health = 30f;
    public GameObject bulletPrefab;
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;
    public float fireRate = 1.5f;
    private float timer;
    public int scoreWorth = 50;
    public audioClip enemy;


    public event Action onDestroyed;


    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            if (bulletPrefab != null)
            {
                // Aktif firePoint’leri tek tek kontrol edip listeye ekleyelim
                List<Transform> activePoints = new List<Transform>();
                if (firePoint1 != null) activePoints.Add(firePoint1);
                if (firePoint2 != null) activePoints.Add(firePoint2);
                if (firePoint3 != null) activePoints.Add(firePoint3);
                if (firePoint4 != null) activePoints.Add(firePoint4);

                // Liste üzerinden instantiate et
                foreach (var fp in activePoints)
                {
                    Instantiate(bulletPrefab, fp.position, fp.rotation);
                }
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
            onDestroyed?.Invoke();
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
