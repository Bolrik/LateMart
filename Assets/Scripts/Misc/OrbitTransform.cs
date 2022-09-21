using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class OrbitTransform : MonoBehaviour
    {
        [SerializeField] private Transform target;
        public Transform Target { get { return target; } }

        [SerializeField] private float speed;
        public float Speed { get { return speed; } }

        [SerializeField] private MinMax<float> radius;
        public MinMax<float> Radius { get { return radius; } }

        float Angle { get; set; }
        float Distance { get; set; }
        float DistanceTarget { get; set; }

        float DistanceTime { get; set; }

        private void Start()
        {
            this.Distance = this.DistanceTarget = this.Radius.Min;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            this.Angle += Time.deltaTime * this.Speed;
            this.DistanceTime += Time.deltaTime;

            if (this.DistanceTime >= 5)
            {
                this.DistanceTarget = Random.Range(this.Radius.Min, this.Radius.Max);
                this.DistanceTime = 0;
            }

            this.Distance = Mathf.Lerp(this.Distance, this.DistanceTarget, 1f / 2f * Time.deltaTime);

            var x = Mathf.Cos(this.Angle) * this.Distance;
            var y = Mathf.Sin(this.Angle) * this.Distance;

            this.transform.position = this.Target.position + new Vector3(x, 0, y);
        }
    }
}