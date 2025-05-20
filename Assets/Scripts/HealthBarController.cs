using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Header("Assign the SpriteRenderer on your bar sprite")]
    [SerializeField] private SpriteRenderer barSprite;

    private PlayerStats playerStats;

    // Orijinal transform ölçeği ve ilk max health
    private Vector3 originalScale;
    private float baseMaxHealth;

    void Awake()
    {
        // PlayerStats'i bul
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }

        originalScale = transform.localScale;
        baseMaxHealth = playerStats.playerMaxHealth;
    }

    void Start()
    {
        UpdateBar();
    }

    void Update()
    {
        UpdateBar();
    }

    public void UpdateBar()
    {
        float maxH = playerStats.playerMaxHealth;
        float curH = playerStats.playerCurrentHealth;

        float fullLengthFactor = maxH / baseMaxHealth;
        Vector3 fullScale = new Vector3(originalScale.x * fullLengthFactor,
        originalScale.y,
        originalScale.z);

        float ratio = Mathf.Clamp01(curH / maxH);
        transform.localScale = new Vector3(fullScale.x * ratio,
        fullScale.y,
        fullScale.z);

        barSprite.color = Color.Lerp(Color.red, Color.green, ratio);
    }
}
