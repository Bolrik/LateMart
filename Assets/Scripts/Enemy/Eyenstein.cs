using Game;
using Player;
using System;
using UnityEngine;

namespace Enemy
{
    public class Eyenstein : MonoBehaviour
    {
        [SerializeField] private Transform offsetTransform;
        public Transform OffsetTransform { get { return this.offsetTransform; } }

        [SerializeField] private PlayerController player;
        public PlayerController Player { get { return this.player; } }

        [SerializeField] private Transform viewTransform;
        public Transform ViewTransform { get { return this.viewTransform; } }

        [SerializeField] private ParticleSystem groundSlamParticlePrefab;
        public ParticleSystem GroundSlamParticlePrefab { get { return this.groundSlamParticlePrefab; } }

        [SerializeField] private Light glowLight;
        public Light GlowLight { get { return glowLight; } }



        [SerializeField] private EyensteinData data;
        public EyensteinData Data { get { return this.data; } }

        [SerializeField] private GameData gameData;
        public GameData GameData { get { return gameData; } }


        private EyensteinState State { get; set; }



        private bool IsHeightDone { get; set; }
        private float Height { get; set; }
        private float PreviousHeight { get; set; }
        private float HeightOrigin { get; set; }

        [SerializeField] private float heightTarget = 0;
        public float HeightTarget { get { return this.heightTarget; } private set { this.heightTarget = value; } }

        public bool IsVisible { get; private set; }
        public float IsVisibleTime { get; private set; }
        private float WaitTime { get; set; }


        public bool IsVisibleDirectly { get; private set; }
        public float IsVisibleDirectlyTime { get; private set; }

        private float DistanceToPlayer { get; set; }

        Vector3 PlayerViewDirection { get; set; }
        Vector3 CurrentViewDirection { get; set; }

        Vector3 DiveLocation { get; set; }
        Vector3 PreviousPosition { get; set; }

        float DiveCooldown { get; set; }

        private bool IsDiveReady { get => this.DiveCooldown <= 0; }


        private bool IsBuried { get => this.Height < 0 && this.IsHeightDone; }


        [SerializeField] private AudioSource audio;
        public AudioSource Audio { get { return audio; } }


        [SerializeField] private AudioClip groundSlamClip;
        public AudioClip GroundSlamClip { get { return groundSlamClip; } }

        float RageCooldown { get; set; } = 30;
        float RageTime { get; set; } = 10;
        float DistanceTime { get; set; }

        float RandomDistance { get; set; } = 60;

        private void Update()
        {
            this.PreviousHeight = this.Height;
            this.PreviousPosition = this.transform.position;

            this.UpdateHeight();
            this.CalculateStateInfo();
            this.CheckStates();

            this.GlowLight.intensity = this.Data.GlowIntensity * this.IsVisibleTime.ClampMin(1);

            this.UpdateStates();

            this.AttackPlayer();
        }

        private void AttackPlayer()
        {
            if (this.DistanceToPlayer <= 1 && (this.State == EyensteinState.Follow || this.State == EyensteinState.Chase))
            {
                Debug.Log("HIT");

                this.Player.Damage();
                this.SetState_Dive(this.transform.position);
            }
        }

        private void CalculateStateInfo()
        {
            this.PlayerViewDirection = this.Player.ViewController.Head.forward;
            this.CurrentViewDirection = this.ViewTransform.forward;

            this.CalculateStateInfo_Distance();
            this.CalculateStateInfo_Visibility();
            this.CalculateStateInfo_DirectVisibility();
            this.CalculateStateInfo_Dive();
            this.CalculateStateInfo_Wait();
            this.CalculateStateInfo_Rage();
        }

        private void CalculateStateInfo_Distance()
        {
            Vector3 delta = (this.Player.transform.position - this.transform.position);
            delta.y = 0;

            this.DistanceToPlayer = delta.magnitude;
        }

        private void CalculateStateInfo_Visibility()
        {
            if (this.DistanceToPlayer >= 20)
            {
                this.IsVisible = false;
                this.IsVisibleTime = 0;
                return;
            }

            Vector3 currentViewDirection = this.CurrentViewDirection;
            currentViewDirection.y = 0;
            Vector3 playerViewDirection = this.PlayerViewDirection;
            playerViewDirection.y = 0;

            float viewDot = Vector3.Dot(currentViewDirection, playerViewDirection);

            this.IsVisible = (viewDot <= -.8f);

            if (this.IsVisible)
            {
                this.IsVisibleTime += Time.deltaTime;
            }
            else
            {
                this.IsVisibleTime = 0;
            }
        }

