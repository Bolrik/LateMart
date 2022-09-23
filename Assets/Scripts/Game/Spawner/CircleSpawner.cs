using UnityEngine;

public class CircleSpawner : Spawner
{
    [SerializeField] private float radius;
    public float Radius { get { return radius; } }

    [SerializeField] private Vector2 offset;
    public Vector2 Offset { get { return offset; } }


    protected override void GetPoints(out Vector3[] points)
    {
        points = new Vector3[this.Spawns];

        float degreePerSpawn = 360f / this.Spawns;

        for (int i = 0; i < this.Spawns; i++)
        {
            float offset = Random.Range(this.Offset.x, this.Offset.y);
            float angle = i * degreePerSpawn;

            var x = Mathf.Cos(angle * Mathf.Deg2Rad) * (this.Radius + offset);
            var z = Mathf.Sin(angle * Mathf.Deg2Rad) * (this.Radius + offset);

            points[i] = new Vector3(x, 0, z);
        }
    }

    protected override void DrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.Radius);
    }
}