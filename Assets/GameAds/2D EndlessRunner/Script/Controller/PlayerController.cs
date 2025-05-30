using gameAds.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EndlessRunner.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 12f;
        
        [Header("UI References")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField]
        private GameObject userGuide;
        [SerializeField]
        private Button restartButton;
        private Rigidbody2D rb;
        private Animation anim;
        private bool isGrounded = true;

        private void Start()
        {
            gameOverPanel.SetActive(false);
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animation>();
            restartButton.onClick.AddListener(ResetGame);
        }

        private void Update()
        {
            if (!isGrounded)
            {
                anim.Stop();
            }
            if(isGrounded)
            {
                anim.Play("Player");
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && isGrounded)
                {
                    Jump();
                }
            }
        }

        private void Jump()
        {
            if (userGuide.activeInHierarchy)
            {
                userGuide.SetActive(false);
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
            
            if (collision.collider.CompareTag("Obstacle"))
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            AdsManager.Instance.InterstitialAdsHandler.ShowIntersitialAds();
           // gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void ResetGame()
        {
         //   SceneManager.LoadScene();
        }
    }
}