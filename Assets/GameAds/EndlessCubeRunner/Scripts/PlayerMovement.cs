using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody rb;

    float horizontalInput;
    public float horizontalMultiplier = 1.5f;

    public GameObject GameOverPanel;
    public Score score;
    public Text ScoreText;
    private void FixedUpdate()
    {
        Vector3 forwardMove = transform.forward * speed * Time.deltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.deltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    // Update is called once per frame
    public void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "obstacles")
        {
            Debug.Log("Run");
            Time.timeScale = 0f;
            GameOver();   
        }
        else if(collision.gameObject.tag == "coins")
        {
            Destroy(collision.gameObject);
            score.AddScore(1);
        }
    }
    public void OnRightCick()
    {
        Vector3 leftMove = transform.right * 50f * Time.deltaTime;
        Vector3 leftmove = transform.right * horizontalInput * 50f * Time.deltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + leftMove + leftmove);
    }
    public void onLeftClick()
    {
        Vector3 leftMove = -transform.right * 50f * Time.deltaTime;
        Vector3 leftmove = -transform.right * horizontalInput * 50f * Time.deltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + leftMove + leftmove);
    }
    void GameOver()
    {
        GameOverPanel.SetActive(true);
        Debug.Log("GameOver");
    }
    public void RestartButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
