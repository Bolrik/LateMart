using UnityEngine;

namespace Misc
{
    public class RotateTransform : MonoBehaviour
    {
        [SerializeField] private Vector3 axisWeight;
        public Vector3 AxisWeight { get { return this.axisWeight; } }

        [SerializeField] private float speed;
        public float Speed { get { return this.speed; } }

        [SerializeField] private Vector3 degree;
        public Vector3 Degree { get { return this.degree; } private set { this.degree = value; } }


        private void Start()
        {
            this.Degree = this.transform.localEulerAngles;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Vector3 rotation = this.Degree;

            rotation.x += this.AxisWeight.x * this.Speed * Time.deltaTime;
            rotation.y += this.AxisWeight.y * this.Speed * Time.deltaTime;
            rotation.z += this.AxisWeight.z * this.Speed * Time.deltaTime;

            this.Degree = rotation;

            this.transform.localEulerAngles = this.Degree;
        }
    }
}