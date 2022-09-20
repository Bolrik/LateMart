using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Data", menuName = "Game/Data/Spawn/new Spawn Data")]
public class SpawnData : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }

    [SerializeField] private Vector2 scaleMinMax;
    public Vector2 ScaleMinMax { get { return scaleMinMax; } }
}
