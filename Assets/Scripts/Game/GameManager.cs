using Enemy;
using Input;
using Player;
using System;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputManager input;
        public InputManager Input { get { return input; } }

        [SerializeField] private SceneLoaderProxy sceneLoader;
        public SceneLoaderProxy SceneLoader { get { return sceneLoader; } }

        [SerializeField] private Eyenstein enemy;
        public Eyenstein Enemy { get { return enemy; } }

        [SerializeField] private PlayerController player;
        public PlayerController Player { get { return player; } }


        [SerializeField] private Spawner targetSpawner;
        public Spawner TargetSpawner { get { return targetSpawner; } }

        [SerializeField] private float targetClearanceRadius = 1.5f;
        public float TargetClearanceRadius { get { return targetClearanceRadius; } }

        [SerializeField] private LayerMask clearanceLayerMask;
        public LayerMask ClearanceLayerMask { get { return clearanceLayerMask; } }




        private void Start()
        {
            this.TargetSpawner.SetPointAction(new Action<Vector3>[]
            {
                this.RemoveVegetationAtPoint
            });

            GameObject[] targets = this.TargetSpawner.Spawn();
        }

        private void RemoveVegetationAtPoint(Vector3 point)
        {
            var overlaps = Physics.OverlapSphere(point, this.TargetClearanceRadius, this.ClearanceLayerMask, QueryTriggerInteraction.Collide);

            Debug.Log($"Look at {point}: {overlaps.Length}");

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
            if (this.Input.Back.IsPressed)
            {
                this.SceneLoader.Load(SceneType.MenuScene);
                return;
            }
        }
    }
}