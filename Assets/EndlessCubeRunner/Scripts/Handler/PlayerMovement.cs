using EndlessCubeRunner.Constant;
using EndlessCubeRunner.Manager;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace EndlessCubeRunner.Handler
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [Header("Movement Setup")]
        [SerializeField]
        private float touchSensitivity = 1f;
        [SerializeField]
        private float baseSpeed = 5f;
        [SerializeField]
        private float speedIncreaseRate = 0.5f; // How much speed increases per second

        [Header("Jump Setup")]
        [SerializeField]
        private float jumpForce = 4;
        [SerializeField]
        private Rigidbody rb;

        [Header("Position Clamping")]
        [SerializeField]
        private float leftBoundary = -1.3f;  // Adjust based on your road width
        [SerializeField]
        private float rightBoundary = 1.7f;  // Adjust based on your road width

        [Header("Powerup UI")]
        [SerializeField]
        private Slider powerupSlider; 

        // Lives system
        public int maxLives = 3;
        private int currentLives;
        public Image[] lifeImages;

        // Smooth movement Input
        private float mTargetHorizontalInput = 0f;
        private float mHorizontalMultiplier = 1.5f;
        private float mInputSmoothSpeed = 5;
        private float mHorizontalInput;

        //Speed
        private float mCurrentSpeed;
        private float mMaxSpeed = 100f; // Maximum speed cap
        private float mNextSpeedIncreaseScore = 200f; // First speed increase at 200m points
        private float mScoreIntervalForSpeedIncrease = 200f; // Increase speed every 200m points

        //Jump
        private int mJumpCount = 0;
        private int mMaxJumps = 2;

        //Ground
        private bool mIsGrounded = false;
        private bool mCanDoubleJump = false;

        // Powerup system
        private bool isShielded = false;
        private bool isSpeedBoosted = false;
        private Coroutine currentPowerupCoroutine;

        // Powerup timer variables
        private float currentPowerupDuration;
        private float maxPowerupDuration;
        private string currentPowerupType = "";

        private void Start()
        {
            EndlessRunnerConstant.OnGetPlayerMovementHandler?.Invoke(this);
            currentLives = maxLives;
            mCurrentSpeed = baseSpeed;
            UpdateLifeImages();
        }

        private void Update()
        {
            // Update powerup slider
            UpdatePowerupSlider();

            // Combine touch and keyboard input
            if (PlayerInputHandler.Instance.isTouchingLeft)
            {
                mTargetHorizontalInput = -1f * touchSensitivity;
            }
            else if (PlayerInputHandler.Instance.isTouchingRight)
            {
                mTargetHorizontalInput = 1f * touchSensitivity;
            }
            else if (Mathf.Abs(PlayerInputHandler.Instance.KeyBoardInput) > 0.1f)
            {
                mTargetHorizontalInput = PlayerInputHandler.Instance.KeyBoardInput;
            }
            else
            {
                mTargetHorizontalInput = 0f;
            }

            // Smooth the horizontal input for better feel
            mHorizontalInput = Mathf.Lerp(mHorizontalInput, mTargetHorizontalInput, mInputSmoothSpeed * Time.deltaTime);

            // Jump input - keyboard for editor, touch handled separately
            if (Input.GetButtonDown("Jump"))
            {
                TryJump();
            }

            // Handle jump from touch input
            if (PlayerInputHandler.Instance.jumpTouchDetected)
            {
                TryJump();
                PlayerInputHandler.Instance.jumpTouchDetected = false; // Reset after using
            }
        }

        private void FixedUpdate()
        {
            // Progressive speed increase over time
            if (mCurrentSpeed < mMaxSpeed)
            {
                mCurrentSpeed += speedIncreaseRate * Time.deltaTime;
                mCurrentSpeed = Mathf.Min(mCurrentSpeed, mMaxSpeed);
            }

            Vector3 forwardMove = transform.forward * mCurrentSpeed * Time.deltaTime;
            Vector3 horizontalMove = transform.right * mHorizontalInput * mCurrentSpeed * Time.deltaTime * mHorizontalMultiplier;

            // Calculate new position
            Vector3 newPosition = rb.position + forwardMove + horizontalMove;

            // Clamp horizontal position to keep player within boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, leftBoundary, rightBoundary);

            rb.MovePosition(newPosition);

            // Score-based speed increase
            if (GameManager.Instance.TotalDistance >= mNextSpeedIncreaseScore)
            {
                IncreaseSpeed();
                mNextSpeedIncreaseScore += mScoreIntervalForSpeedIncrease;
            }
        }

        private void UpdatePowerupSlider()
        {
            if (powerupSlider != null && (isShielded || isSpeedBoosted))
            {
                // Update the slider value based on remaining time
                float normalizedValue = currentPowerupDuration / maxPowerupDuration;
                powerupSlider.value = normalizedValue;

                // Update the countdown (decrease timer)
                currentPowerupDuration -= Time.deltaTime;

                // Update text to show remaining time
                //if (powerupText != null)
                //{
                //    powerupText.text = $"{currentPowerupType}: {currentPowerupDuration:F1}s";
                //}
            }
        }

        private void ShowPowerupUI(string powerupType, float duration)//, Sprite icon)
        {
            currentPowerupType = powerupType;
            currentPowerupDuration = duration;
            maxPowerupDuration = duration;
            // Set slider to full
            if (powerupSlider != null)
                powerupSlider.value = 1f;
        }

        private void HidePowerupUI()
        {
            currentPowerupType = "";
            currentPowerupDuration = 0f;
            maxPowerupDuration = 0f;
        }

        private void IncreaseSpeed()
        {
            mCurrentSpeed += 1f; // Increase speed by 1 unit
            mCurrentSpeed = Mathf.Min(mCurrentSpeed, mMaxSpeed);
        }

        private void TryJump()
        {
            if (mIsGrounded || (mCanDoubleJump && mJumpCount < mMaxJumps))
            {
                GameManager.Instance.PlayAudio(GameManager.Instance.JumpAudio, audioSource);
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                mJumpCount++;

                if (mIsGrounded)
                {
                    mIsGrounded = false;
                    mCanDoubleJump = true;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                mIsGrounded = true;
                mJumpCount = 0;
                mCanDoubleJump = false;
            }

            if (collision.gameObject.tag == "obstacles")
            {
                if (isShielded)
                {
                    Destroy(collision.gameObject);
                    // Shield absorbs hit but continues
                }
                else
                {
                    GameManager.Instance.PlayAudio(GameManager.Instance.DieAudio, audioSource);
                    LoseLife();
                    Destroy(collision.gameObject);
                }
            }
            else if (collision.gameObject.tag == "coins")
            {
                GameManager.Instance.PlayAudio(GameManager.Instance.CollectAudio, audioSource);
                Destroy(collision.gameObject);
                EndlessRunnerConstant.OnCoinCOllected?.Invoke(1);
            }
        }

        private void LoseLife()
        {
            currentLives--;
            UpdateLifeImages();

            if (currentLives <= 0)
            {
                EndlessRunnerConstant.OnGameOver?.Invoke();
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
            // Stop any existing powerup
            if (currentPowerupCoroutine != null)
            {
                StopCoroutine(currentPowerupCoroutine);
            }

            isShielded = true;
            isSpeedBoosted = false;

            // Show shield UI
            ShowPowerupUI("Shield", duration);

            // Start shield coroutine
            currentPowerupCoroutine = StartCoroutine(DeactivateShield(duration));
        }

        IEnumerator DeactivateShield(float duration)
        {
            yield return new WaitForSeconds(duration);
            isShielded = false;
            HidePowerupUI();
            currentPowerupCoroutine = null;
        }

        public void ActivateSpeedBoost(float boostAmount, float duration)
        {
            // Stop any existing powerup
            if (currentPowerupCoroutine != null)
            {
                StopCoroutine(currentPowerupCoroutine);
                // Reset speed if we were speed boosted
                if (isSpeedBoosted)
                {
                    mCurrentSpeed = baseSpeed;
                }
            }

            isSpeedBoosted = true;
            isShielded = true;

            // Show speed boost UI
            ShowPowerupUI("Speed Boost", duration);

            // Start speed boost coroutine
            currentPowerupCoroutine = StartCoroutine(BoostSpeed(boostAmount, duration));
        }

        IEnumerator BoostSpeed(float boostAmount, float duration)
        {
            float originalSpeed = mCurrentSpeed;
            mCurrentSpeed += boostAmount;

            yield return new WaitForSeconds(duration);

            mCurrentSpeed = originalSpeed;
            isSpeedBoosted = false;
            isShielded = false;
            HidePowerupUI();
            currentPowerupCoroutine = null;
        }
    }
}