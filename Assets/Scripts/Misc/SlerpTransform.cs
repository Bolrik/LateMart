using UnityEngine;

namespace Misc
{
    public class SlerpTransform : MonoBehaviour
    {
        [SerializeField] private UpdateType updateType;
        public UpdateType UpdateType { get { return updateType; } }

        [SerializeField] private Transform target;
        public Transform Target { get { return target; } }


        [SerializeField] private bool setRotation;
        public bool SetRotation { get { return setRotation; } }

        [SerializeField] private float positionLerpTime;
        public float PositionLerpTime { get { return positionLerpTime; } set { this.positionLerpTime = value; } }


        [SerializeField] private bool setPosition;
        public bool SetPosition { get { return setPosition; } }

        [SerializeField] private float positionThreshold;
        public float PositionThreshold { get { return positionThreshold; } }

        [SerializeField] private float rotationLerpTime;
        public float RotationLerpTime { get { return rotationLerpTime; } set { this.rotationLerpTime = value; } }



        private void Update()
        {
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
            if (this.SetPosition)
            {
                this.transform.position = Vector3.Slerp(this.transform.position, this.Target.position, this.PositionLerpTime);

                Vector3 delta = this.Target.position - this.transform.position;
                float magnitude = delta.magnitude;

                if (magnitude > this.PositionThreshold)
                {
                    this.transform.position = this.transform.position + delta.normalized * (magnitude - this.PositionThreshold);
                }
            }

            if (this.SetRotation)
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.Target.rotation, this.RotationLerpTime);
        }

        public void SetLerpTime(float lerpTime)
        {
            this.RotationLerpTime = lerpTime;
            this.PositionLerpTime = lerpTime;
        }
    }
}