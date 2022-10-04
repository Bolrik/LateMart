using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Targets
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private Transform rootTransform;
        public Transform RootTransform { get { return rootTransform; } }

        [SerializeField] private Collider[] triggers;
        public Collider[] Triggers { get { return triggers; } }


        public Action<Target> OnCollected { get; set; }

        public void Activate(PlayerController player)
        {
            this.OnCollected?.Invoke(this);
            foreach (var trigger in this.Triggers)
            {
                trigger.enabled = false;
            }

            GameObject.Destroy(this.RootTransform.gameObject);
        }
    }
}