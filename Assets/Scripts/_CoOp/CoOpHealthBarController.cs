using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoOpHealthBarController : MonoBehaviour
{
    [Header("Assign the SpriteRenderer on your bar sprite")]
    [SerializeField] private SpriteRenderer barSprite;

    private Health healthscript;

    private Vector3 originalScale;
    private float baseMaxHealth;

    void Awake()
    {
        if (healthscript == null)
        {
            healthscript = FindObjectOfType<Health>();
        }

        originalScale = transform.localScale;
        baseMaxHealth = 10;
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
        float maxH =  healthscript.maxHealth;
        float curH = healthscript.currentHealth;

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
