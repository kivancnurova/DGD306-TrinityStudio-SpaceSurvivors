using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 30f;
    public float health = 30f;
    public int scoreWorth = 50;

    public GameObject bulletPrefab;
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;
    public float fireRate = 1.5f;
    float timer;

    public Transform healthBarFill;
    float initialFillScaleX;

    public event Action onDestroyed;

    void Awake()
    {
        health = maxHealth;
        if (healthBarFill != null)
        {
            initialFillScaleX = healthBarFill.localScale.x;
        }
    }

    void Start()
    {
        timer = fireRate;
        UpdateHealthBar();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            if (bulletPrefab != null)
            {
                List<Transform> points = new List<Transform>();
                if (firePoint1 != null) points.Add(firePoint1);
                if (firePoint2 != null) points.Add(firePoint2);
                if (firePoint3 != null) points.Add(firePoint3);
                if (firePoint4 != null) points.Add(firePoint4);

                foreach (var fp in points)
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
        if (health < 0f) health = 0f;
        UpdateHealthBar();
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        GiveScoreToPlayer();
        onDestroyed?.Invoke();
        Destroy(gameObject);
    }

    void GiveScoreToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.score += scoreWorth;
            }
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarFill == null || maxHealth <= 0f) return;
        float ratio = health / maxHealth;
        ratio = Mathf.Clamp01(ratio);
        Vector3 s = healthBarFill.localScale;
        s.x = initialFillScaleX * ratio;
        healthBarFill.localScale = s;
    }
}
