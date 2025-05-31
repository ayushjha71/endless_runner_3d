using EndlessCubeRunner.Handler;
using UnityEngine;

namespace EndlessCubeRunner.PowerUp
{
    public class ShieldPowerUp : MonoBehaviour
    {
        public float duration = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerMovement>().ActivateShield(duration);
                Destroy(gameObject);
            }
        }
    }
}