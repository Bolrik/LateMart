using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class LerpTransform : MonoBehaviour
    {
        [SerializeField] private Transform target;
        public Transform Target { get { return target; } }

        [SerializeField] private float lerpTime;
        public float LerpTime { get { return lerpTime; } }

        [SerializeField] private float lerpInstantTime;
        public float LerpInstantTime { get { return lerpInstantTime; } }


        [SerializeField] private LerpTransformMode mode;
        public LerpTransformMode Mode { get { return mode; } set { mode = value; } }



        // Update is called once per frame
        void LateUpdate()
        {
            this.transform.position = Vector3.Slerp(this.transform.position, this.Target.position, this.Mode == LerpTransformMode.Instant ? this.LerpInstantTime : 1f / this.LerpTime * Time.deltaTime);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.Target.rotation, this.Mode == LerpTransformMode.Instant ? this.LerpInstantTime : 1f / this.LerpTime * Time.deltaTime);
        }
    }

    public enum LerpTransformMode
    {
        Instant,
        Lerp
    }
}