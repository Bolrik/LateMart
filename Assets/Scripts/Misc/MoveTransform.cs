using UnityEngine;
using System;

namespace Misc
{
    public class MoveTransform : MonoBehaviour
    {
        [SerializeField] private Transform[] targets;
        public Transform[] Targets { get { return targets; } }

        [SerializeField] private float time;
        public float Time { get { return time; } }


        [SerializeField] private bool setRotation;
        public bool SetRotation { get { return setRotation; } }

        [SerializeField] private bool setPosition;
        public bool SetPosition { get { return setPosition; } }


        Vector3 OriginPosition { get; set; }
        Quaternion OriginRotation { get; set; }

        Vector3 TargetPosition { get; set; }
        Quaternion TargetRotation { get; set; }

        float LerpTime { get; set; }
        int TargetIndex { get; set; }

        private void Start()
        {
            this.AdvanceTarget();
            this.AdvanceTarget();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            this.LerpTime += 1f / this.Time * UnityEngine.Time.deltaTime;

            if (this.SetPosition)
                this.transform.position = Vector3.Slerp(this.OriginPosition, this.TargetPosition, Mathf.SmoothStep(0, 1, this.LerpTime));

            if (this.SetRotation)
                this.transform.rotation = Quaternion.Slerp(this.OriginRotation, this.TargetRotation, this.LerpTime);

            if (this.LerpTime >= 1)
            {
                this.AdvanceTarget();
                this.LerpTime = 0;
            }
        }

        void AdvanceTarget()
        {
            this.TargetIndex = (this.TargetIndex + 1) % this.Targets.Length;

            this.OriginPosition = this.TargetPosition;
            this.OriginRotation = this.TargetRotation;

            Transform target = this.Targets[this.TargetIndex];

            this.TargetPosition = target.position;
            this.TargetRotation = target.rotation;
        }
    }

    public class SinMoveTransform : MonoBehaviour
    {
        [SerializeField] private Transform[] targets;
        public Transform[] Targets { get { return targets; } }

        [SerializeField] private float speed;
        public float Speed { get { return speed; } }


        [SerializeField] private bool setRotation;
        public bool SetRotation { get { return setRotation; } }

        [SerializeField] private bool setPosition;
        public bool SetPosition { get { return setPosition; } }


        Vector3 OriginPosition { get; set; }
        Quaternion OriginRotation { get; set; }

        Vector3 TargetPosition { get; set; }
        Quaternion TargetRotation { get; set; }

        float LerpTime { get; set; }
        int TargetIndex { get; set; }

        // Update is called once per frame
        void LateUpdate()
        {
            this.LerpTime += Time.deltaTime * this.Speed;

            if (this.SetPosition)
                this.transform.position = Vector3.Lerp(this.OriginPosition, this.TargetPosition, this.LerpTime);

            if (this.SetRotation)
                this.transform.rotation = Quaternion.Lerp(this.OriginRotation, this.TargetRotation, this.LerpTime);

            if (this.LerpTime >= 1)
            {
                this.AdvanceTarget();
                this.LerpTime = 0;
            }
        }

        void AdvanceTarget()
        {
            this.TargetIndex = (this.TargetIndex + 1) % this.Targets.Length;

            this.OriginPosition = this.TargetPosition;
            this.OriginRotation = this.TargetRotation;

            Transform target = this.Targets[this.TargetIndex];

            this.TargetPosition = target.position;
            this.TargetRotation = target.rotation;
        }
    }
}