using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float currentSpeed;
    public float speedIncreaseRate = 0.5f; // How much speed increases per second
    public float maxSpeed = 100f; // Maximum speed cap

    public Rigidbody rb;

    // Jump variables
    public float jumpForce = 7f;
    private bool isGrounded = false;
    private bool canDoubleJump = false;
    private int jumpCount = 0;
    public int maxJumps = 2;

    // Lives system
    public int maxLives = 3;
    private int currentLives;
    public Image[] lifeImages;

    // Score tracking for speed milestones
    [SerializeField]
    private ScoreManager scoreManager;
    private float nextSpeedIncreaseScore = 100f; // First speed increase at 50 points
    public float scoreIntervalForSpeedIncrease = 100f; // Increase speed every 50 points

    float horizontalInput;
    public float horizontalMultiplier = 1.5f;

    public GameObject GameOverPanel;

    private bool isShielded = false;

    private void Start()
    {
        currentLives = maxLives;
        currentSpeed = baseSpeed;
        UpdateLifeImages();
    }

    private void FixedUpdate()
    {
        // Progressive speed increase over time
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncreaseRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }

        Vector3 forwardMove = transform.forward * currentSpeed * Time.deltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * currentSpeed * Time.deltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Score-based speed increase (alternative or additional to time-based)
        if (scoreManager != null && scoreManager.totalDistance >= nextSpeedIncreaseScore)
        {
            IncreaseSpeed();
            nextSpeedIncreaseScore += scoreIntervalForSpeedIncrease;
        }

        if (Input.GetButtonDown("Jump"))
        {
            TryJump();
        }
    }

    private void IncreaseSpeed()
    {
        currentSpeed += 1f; // Increase speed by 1 unit
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        // Optional: Add visual/audio feedback for speed increase
        Debug.Log("Speed increased to: " + currentSpeed);
    }

    private void TryJump()
    {
        if (isGrounded || (canDoubleJump && jumpCount < maxJumps))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            jumpCount++;

            if (isGrounded)
            {
                isGrounded = false;
                canDoubleJump = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            jumpCount = 0;
            canDoubleJump = false;
        }

        if (collision.gameObject.tag == "obstacles")
        {
            if (isShielded)
            {
                Destroy(collision.gameObject);
                isShielded = false;
                // Remove shield visual effect immediately
            }
            else
            {
                LoseLife();
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "coins")
        {
            Destroy(collision.gameObject);
            scoreManager.AddCoin(1);
        }
    }

    private void LoseLife()
    {
        currentLives--;
        UpdateLifeImages();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLifeImages()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            lifeImages[i].enabled = i < currentLives;
        }
    }

    public void ActivateShield(float duration)
    {
        if (!isShielded)
        {
            isShielded = true;
            // Add shield visual effect
            StartCoroutine(DeactivateShield(duration));
        }
    }

    IEnumerator DeactivateShield(float duration)
    {
        yield return new WaitForSeconds(duration);
        isShielded = false;
        // Remove shield visual effect
    }

    public void ActivateSpeedBoost(float boostAmount, float duration)
    {
        StartCoroutine(BoostSpeed(boostAmount, duration));
    }

    IEnumerator BoostSpeed(float boostAmount, float duration)
    {
        isShielded = true;
        float originalSpeed = currentSpeed;
        currentSpeed += boostAmount;
        // Add speed boost visual effect

        yield return new WaitForSeconds(duration);

        currentSpeed = originalSpeed;
        isShielded = false;
        // Remove speed boost visual effect
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
        Debug.Log("GameOver");
    }

    public void RestartButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}