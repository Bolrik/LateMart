using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class ResetAnchor : MonoBehaviour
    {
        [SerializeField] private Transform anchor;
        public Transform Anchor { get { return anchor; } }

        [SerializeField] private float radius;
        public float Radius { get { return radius; } }

        [SerializeField] private float speed;
        public float Speed { get { return speed; } }


        public bool IsReset { get; private set; }


        void LateUpdate()
        {
            if (this.IsReset)
            {
                return;
            }

            float distance = (this.transform.position - this.Anchor.position).magnitude;

            if (distance > this.Radius)
            {
                this.IsReset = true;
            }
        }

        private void Update()
        {
            if (this.IsReset)
            {
                Vector3 delta = this.anchor.position - this.transform.position;
                float change = this.Speed * Time.deltaTime;

                change = change.ClampMax(delta.magnitude);
                this.transform.position += delta.normalized * change;

                if (delta.magnitude <= float.Epsilon)
                    this.IsReset = false;
            }
        }
    }
}