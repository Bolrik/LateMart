using Enemy;
using Game.Targets;
using Input;
using Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputManager input;
        public InputManager Input { get { return this.input; } }

        [SerializeField] private SceneLoaderProxy sceneLoader;
        public SceneLoaderProxy SceneLoader { get { return this.sceneLoader; } }

        [SerializeField] private Eyenstein enemy;
        public Eyenstein Enemy { get { return this.enemy; } }

        [SerializeField] private PlayerController player;
        public PlayerController Player { get { return this.player; } }

        [SerializeField] private Spawner targetSpawner;
        public Spawner TargetSpawner { get { return this.targetSpawner; } }

        [SerializeField] private float targetClearanceRadius = 1.5f;
        public float TargetClearanceRadius { get { return this.targetClearanceRadius; } }

        [SerializeField] private AudioClip target;
        public AudioClip Target { get { return target; } }


        List<Vector3> Checks { get; set; } = new List<Vector3>();
        bool IsTargetSpawned { get; set; }

        float FogValue { get; set; }
        float FogValueTo { get; set; }


        [SerializeField] private GameData data;
        public GameData Data { get { return this.data; } }


        private void Start()
        {
            this.Data.TargetCount = 0;

            this.TargetSpawner.SetPointAction(new Action<Vector3>[]
            {
                this.RemoveVegetationAtPoint
            });
        }

        private void SpawnTargets()
        {
            if (this.IsTargetSpawned)
                return;

            GameObject[] targets = this.TargetSpawner.Spawn();

            this.Data.TargetMax = targets.Length;

            foreach (var target in targets)
            {
                Target t = target.GetComponentInChildren<Target>();

                if (t == null)
                {
                    this.Data.TargetMax--;
                    continue;
                }

                t.OnCollected += this.Target_OnCollected;
            }

            this.IsTargetSpawned = true;
        }

        private void Target_OnCollected(Target target)
        {
            this.Player.Audio.PlayOneShot(this.Target);

            if (this.Data.TargetCount == 0)
            {
                this.Enemy.transform.position = Vector3.zero;
                this.Enemy.SetState_Follow();
            }

            this.Data.TargetCount++;

            this.Player.UIController.AddDialog(
                new DialogItem(
                    DialogCatalogue.Get(
                        SystemLanguage.English,
                        DialogCatalogue.DialogLine.Player_Target_Collected,
                        $"{this.Data.TargetCount}", $"{this.Data.TargetMax}"), 1f, .2f));

            this.UpdateFogTarget();
        }

        private void UpdateFogTarget()
        {
            float finalValue;
            this.FogValue = this.FogValueTo;


            if (this.Data.TargetCount >= this.Data.TargetMax)
            {
                finalValue = 0;
                this.Player.Win();
                GameObject.Destroy(this.Enemy.gameObject);
            }
            else
            {
                if (this.Data.TargetCount <= 0)
                    finalValue = 0;
                else
                {
                    finalValue = 0.002f;
                    for (int i = 0; i < this.Data.TargetCount; i++)
                    {
                        finalValue *= 2f;
                    }
                }
            }

            this.FogValueTo = finalValue;
        }

        private void UpdateFog()
        {
            if (this.FogValue == this.FogValueTo)
                return;

            this.FogValue = Mathf.MoveTowards(this.FogValue, this.FogValueTo, .001f * Time.deltaTime);
            RenderSettings.fogDensity = this.FogValue;
        }

        private void RemoveVegetationAtPoint(Vector3 point)
        {
            var overlaps = Physics.OverlapSphere(point, this.TargetClearanceRadius, Physics.AllLayers, QueryTriggerInteraction.Collide);
            this.Checks.Add(point);

            foreach (var item in overlaps)
            {
                if (item.GetComponent<VegetationObject>() is VegetationObject vegetationObject)
                {
                    vegetationObject.Remove();
                }
            }
        }

        private void Update()
        {
            this.SpawnTargets();

            if (this.Input.Back.IsPressed)
            {
                this.SceneLoader.Load(SceneType.MenuScene);
                return;
            }

            this.UpdateFog();
        }

        private void OnDrawGizmos()
        {
            foreach (var item in this.Checks)
            {
                Gizmos.DrawWireSphere(item, this.TargetClearanceRadius);
            }
        }
    }
}