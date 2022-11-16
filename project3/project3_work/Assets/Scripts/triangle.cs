using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangle
{
    public vertex v0;
    public vertex v1;
    public vertex v2;
    public edge e0;
    public edge e1;
    public edge e2;

    public triangle(vertex v0, vertex v1, vertex v2, edge e0, edge e1, edge e2)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.v2 = v2;
        this.e0 = e0;
        this.e1 = e1;
        this.e2 = e2;
    }

    public vertex GetOtherVertex(edge e)
    {
        if (!e.Has(v0)) return v0;
        else if (!e.Has(v1)) return v1;
        else return v2;
    }
}
