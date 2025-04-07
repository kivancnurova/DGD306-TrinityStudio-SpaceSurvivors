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
    public float expWorth = 50f;

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
            GiveExpToPlayer();
            Destroy(gameObject);
        }
    }

    void GiveExpToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.AddExp(expWorth);
                Debug.Log("Player gained " + expWorth + " experience.");
                Debug.Log("Player level: " + pc.level + ", experience: " + pc.exp);
            }
        }
    }
}
