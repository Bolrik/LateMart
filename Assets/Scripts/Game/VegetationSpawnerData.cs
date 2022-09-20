using UnityEngine;

[CreateAssetMenu(fileName = "VegetationSpawnerData", menuName = "Game/Data/Vegetation/new Vegetation Spawner Data")]
public class VegetationSpawnerData : ScriptableObject
{
    [SerializeField] private SpawnData[] spawnDatas;
    public SpawnData[] SpawnDatas { get { return spawnDatas; } }
}

