using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;
    private Rigidbody2D rb;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform firePoint2;
    private float nextFireTime = 0f;

    public TMP_Text scoreText;

    Vector2 moveInput = Vector2.zero;

    public AudioClip playerShootSound;
    public AudioSource audioSource;

    public InputActionReference moveAction;


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

    void OnEnable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.Enable();
            moveAction.action.performed += OnMovePerformed;
            moveAction.action.canceled += OnMoveCanceled;
        }
    }

    void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.performed -= OnMovePerformed;
            moveAction.action.canceled -= OnMoveCanceled;
            moveAction.action.Disable();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats.playerCurrentHealth = playerStats.playerMaxHealth; 
    }

    void Update()
    {
        Vector2 movement = moveInput.normalized * playerStats.playerMovementSpeed;
        rb.velocity = movement;

        if (Time.time > nextFireTime)
        {
            if (!firePoint2)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(playerShootSound);
                nextFireTime = Time.time + playerStats.playerFireRate;
            }
            if (firePoint2)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(playerShootSound);
                nextFireTime = Time.time + playerStats.playerFireRate;
            }
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


    void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 val = context.ReadValue<Vector2>();
        moveInput = val;
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
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