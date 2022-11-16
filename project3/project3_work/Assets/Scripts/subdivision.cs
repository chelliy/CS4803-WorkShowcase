using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subdivision : MonoBehaviour
{
    public static Mesh Subdivide(Mesh source, int details = 1)
    {
        var model = new model(source);
        var divider = new subdivision();

        for (int i = 0; i < details; i++)
        {
            model = divider.Divide(model);
        }
        var mesh = model.ConnectAll();
        return mesh;
    }

    model Divide(model model)
    {
        var nmodel = new model();
        for (int i = 0, n = model.triangles.Count; i < n; i++)
        {
            var f = model.triangles[i];

            var ne0 = newPointGeneration(f.e0);
            var ne1 = newPointGeneration(f.e1);
            var ne2 = newPointGeneration(f.e2);

            var nv0 = updateOldVertexs(f.v0);
            var nv1 = updateOldVertexs(f.v1);
            var nv2 = updateOldVertexs(f.v2);

            nmodel.AddTriangle(nv0, ne0, ne2);
            nmodel.AddTriangle(ne0, nv1, ne1);
            nmodel.AddTriangle(ne0, ne1, ne2);
            nmodel.AddTriangle(ne2, ne1, nv2);
        }
        return nmodel;
    }

    public vertex newPointGeneration(edge e)
    {
        if (e.newEdgePoint != null) return e.newEdgePoint;

        if (e.triangles.Count < 2)
        {
            var newPointPosition = (e.v0.position + e.v1.position) * 0.5f;
            e.newEdgePoint = new vertex(newPointPosition);
        }
        else
        {
            const float connected = 3f / 8f;
            const float unconnected = 1f / 8f;
            var left = e.triangles[0].GetOtherVertex(e);
            var right = e.triangles[1].GetOtherVertex(e);
            e.newEdgePoint = new vertex((e.v0.position + e.v1.position) * connected + (left.position + right.position) * unconnected);
        }

        return e.newEdgePoint;
    }
    public vertex updateOldVertexs(vertex v)
    {
        if (v.updated != null) return v.updated;

        var adjancies = GetAdjancies(v);
        var n = adjancies.Length;
        if (n < 3)
        {
            // boundary case for vertex
            var e0 = v.edges[0].GetOtherVertex(v);
            var e1 = v.edges[1].GetOtherVertex(v);
            const float k0 = (3f / 4f);
            const float k1 = (1f / 8f);
            v.updated = new vertex(k0 * v.position + k1 * (e0.position + e1.position));
        }
        else
        {
            const float parameter = (3f / 8f);
            var alpha = (n == 3) ? (3f / 16f) : (1f / n) * parameter;

            var np = (1f - n * alpha) * v.position + alpha * GetSumOfVertexs(adjancies);

            v.updated = new vertex(np);
        }

        return v.updated;
    }
    public vertex[] GetAdjancies(vertex v)
    {
        var adjancies = new vertex[v.edges.Count];
        for (int i = 0, n = v.edges.Count; i < n; i++)
        {
            adjancies[i] = v.edges[i].GetOtherVertex(v);
        }
        return adjancies;
    }
    public Vector3 GetSumOfVertexs(vertex[] v)
    {
        var n = v.Length;
        Vector3 sum = new Vector3(0, 0, 0);
        for (int i = 0; i < n; i++)
        {
            var vertex = v[i];
            sum += vertex.position;
        }
        return sum;
    }
}
