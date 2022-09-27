using UnityEngine;

namespace Misc
{
    public class CloneTransform : MonoBehaviour
    {
        [SerializeField] private UpdateType updateType;
        public UpdateType UpdateType { get { return updateType; } }

        [SerializeField] private Transform target;
        public Transform Target { get { return target; } }


        [SerializeField] private bool setRotation;
        public bool SetRotation { get { return setRotation; } }

        [SerializeField] private bool setPosition;
        public bool SetPosition { get { return setPosition; } }


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

        public void UpdateTransform()
        {
            if (this.SetPosition)
                this.transform.position = this.Target.position;

            if (this.SetRotation)
                this.transform.rotation = this.Target.rotation;
        }
    }
}