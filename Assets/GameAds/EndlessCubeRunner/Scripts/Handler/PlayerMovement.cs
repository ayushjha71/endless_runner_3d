using gameAds.Manager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private float nextSpeedIncreaseScore = 100f; // First speed increase at 100m points
    public float scoreIntervalForSpeedIncrease = 100f; // Increase speed every 100mpoints

    float horizontalInput;
    public float horizontalMultiplier = 1.5f;

    public GameObject GameOverPanel;

    private bool isShielded = false;

    // Mobile touch controls
    [Header("Mobile Touch Controls")]
    public float touchSensitivity = 1f;
    private bool isTouchingLeft = false;
    private bool isTouchingRight = false;

    // Jump control settings
    [Header("Jump Controls")]
    public float jumpZoneWidth = 0.3f; // Center zone width for jump (0.3 = 30% of screen width)
    private bool jumpTouchDetected = false;

    // Position clamping
    [Header("Position Clamping")]
    public float leftBoundary = -2.5f;  // Adjust based on your road width
    public float rightBoundary = 2.5f;  // Adjust based on your road width

    // Smooth movement
    private float targetHorizontalInput = 0f;
    public float inputSmoothSpeed = 8f;

    private void Start()
    {
        CustomEvents.OnGetPlayerMovementHandler?.Invoke(this);
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

        // Calculate new position
        Vector3 newPosition = rb.position + forwardMove + horizontalMove;

        // Clamp horizontal position to keep player within boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);

        rb.MovePosition(newPosition);
    }

    private void Update()
    {
        // Handle mobile touch input for left/right movement
        HandleTouchInput();

        // Get keyboard input (for testing in editor)
        float keyboardInput = Input.GetAxis("Horizontal");

        // Combine touch and keyboard input
        if (isTouchingLeft)
        {
            targetHorizontalInput = -1f * touchSensitivity;
        }
        else if (isTouchingRight)
        {
            targetHorizontalInput = 1f * touchSensitivity;
        }
        else if (Mathf.Abs(keyboardInput) > 0.1f)
        {
            targetHorizontalInput = keyboardInput;
        }
        else
        {
            targetHorizontalInput = 0f;
        }

        // Smooth the horizontal input for better feel
        horizontalInput = Mathf.Lerp(horizontalInput, targetHorizontalInput, inputSmoothSpeed * Time.deltaTime);

        // Score-based speed increase
        if (GameManager.Instance.TotalDistance >= nextSpeedIncreaseScore)
        {
            IncreaseSpeed();
            nextSpeedIncreaseScore += scoreIntervalForSpeedIncrease;
        }

        // Jump input - keyboard for editor, touch handled separately
        if (Input.GetButtonDown("Jump"))
        {
            TryJump();
        }

        // Handle jump from touch input
        if (jumpTouchDetected)
        {
            TryJump();
            jumpTouchDetected = false; // Reset after using
        }
    }

    private void HandleTouchInput()
    {
        isTouchingLeft = false;
        isTouchingRight = false;
        jumpTouchDetected = false;

        // Calculate screen zones
        float screenWidth = Screen.width;
        float centerZoneStart = screenWidth * (0.5f - jumpZoneWidth * 0.5f);
        float centerZoneEnd = screenWidth * (0.5f + jumpZoneWidth * 0.5f);

        // Check all active touches
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Check which zone the touch is in
                if (touch.position.x < centerZoneStart)
                {
                    // Left zone - movement only
                    isTouchingLeft = true;
                }
                else if (touch.position.x > centerZoneEnd)
                {
                    // Right zone - movement only
                    isTouchingRight = true;
                }
                else
                {
                    // Center zone - jump only
                    jumpTouchDetected = true;
                }
            }
            else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // Continue movement for held touches (only check movement zones)
                if (touch.position.x < centerZoneStart)
                {
                    isTouchingLeft = true;
                }
                else if (touch.position.x > centerZoneEnd)
                {
                    isTouchingRight = true;
                }
                // Note: No continuous jump in center zone - only on TouchPhase.Began
            }
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
            CustomEvents.OnCoinCOllected?.Invoke(1);
            //scoreManager.AddCoin(1);
        }
    }

    private void LoseLife()
    {
        currentLives--;
        UpdateLifeImages();

        if (currentLives <= 0)
        {
            CustomEvents.OnGameOver?.Invoke();
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

    // Public methods to adjust settings at runtime
    public void SetTouchSensitivity(float sensitivity)
    {
        touchSensitivity = sensitivity;
    }

    public void SetBoundaries(float left, float right)
    {
        leftBoundary = left;
        rightBoundary = right;
    }
}