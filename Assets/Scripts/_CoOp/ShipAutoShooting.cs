using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ShipAutoShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public Transform[] firePoints;

    public float fireInterval = 1.2f;
    public float initialDelay = 0f;

    private string ownerTag;

    private void Awake()
    {
        ownerTag = gameObject.tag;

    }

    private void Start()
    {
        InvokeRepeating(nameof(ShootAllPoints), initialDelay, fireInterval);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(ShootAllPoints));
    }

    private void ShootAllPoints()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"[{name}] bulletPrefab atanmamış!");
            return;
        }
        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogWarning($"[{name}] firePoints dizisi boş veya atanmamış!");
            return;
        }

        foreach (Transform fp in firePoints)
        {
            if (fp == null) continue;
            GameObject bulletGO = Instantiate(bulletPrefab, fp.position, fp.rotation);
            Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = fp.up * bulletSpeed;
            }
            else
            {
                Debug.LogWarning("Bullet prefab'ında Rigidbody2D bulunamadı!");
            }

            Bullet bulletScript = bulletGO.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.ownerTag = ownerTag;
            }
            else
            {
                Debug.LogWarning("Bullet prefab'ında Bullet script bulunamadı!");
            }
        }
    }
}