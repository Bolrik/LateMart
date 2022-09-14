using UnityEngine;

[CreateAssetMenu(fileName = "VegetationSpawnerData", menuName = "Game/Data/Vegetation/new Vegetation Spawner Data")]
public class VegetationSpawnerData : ScriptableObject
{
    [SerializeField] private SpawnData[] spawnDatas;
    public SpawnData[] SpawnDatas { get { return spawnDatas; } }
}

[System.Serializable]
public class SpawnData
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }

    [SerializeField] private Vector2 scaleMinMax;
    public Vector2 ScaleMinMax { get { return scaleMinMax; } }
}
