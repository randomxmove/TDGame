using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] Points { get => points; }
    public WaypointType Type { get => waypointType; }

    [SerializeField] private WaypointType waypointType;

    private Transform[] points;

    private void Awake()
    {
        points = new Transform[transform.childCount];

        for (int i = 0; i < Points.Length; i++)
            Points[i] = transform.GetChild(i);
    }
}
