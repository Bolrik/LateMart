using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiralSpawner : Spawner
{
    [SerializeField] private float radius;
    public float Radius { get { return radius; } }

    [SerializeField] private float distance;
    public float Distance { get { return distance; } }

    [SerializeField] private float segmentDistance;
    public float SegmentDistance { get { return segmentDistance; } }

    [SerializeField] private Vector2 offset;
    public Vector2 Offset { get { return offset; } }


    protected override void GetPoints(out Vector3[] points)
    {
        points = new Vector3[this.Spawns];

        //float angle = this.Radius;

        //for (var i = 0; i < this.Spawns; i++)
        //{
        //    float offset = Random.Range(this.Offset.x, this.Offset.y);

        //    float radius = Mathf.Sqrt(i + 1);
        //    angle += Mathf.Asin(1 / radius) * this.Distance;
        //    // radius *= this.Distance;

        //    var x = Mathf.Cos(angle) * (radius + offset + this.Radius);
        //    var y = Mathf.Sin(angle) * (radius + offset + this.Radius);
        //    points[i] = new Vector3(x, 0, y);
        //}

        float baseRadius = 0;
        float angle = 0;

        for (var i = 0; i < this.Spawns; i++)
        {
            float offset = Random.Range(this.Offset.x, this.Offset.y);

            // radius = Mathf.Sqrt(i + 1 + this.Distance);
            baseRadius = Mathf.Sqrt(i + 1);

            angle += (Mathf.Asin(1 / baseRadius) * this.Distance);

            baseRadius *= this.SegmentDistance;
            baseRadius += this.Radius;
            baseRadius += offset;

            points[i] = new Vector3(
                Mathf.Cos(angle) * baseRadius,
                0,
                Mathf.Sin(angle) * baseRadius);
        }
    }


    protected override void DrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.Radius);
    }
}
