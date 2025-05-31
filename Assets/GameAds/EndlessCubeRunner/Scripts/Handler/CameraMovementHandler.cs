using UnityEngine;

namespace EndlessCubeRunner.Handler
{
    public class CameraMovementHandler : MonoBehaviour
    {
        [SerializeField]
        private Transform Player;
        Vector3 offset;

        // Start is called before the first frame update
        void Start()
        {
            offset = transform.position - Player.position;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 targetPos = Player.position + offset;
            targetPos.x = 0;
            targetPos.y = 2;
            transform.position = targetPos;
        }
    }
}
