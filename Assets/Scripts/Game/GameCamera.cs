using UnityEngine;
namespace Game
{
    public class GameCamera : MonoBehaviour
    {
        [SerializeField] private CameraData data;
        public CameraData Data { get { return data; } }

        private void Start()
        {
            this.Data.Camera = this;
        }
    }
}