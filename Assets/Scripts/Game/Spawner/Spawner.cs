using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] private Transform container;
    public Transform Container { get { return container; } }

    [SerializeField] private SpawnDataCollection[] spawnData;
    public SpawnDataCollection[] SpawnData { get { return spawnData; } }

    [SerializeField] private int spawns;
    public int Spawns { get { return spawns; } }



    [SerializeField] private bool spawnOnAwake;
    public bool SpawnOnAwake { get { return spawnOnAwake; } }

    private Action<Vector3>[] PointActions { get; set; } = new Action<Vector3>[0];


    private void Awake()
    {
        if (this.SpawnOnAwake)
            this.Spawn();
    }

    public void SetPointAction(params Action<Vector3>[] actions)
    {
        this.PointActions = actions ?? new Action<Vector3>[0];
    }

    public GameObject[] Spawn()
    {
        GameObject[] toReturn = new GameObject[this.Spawns];

        this.GetPoints(out Vector3[] points);

        int index = 0;

        foreach (var point in points)
        {
            foreach (var pointAction in this.PointActions)
            {
                pointAction?.Invoke(this.transform.position + point);
            }

            if (Physics.Raycast(
                new Ray(new Vector3(this.transform.position.x + point.x, 10, this.transform.position.z + point.z), Vector3.down), 
                out RaycastHit hit, 20))
            {
                SpawnData data = this.GetRandomData();

                var obj = GameObject.Instantiate(data.Prefab);
                toReturn[index] = obj;

                float scale = data.ScaleMinMax.y - data.ScaleMinMax.x;
                scale = data.ScaleMinMax.x + Random.value * scale;

                obj.transform.localScale = new Vector3(scale, scale, scale);
                obj.transform.position = hit.point;

                obj.transform.localEulerAngles = data.GetEulerAngles();

                if (this.Container != null)
                    obj.transform.SetParent(this.Container, true);

                index++;
            }
        }

        return toReturn;
    }

    protected abstract void GetPoints(out Vector3[] points);

    SpawnData GetRandomData()
    {
        int collectionIndex = Random.Range(0, this.SpawnData.Length);

        int index = Random.Range(0, this.SpawnData[collectionIndex].SpawnData.Length);

        SpawnData spawnData = this.SpawnData[collectionIndex].SpawnData[index];

        return spawnData;
    }

    private void OnDrawGizmosSelected()
    {
        this.DrawGizmos();

        this.GetPoints(out Vector3[] points);

        int i = 0;

        foreach (var point in points)
        {
            Gizmos.color = new Color((i / 56f).Loop(1), (i / 71f).Loop(1), (i / 18f).Loop(1));

            Gizmos.DrawWireSphere(
                new Vector3(
                    this.transform.position.x + point.x,
                    this.transform.position.y,
                    this.transform.position.z + point.z), 1);

            i++;
        }
    }

    protected virtual void DrawGizmos() { }
}
