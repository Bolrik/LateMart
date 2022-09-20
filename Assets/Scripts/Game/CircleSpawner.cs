using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [SerializeField] private SpawnDataCollection[] spawnData;
    public SpawnDataCollection[] SpawnData { get { return spawnData; } }


    [SerializeField] private Vector2 radius;
    public Vector2 Radius { get { return radius; } }

    [SerializeField] private Vector2 circleDistance;
    public Vector2 CircleDistance { get { return circleDistance; } }

    [SerializeField] private Vector2 spawnOffset;
    public Vector2 SpawnOffset { get { return spawnOffset; } }

    [SerializeField] private float spawnStep;
    public float SpawnStep { get { return spawnStep; } }



    [SerializeField] private int spawns;
    public int Spawns { get { return spawns; } }



    // Start is called before the first frame update
    void Start()
    {
        float radius = this.Radius.x;

        float twoPI = (2 * Mathf.PI);

        float angle = 0;
        bool direction = true; // true :: Count Up



        for (int i = 0; i < this.Spawns; i++)
        {
            float offset = Random.Range(this.SpawnOffset.x, this.SpawnOffset.y);

            float x = Mathf.Cos(angle) * (radius * offset);
            float y = Mathf.Sin(angle) * (radius * offset);

            angle += this.SpawnStep;

            if (angle >= twoPI)
            {
                angle -= twoPI;

                if (direction)
                {
                    radius += Random.Range(this.CircleDistance.x, this.CircleDistance.y);
                    if (radius >= this.Radius.y)
                        direction = !direction;
                }
                else
                {
                    radius -= Random.Range(this.CircleDistance.x, this.CircleDistance.y);

                    if (radius <= this.Radius.x)
                        direction = !direction;
                }
            }

            if (Physics.Raycast(
                new Ray(
                    new Vector3(this.transform.position.x + x, 10, this.transform.position.z + y), Vector3.down), out RaycastHit hit, 20))
            {
                SpawnData data = this.GetRandomData(); // this.Data.SpawnDatas[Random.Range(0, this.Data.SpawnDatas.Length)];

                var obj = GameObject.Instantiate(data.Prefab);
                float scale = data.ScaleMinMax.y - data.ScaleMinMax.x;
                scale = data.ScaleMinMax.x + Random.value * scale;

                obj.transform.localScale = new Vector3(scale, scale, scale);
                obj.transform.position = hit.point;

            }
        }
    }

    SpawnData GetRandomData()
    {
        int collectionIndex = Random.Range(0, this.SpawnData.Length);

        int index = Random.Range(0, this.SpawnData[collectionIndex].SpawnData.Length);

        SpawnData spawnData = this.SpawnData[collectionIndex].SpawnData[index];

        return spawnData;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.Radius.x);
        Gizmos.DrawWireSphere(this.transform.position, this.Radius.y);
    }
}
