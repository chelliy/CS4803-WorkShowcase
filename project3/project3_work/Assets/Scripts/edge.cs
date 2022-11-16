using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edge
{
    public vertex v0, v1;
    public List<triangle> triangles;
    public vertex newEdgePoint;

    public edge(vertex v0, vertex v1)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.triangles = new List<triangle>();
    }

    public void AddTriangle(triangle f)
    {
        triangles.Add(f);
    }

    public bool Has(vertex v)
    {
        return v == v0 || v == v1;
    }

    public vertex GetOtherVertex(vertex v)
    {
        if (v0 == v) 
            return v1;
        else 
            return v0;
    }
}
