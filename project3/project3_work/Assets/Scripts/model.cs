using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class model
{
    List<vertex> vertices;
    List<edge> edges;
    public List<triangle> triangles;

    public model()
    {
        this.vertices = new List<vertex>();
        this.edges = new List<edge>();
        this.triangles = new List<triangle>();
    }

    public model(Mesh source) : this()
    {
        Mesh originial = preProcess(source);
        var originalVertices = originial.vertices;
        for (int i = 0; i < originalVertices.Length; i++)
        {
            var point = new vertex(originalVertices[i]);
            this.vertices.Add(point);
        }

        var originalTriangles = originial.triangles;
        for (int i = 0; i < originalTriangles.Length; i += 3)
        {
            int i0 = originalTriangles[i];
            int i1 = originalTriangles[i + 1];
            int i2 = originalTriangles[i + 2];
            vertex v0 = this.vertices[i0];
            vertex v1 = this.vertices[i1];
            vertex v2 = this.vertices[i2];

            var e0 = GetEdge(v0, v1);
            var e1 = GetEdge(v1, v2);
            var e2 = GetEdge(v2, v0);
            var f = new triangle(v0, v1, v2, e0, e1, e2);

            this.triangles.Add(f);

            v0.AddTriangle(f); 
            v1.AddTriangle(f); 
            v2.AddTriangle(f);

            e0.AddTriangle(f);   
            e1.AddTriangle(f); 
            e2.AddTriangle(f);
        }
    }

    public Mesh preProcess(Mesh originial) {
        var originalVertices = originial.vertices;
        List<Vector3> newVertices = new List<Vector3>();
        var originalTriangles = originial.triangles;
        for (int i = 0; i < originalVertices.Length; i++)
        {
            if (!newVertices.Contains(originalVertices[i]))
            {
                newVertices.Add(originalVertices[i]);
            }
            else {
                var newIndex = newVertices.IndexOf(originalVertices[i]);
                for (int j = 0; j < originalTriangles.Length; j++) {
                    if (originalTriangles[j] == i) {
                        originalTriangles[j] = newIndex;
                    }
                }
            }
        }
        Mesh updated = new Mesh();
        updated.vertices = newVertices.ToArray();
        updated.triangles = originalTriangles;
        updated.RecalculateBounds();
        updated.RecalculateNormals();
        return updated;
    }
    public void AddTriangle(vertex v0, vertex v1, vertex v2)
    {
        if (!this.vertices.Contains(v0)) 
            this.vertices.Add(v0);

        if (!this.vertices.Contains(v1)) 
            this.vertices.Add(v1);

        if (!this.vertices.Contains(v2)) 
            this.vertices.Add(v2);

        var e0 = GetEdge(v0, v1);
        var e1 = GetEdge(v1, v2);
        var e2 = GetEdge(v2, v0);
        var f = new triangle(v0, v1, v2, e0, e1, e2);

        this.triangles.Add(f);
        v0.AddTriangle(f); 
        v1.AddTriangle(f); 
        v2.AddTriangle(f);

        e0.AddTriangle(f); 
        e1.AddTriangle(f); 
        e2.AddTriangle(f);
    }

    edge GetEdge(vertex v0, vertex v1)
    {
        var check = v0.edges.Find(e =>
        {
            return e.Has(v1);
        });

        if (check != null) 
            return check;

        var newEdge = new edge(v0, v1);
        this.edges.Add(newEdge);
        v0.AddEdge(newEdge);
        v1.AddEdge(newEdge);
        return newEdge;
    }

    public Mesh ConnectAll()
    {
        var mesh = new Mesh();
        var triangles = new int[this.triangles.Count * 3];
        mesh.vertices = vertices.Select(v => v.position).ToArray(); 
        for (int i = 0, n = this.triangles.Count; i < n; i++)
        {
            var f = this.triangles[i];

            triangles[i * 3] = this.vertices.IndexOf(f.v0);
            triangles[i * 3 + 1] = this.vertices.IndexOf(f.v1); 
            triangles[i * 3 + 2] = this.vertices.IndexOf(f.v2); 
        }
     

        mesh.indexFormat = mesh.vertexCount < 65535 ? IndexFormat.UInt16 : IndexFormat.UInt32;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        var offset = mesh.bounds.center;
        mesh.vertices = vertices.Select(v => v.position - offset).ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
