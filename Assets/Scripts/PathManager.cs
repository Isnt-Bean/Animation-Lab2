using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{

    public PathManager pm;
    public int currentPointIndex = 0;
    
    [HideInInspector]
    [SerializeField]
    public List<Waypoints> path; 
    public List<Waypoints> getPath()
    {
        if (path == null)
            path = new List<Waypoints>();

        return path;
    }

    public void CreateAddPoint()
    {
        Waypoints go = new Waypoints();
        path.Add(go);
    }

    public Waypoints GetNextTarget()
    {
        int nextPointIndex = (currentPointIndex + 1) % path.Count;
        currentPointIndex = nextPointIndex;
        return path[nextPointIndex];
    }
}
