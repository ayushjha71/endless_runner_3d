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

        private bool isShielded = false;


        private void Start()
        {
            CustomEvents.OnGetPlayerMovementHandler?.Invoke(this);
            currentLives = maxLives;
            mCurrentSpeed = baseSpeed;
            UpdateLifeImages();
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
        }

        private void Update()
        {
            // Get keyboard input (for testing in editor)
            float keyboardInput = Input.GetAxis("Horizontal");

            // Combine touch and keyboard input
            if (PlayerInputHandler.Instance.isTouchingLeft)
            {
                mTargetHorizontalInput = -1f * touchSensitivity;
            }
            else if (PlayerInputHandler.Instance.isTouchingRight)
            {
                mTargetHorizontalInput = 1f * touchSensitivity;
            }
            else if (Mathf.Abs(keyboardInput) > 0.1f)
            {
                mTargetHorizontalInput = keyboardInput;
            }
            else
            {
                mTargetHorizontalInput = 0f;
            }

            // Smooth the horizontal input for better feel
            mHorizontalInput = Mathf.Lerp(mHorizontalInput, mTargetHorizontalInput, mInputSmoothSpeed * Time.deltaTime);

            // Score-based speed increase
            if (GameManager.Instance.TotalDistance >= mNextSpeedIncreaseScore)
            {
                IncreaseSpeed();
                mNextSpeedIncreaseScore += mScoreIntervalForSpeedIncrease;
            }

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
                    isShielded = false;
                    // Remove shield visual effect immediately
                }
                else
                {
                    GameManager.Instance.PlayAudio(GameManager.Instance.DieAudio, audioSource);
                    LoseLife();
                }
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "coins")
            {
                GameManager.Instance.PlayAudio(GameManager.Instance.CollectAudio, audioSource);
                Destroy(collision.gameObject);
                CustomEvents.OnCoinCOllected?.Invoke(1);
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
            float originalSpeed = mCurrentSpeed;
            mCurrentSpeed += boostAmount;
            // Add speed boost visual effect

            yield return new WaitForSeconds(duration);

            mCurrentSpeed = originalSpeed;
            isShielded = false;
            // Remove speed boost visual effect
        }
    }
}