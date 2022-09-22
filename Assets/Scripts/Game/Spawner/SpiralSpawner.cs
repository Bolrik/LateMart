using UnityEngine;

public class SpiralSpawner : MonoBehaviour
{
    [SerializeField] private SpawnDataCollection[] spawnData;
    public SpawnDataCollection[] SpawnData { get { return spawnData; } }


    [SerializeField] private float radius;
    public float Radius { get { return radius; } }

    [SerializeField] private float distance;
    public float Distance { get { return distance; } }

    [SerializeField] private Vector2 offset;
    public Vector2 Offset { get { return offset; } }

    [SerializeField] private int spawns;
    public int Spawns { get { return spawns; } }



    // Start is called before the first frame update
    void Start()
    {
        float radius = 0;
        float angle = this.Radius;
        float offset = 0;

        for (var i = 0; i < this.Spawns; i++)
        {
            offset = Random.Range(this.Offset.x, this.Offset.y);

            radius = Mathf.Sqrt(i + 1);
            angle += Mathf.Asin(1 / radius) * this.Distance;
            radius *= this.Distance;

            var x = Mathf.Cos(angle) * (radius + offset + this.Radius);
            var y = Mathf.Sin(angle) * (radius + offset + this.Radius);


            if (Physics.Raycast(
                new Ray(
                    new Vector3(this.transform.position.x + x, 10, this.transform.position.z + y), Vector3.down), out RaycastHit hit, 20))
            {
                SpawnData data = this.GetRandomData();

                var obj = GameObject.Instantiate(data.Prefab);
                float scale = data.ScaleMinMax.y - data.ScaleMinMax.x;
                scale = data.ScaleMinMax.x + Random.value * scale;

                obj.transform.localScale = new Vector3(scale, scale, scale);
                obj.transform.position = hit.point;

                obj.transform.localEulerAngles = data.GetEulerAngles();
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
        Gizmos.DrawWireSphere(this.transform.position, this.Radius);

        float radius = 0;
        float angle = this.Radius;
        float offset = 0;

        for (var i = 0; i < this.Spawns; i++)
        {
            offset = Random.Range(this.Offset.x, this.Offset.y);

            radius = Mathf.Sqrt(i + 1);
            angle += Mathf.Asin(1 / radius) * this.Distance;
            radius *= this.Distance;

            var x = Mathf.Cos(angle) * (radius + offset + this.Radius);
            var y = Mathf.Sin(angle) * (radius + offset + this.Radius);

            Gizmos.DrawWireSphere(new Vector3(x, this.transform.position.y, y), 1);
        }
    }
}
