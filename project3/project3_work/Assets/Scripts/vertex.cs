using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vertex
{
    public Vector3 position;
    public List<edge> edges;
    public List<triangle> triangles;
    public vertex updated;


    public vertex(Vector3 position)
    {
        this.position = position;
        this.edges = new List<edge>();
        this.triangles = new List<triangle>();
    }

    public void AddEdge(edge e)
    {
        edges.Add(e);
    }

    public void AddTriangle(triangle f)
    {
        triangles.Add(f);
    }

}

