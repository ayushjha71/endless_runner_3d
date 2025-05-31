using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace EndlessCubeRunner.Handler
{
public class PlayerInputHandler : MonoBehaviour
{
    // Mobile touch controls
    [Header("Mobile Touch Controls")]
    public bool isTouchingLeft = false;
    public bool isTouchingRight = false;
    [Header("Jump Controls")]
    public float jumpZoneWidth = 0.3f; // Center zone width for jump (0.3 = 30% of screen width)
    public bool jumpTouchDetected = false;

    public static PlayerInputHandler Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleTouchInput();
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
    }
}
