using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Game/Data/Game/new Game Data")]
    public class GameData : ScriptableObject
    {
        [SerializeField] private int targetCount;
        public int TargetCount { get { return this.targetCount; } set { this.targetCount = value; } }

        [SerializeField] private int targetMax;
        public int TargetMax { get { return this.targetMax; } set { this.targetMax = value; } }

    }
}