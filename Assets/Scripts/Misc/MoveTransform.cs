using UnityEngine;

namespace Misc
{
    public class MoveTransform : MonoBehaviour
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
                this.transform.position = Vector3.Lerp(this.transform.position, this.TargetPosition, this.LerpTime);

            if (this.SetRotation)
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.TargetRotation, this.LerpTime);

            if (this.LerpTime >= 1)
            {
                this.SetTarget();
                this.LerpTime = 0;
            }
        }

        void SetTarget()
        {
            this.OriginPosition = this.transform.position;
            this.OriginRotation = this.transform.rotation;

            Transform target = this.Targets[this.TargetIndex];

            this.TargetPosition = target.position;
            this.TargetRotation = target.rotation;
        }
    }
}