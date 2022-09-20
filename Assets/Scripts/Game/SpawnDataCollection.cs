using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Data Collection", menuName = "Game/Data/Spawn/new Spawn Data Collection")]
public class SpawnDataCollection : ScriptableObject
{
    [SerializeField] private SpawnData[] spawnData;
    public SpawnData[] SpawnData { get { return spawnData; } }
}