        private void CalculateStateInfo_DirectVisibility()
        {
            if (this.DistanceToPlayer >= 50)
            {
                this.IsVisibleDirectly = false;
                this.IsVisibleDirectlyTime = 0;
                return;
            }

            float viewDot = Vector3.Dot(this.CurrentViewDirection, this.PlayerViewDirection);

            this.IsVisibleDirectly = (viewDot <= -.95f);
            
            if (this.IsVisibleDirectly)
            {
                this.IsVisibleDirectlyTime += Time.deltaTime;
            }
            else
            {
                this.IsVisibleDirectlyTime = 0;
            }
        }

        private void CalculateStateInfo_Dive()
        {
            this.DiveCooldown = (this.DiveCooldown -= Time.deltaTime).ClampMin(0);
        }
        
        private void CalculateStateInfo_Wait()
        {
            this.WaitTime = (this.WaitTime += Time.deltaTime);
        }

        private void CalculateStateInfo_Rage()
        {
            if (this.State != EyensteinState.Chase && this.State != EyensteinState.Inactive)
                this.RageCooldown = (this.RageCooldown - Time.deltaTime).ClampMin(0);
        }

        private void UpdateStates()
        {
            switch (this.State)
            {
                case EyensteinState.Inactive:
                    this.UpdateState_Inactive();
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
                case EyensteinState.Dive:
                    this.UpdateState_Dive();
                    break;
                default:
                    break;
            }
        }

        private void CheckStates()
        {
            switch (this.State)
            {
                case EyensteinState.Follow:
                    this.CheckSetState_Chase();
                    this.CheckSetState_Flee();
                    break;
                case EyensteinState.Chase:
                    break;
                case EyensteinState.Flee:
                    this.CheckSetState_Wait();
                    break;
                case EyensteinState.Wait:
                    this.CheckSetState_Chase();
                    this.CheckSetState_Dive();
                    break;
                case EyensteinState.Dive:
                    break;
                case EyensteinState.Inactive:
                default:
                    break;
            }
        }

        public void SetState_Inactive()
        {
            Debug.Log("State: Inactive");

            this.State = EyensteinState.Inactive;
        }

        private void UpdateState_Inactive()
        {
            this.SetHeight(100);
        }

        public void SetState_Follow()
        {
            Debug.Log("State: Follow");

            this.SetHeight(0);
            this.State = EyensteinState.Follow;
        }

        private void UpdateState_Follow()
        {
            if (this.DistanceToPlayer > this.RandomDistance)
            {
                Debug.Log("RANDOM");
                this.SetState_Dive(this.GetLocationAroundPlayer());
            }

            this.transform.position = Vector3.MoveTowards(this.transform.position, this.Player.transform.position, this.GetSpeed() * Time.deltaTime);
        }

        private void CheckSetState_Follow()
        {
            if (this.DistanceToPlayer > 60)
            {
                this.SetState_Follow();
                return;
            }
        }

        public void SetState_Chase()
        {
            Debug.Log("State: Chase");

            this.RageCooldown = 30;
            this.RageTime = 10;

            this.SetHeight(0);
            this.State = EyensteinState.Chase;
        }

        private void UpdateState_Chase()
        {
            this.RageTime = (this.RageTime - Time.deltaTime).ClampMin(0);

            if (this.RageTime <= 0)
            {
                this.WaitTime = 0;
                this.SetState_Dive(this.transform.position);
                return;
            }

            this.transform.position = Vector3.MoveTowards(this.transform.position, this.Player.transform.position, this.GetSpeed() * 2f * Time.deltaTime);
        }

        private void CheckSetState_Chase()
        {
            if (this.RageCooldown <= 0)
            {
                this.SetState_Chase();
                return;
            }
        }


        public void SetState_Flee()
        {
            Debug.Log("State: Flee");

            this.SetHeight(30);
            this.State = EyensteinState.Flee;
        }

        private void UpdateState_Flee()
        {
            Vector3 delta = this.Player.transform.position - this.transform.position;
            delta.y = 0;
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position - delta.normalized, this.GetSpeed() * Time.deltaTime * 4);
        }

        private void CheckSetState_Flee()
        {
            if (this.State == EyensteinState.Follow)
            {
                if (this.IsVisibleTime >= 3)
                {
                    this.SetState_Flee();
                    return;
                }
            }
        }

