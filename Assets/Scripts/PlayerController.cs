using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;
    private Rigidbody2D rb;

    public GameObject bulletPrefab;
    public Transform firePoint;
    private float nextFireTime = 0f;

    public TMP_Text scoreText;

    Vector2 movement;

    public AudioClip playerShootSound;
    public AudioSource audioSource;


    void Awake()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from the PlayerController GameObject.");
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats.playerCurrentHealth = playerStats.playerMaxHealth; 
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        rb.velocity = movement.normalized * playerStats.playerMovementSpeed;

        if (Time.time > nextFireTime)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            audioSource.pitch = Random.Range(0.8f, 1.2f); // Randomize pitch for each shot
            audioSource.PlayOneShot(playerShootSound);
            nextFireTime = Time.time + playerStats.playerFireRate;
            
        }



        Vector3 pos = transform.position;
        float xMin = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        float xMax = Camera.main.ViewportToWorldPoint(Vector3.right).x;
        float yMin = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
        float yMax = GameBounds.yMid;
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        transform.position = pos;
        
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = playerStats.score.ToString("0");
        }
    }


    public void TakeDamage(int damage)
    {
        playerStats.playerCurrentHealth -= damage;
        Debug.Log("Player took damage: " + damage + ", remaining health: " + playerStats.playerCurrentHealth);
        if (playerStats.playerCurrentHealth <= 0f)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("MainMenu");
        }
    }

}