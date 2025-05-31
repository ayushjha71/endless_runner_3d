using EndlessCubeRunner.Handler;
using UnityEngine;

namespace EndlessCubeRunner.PowerUp
{
    public class SpeedBoostPowerUp : MonoBehaviour
    {
        [SerializeField]
        private float boostAmount = 1f;
        [SerializeField]
        private float duration = 3f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerMovement>().ActivateSpeedBoost(boostAmount, duration);
                Destroy(gameObject);
            }
        }
    }
}
