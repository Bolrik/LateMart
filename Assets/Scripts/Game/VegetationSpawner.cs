using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationSpawner : MonoBehaviour
{
    [SerializeField] private VegetationSpawnerData data;
    public VegetationSpawnerData Data { get { return data; } }

    [SerializeField] private float radius;
    public float Radius { get { return radius; } }

    [SerializeField] private int spawns;
    public int Spawns { get { return spawns; } }


    // Start is called before the first frame update
    void Start()
    {
        float radius = this.Radius / 2f;

        for (int i = 0; i < this.Spawns; i++)
        {
            float x = Random.Range(-radius, radius), 
                y = Random.Range(-radius, radius);

            if (Physics.Raycast(
                new Ray(
                    new Vector3(this.transform.position.x + x, 10, this.transform.position.z + y), Vector3.down), out RaycastHit hit, 20))
            {
                SpawnData data = this.Data.SpawnDatas[Random.Range(0, this.Data.SpawnDatas.Length)];

                var obj = GameObject.Instantiate(data.Prefab);
                float scale = data.ScaleMinMax.y - data.ScaleMinMax.x;
                scale = data.ScaleMinMax.x + Random.value * scale;

                obj.transform.localScale = new Vector3(scale, scale, scale);
                obj.transform.position = hit.point;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(this.transform.position, Vector3.one * this.Radius);
    }
}
