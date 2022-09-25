using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Data/Player/new Player Data")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private float viewSensitivity = 1;
        public float ViewSensitivity { get { return viewSensitivity; } set { this.viewSensitivity = value; } }

        [SerializeField] private float brightness = 2;
        public float Brightness { get { return brightness; } set { this.brightness = value; } }

    }
}