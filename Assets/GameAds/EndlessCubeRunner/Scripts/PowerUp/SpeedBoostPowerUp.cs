using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPowerUp : MonoBehaviour
{
    public float boostAmount = 2f;
    public float duration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().ActivateSpeedBoost(boostAmount, duration);
            Destroy(gameObject);
        }
    }
}
