using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Data", menuName = "Game/Data/Spawn/new Spawn Data")]
public class SpawnData : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }

    [SerializeField] private Vector2 scaleMinMax;
    public Vector2 ScaleMinMax { get { return scaleMinMax; } }

    [SerializeField] private MinMax<Vector3> rotation;
    public MinMax<Vector3> Rotation { get { return rotation; } }

    public Vector3 GetEulerAngles()
    {
        Vector3 toReturn = new Vector3();

        toReturn.x = this.Rotation.Min.x + Random.Range(0, this.Rotation.Max.x - this.Rotation.Min.x);
        toReturn.y = this.Rotation.Min.y + Random.Range(0, this.Rotation.Max.y - this.Rotation.Min.y);
        toReturn.z = this.Rotation.Min.z + Random.Range(0, this.Rotation.Max.z - this.Rotation.Min.z);

        return toReturn;
    }
}

[System.Serializable]
public class MinMax<Type>
{
    [SerializeField] private Type min;
    public Type Min { get { return min; } }

    [SerializeField] private Type max;
    public Type Max { get { return max; } }
}