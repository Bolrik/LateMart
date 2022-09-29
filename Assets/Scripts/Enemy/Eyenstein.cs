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

        [SerializeField] private Transform viewTransform;
        public Transform ViewTransform { get { return viewTransform; } }


        [SerializeField] private float speed;
        public float Speed { get { return speed; } }
        float VisibilitySlow { get; set; }


        private EyensteinState State { get; set; }



        private float Height { get; set; }
        private float HeightOrigin { get; set; }

        [SerializeField] private float heightTarget = 0;
        public float HeightTarget { get { return heightTarget; } private set { this.heightTarget = value; } }

        public bool IsVisibleToPlayer { get; private set; }
        public bool IsChasingPlayer { get; private set; }
        public float IsVisibleToPlayerTime { get; private set; }


        //private float HeightTarget { get; set; }

        private void Update()
        {
            this.UpdateHeight();
            this.CalculateStateInfo();

            this.UpdateState();
        }

        private void CalculateStateInfo()
        {
            Vector3 playerViewDirection = this.Player.ViewController.Head.forward;
            playerViewDirection.y = 0;

            Vector3 viewDirection = this.ViewTransform.forward;
            viewDirection.y = 0;

            this.IsVisibleToPlayer = Vector3.Angle(playerViewDirection, viewDirection) > 160;

            if (this.IsVisibleToPlayer)
            {
                this.IsVisibleToPlayerTime += Time.deltaTime;
            }
            else
            {
                this.IsVisibleToPlayerTime = 0;
            }
        }

        private void UpdateState()
        {
            switch (this.State)
            {
                case EyensteinState.Idle:
                    this.UpdateState_Idle();
                    break;
                case EyensteinState.Follow:
                    this.UpdateState_Follow();
                    break;
                case EyensteinState.Chase:
                    this.UpdateState_Chase();
                    break;
                case EyensteinState.Flee:
                    this.UpdateState_Flee();
                    break;
                case EyensteinState.Wait:
                    this.UpdateState_Wait();
                    break;
                default:
                    break;
            }
        }

        private void UpdateState_Idle()
        {
            this.SetHeight(20);
        }

        private void UpdateState_Follow()
        {

        }

        private void UpdateState_Chase()
        {

        }

        private void UpdateState_Flee()
        {

        }

        private void UpdateState_Wait()
        {

        }

        private void MoveToPlayer()
        {
            Vector3 playerViewDirection = this.Player.ViewController.Head.forward;
            playerViewDirection.y = 0;

            Vector3 viewDirection = this.ViewTransform.forward;
            viewDirection.y = 0;

            this.IsVisibleToPlayer = Vector3.Angle(playerViewDirection, viewDirection) > 160;

            if (this.IsVisibleToPlayer)
            {
                this.IsVisibleToPlayerTime += Time.deltaTime;

                Vector3 delta = this.Player.transform.position - this.transform.position;
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position - delta, this.Speed * Time.deltaTime * 4);
            }
            else
            {
                this.IsChasingPlayer = true;
                // if(this.IsVisibleToPlayerTime > )
            }

            if (this.IsChasingPlayer)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.Player.transform.position, this.Speed * Time.deltaTime);
            }
        }

        private void UpdateHeight()
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

    public enum EyensteinState
    {
        Idle,
        Follow,
        Chase,
        Flee,
        Wait // Rest
    }
}