using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Eyenstein Data", menuName = "Game/Data/Enemy/new Eyenstein Data")]
    public class EyensteinData : ScriptableObject
    {
        [SerializeField] private float speed;
        public float Speed { get { return this.speed; } }

        [SerializeField] private float slowWeight;
        public float SlowWeight { get { return slowWeight; } }


        [SerializeField] private float diveCooldownBase = 15f;
        public float DiveCooldownBase { get { return this.diveCooldownBase; } }

        [SerializeField] private float diveRadiusBase;
        public float DiveRadiusBase { get { return diveRadiusBase; } }

        [SerializeField] private float diveRadiusReductionPerTarget;
        public float DiveRadiusReductionPerTarget { get { return diveRadiusReductionPerTarget; } }


        [SerializeField] private float glowIntensity = 0.1f;
        public float GlowIntensity { get { return glowIntensity = 0.1f; } }


    }
}