using UnityEngine;

namespace Misc
{
    public class LookAtTransform : MonoBehaviour
    {
        [SerializeField] private Transform target;
        public Transform Target { get { return target; } }

        void LateUpdate()
        {
            this.transform.LookAt(this.Target);
        }
    }
}