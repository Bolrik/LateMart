using UnityEngine;

namespace Misc
{
    public class PingPongTransform : MonoBehaviour
    {
        [SerializeField] private UpdateType updateType;
        public UpdateType UpdateType { get { return updateType; } }

        [SerializeField] private MinMax<Vector3> minMax;
        public MinMax<Vector3> MinMax { get { return minMax; } }

        [SerializeField] private Vector3 scale;
        public Vector3 Scale { get { return scale; } }

        [SerializeField] private float speed;
        public float Speed { get { return speed; } }

        float TimeTime { get; set; }


        private void Update()
        {
            this.TimeTime += Time.deltaTime * this.Speed;

            if (this.UpdateType == UpdateType.Update)
            {
                this.UpdateTransform();
            }
        }

        private void LateUpdate()
        {
            if (this.UpdateType == UpdateType.LateUpdate)
            {
                this.UpdateTransform();
            }
        }

        private void FixedUpdate()
        {
            if (this.UpdateType == UpdateType.FixedUpdate)
            {
                this.UpdateTransform();
            }
        }

        public void ManualUpdate()
        {
            if (this.UpdateType == UpdateType.Manually)
            {
                this.UpdateTransform();
            }
        }

        private void UpdateTransform()
        {
            Vector3 position = this.transform.localPosition;

            Vector3 delta = this.MinMax.Max - this.MinMax.Min;

            position.x = this.MinMax.Min.x + Mathf.Sin(this.TimeTime * this.Scale.x) * delta.x;
            position.y = this.MinMax.Min.y + Mathf.Sin(this.TimeTime * this.Scale.y) * delta.y;
            position.z = this.MinMax.Min.z + Mathf.Sin(this.TimeTime * this.Scale.z) * delta.z;

            this.transform.localPosition = position;
        }
    }
}