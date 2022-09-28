using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy
{
    public class Eyenstein : MonoBehaviour
    {
        [SerializeField] private Transform offsetTransform;
        public Transform OffsetTransform { get { return offsetTransform; } }

        [SerializeField] private PlayerController player;
        public PlayerController Player { get { return player; } }



        private float Height { get; set; }
        private float HeightOrigin { get; set; }

        [SerializeField] private float heightTarget = 0;
        public float HeightTarget { get { return heightTarget; } private set { this.heightTarget = value; } }


        //private float HeightTarget { get; set; }

        private void Update()
        {
            this.Height = this.Height.MoveTowards(this.HeightOrigin, this.HeightTarget, 1f / 2f * Time.deltaTime);

            Vector3 offset = this.OffsetTransform.localPosition;
            offset.y = this.Height;
            this.OffsetTransform.localPosition = offset;


            if ((this.Height - this.HeightTarget).Abs() < .1f)
                this.HeightOrigin = this.HeightTarget;
        }

        public void SetHeight(float value)
        {
            this.HeightOrigin = this.Height;
            this.HeightTarget = value;
        }
    }
}