        public void SetState_Wait()
        {
            Debug.Log("State: Wait");

            this.IsVisibleTime = 0;
            this.IsVisibleDirectlyTime = 0;
            this.WaitTime = 0;
            this.State = EyensteinState.Wait;
        }

        private void UpdateState_Wait()
        {
            // Dive to player
            if (this.IsBuried && this.DistanceToPlayer > 60 && this.IsDiveReady)
            {
                this.SetState_Dive(this.GetLocationInfrontPlayer());
            }
            if (!this.IsBuried && this.DistanceToPlayer < 5 && this.IsDiveReady)
            {
                this.SetState_Dive(this.transform.position);
            }

            if (this.WaitTime > 15f)
                this.SetState_Follow();
        }

        private void CheckSetState_Wait()
        {
            if (this.State != EyensteinState.Flee)
                return;

            if (this.DistanceToPlayer > 60)
            {
                this.SetState_Wait();
                return;
            }
        }

        public void SetState_Dive(Vector3 location)
        {
            Debug.Log("State: Dive");

            this.State = EyensteinState.Dive;
            location.y = 0;
            this.DiveLocation = location;
            this.transform.position = this.DiveLocation;
            this.DiveCooldown = this.Data.DiveCooldownBase;

            if (this.Height >= 0)
            {
                Debug.Log("Down");
                this.SetHeight(-100);
            }
            else
            {
                this.SetHeight(20);
            }
        }

        private void UpdateState_Dive()
        {
            if (this.IsHeightDone)
            {
                if (this.IsBuried)
                    this.SetState_Wait();
                else
                    this.SetState_Follow();
            }
            
        }

        private void CheckSetState_Dive()
        {
            if (this.State != EyensteinState.Wait)
                return;

            if (this.Height < 1)
                return;

            if (this.DistanceToPlayer <= 50 &&
                this.IsVisibleDirectlyTime >= 2)
            {
                Vector3 diveLocation = this.transform.position;
                diveLocation.y = 0;

                this.SetState_Dive(diveLocation);
                return;
            }
        }


        private void UpdateHeight()
        {
            this.Height = this.Height.MoveTowards(this.HeightOrigin, this.HeightTarget, 1f / 2f * Time.deltaTime);

            bool swap =
                this.PreviousHeight >= 0 && this.Height < 0 ||
                this.PreviousHeight < 0 && this.Height >= 0;

            if (swap)
            {
                ParticleSystem particleSystem = GameObject.Instantiate(this.GroundSlamParticlePrefab);
                Vector3 position = this.transform.position;
                position.y = 0;

                this.PlayGroundSlam(particleSystem, position);
            }

            Vector3 offset = this.OffsetTransform.localPosition;
            offset.y = this.Height;
            this.OffsetTransform.localPosition = offset;


            if ((this.Height - this.HeightTarget).Abs() < .1f)
            {
                this.HeightOrigin = this.HeightTarget;
                this.IsHeightDone = true;
            }
            else
            {
                this.IsHeightDone = false;
            }
        }

        private void PlayGroundSlam(ParticleSystem particleSystem, Vector3 position)
        {
            this.Audio.PlayOneShot(this.GroundSlamClip);
            particleSystem.transform.position = position;
            particleSystem.Play();
        }

        public void SetHeight(float value)
        {
            this.HeightOrigin = this.Height;
            this.HeightTarget = value;
            this.IsHeightDone = false;
        }

        private Vector3 GetLocationInfrontPlayer()
        {
            float radius = this.Data.DiveRadiusBase;
            radius -= this.GameData.TargetCount * this.Data.DiveRadiusReductionPerTarget;

            Vector3 position = this.Player.transform.position;
            Vector3 direction = this.Player.ViewController.Transform.forward;

            return position + direction * radius;
        }
        
        private Vector3 GetLocationAroundPlayer()
        {
            float radius = this.Data.DiveRadiusBase;
            radius -= this.GameData.TargetCount * this.Data.DiveRadiusReductionPerTarget;

            Vector3 position = this.Player.transform.position;
            position.x += MathF.Sin(UnityEngine.Random.value * MathF.PI) * radius;
            position.z += MathF.Cos(UnityEngine.Random.value * MathF.PI) * radius;
            Vector3 direction = this.Player.ViewController.Transform.forward;

            return position + direction * radius;
        }


        private float GetSpeed()
        {
            return (this.Data.Speed - this.IsVisibleTime * this.Data.SlowWeight).ClampMin(1);
        }
    }

    public enum EyensteinState
    {
        Inactive,
        Follow,
        Chase,
        Flee,
        Wait, // Rest
        Dive
    }
}