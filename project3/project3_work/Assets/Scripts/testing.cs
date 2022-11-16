using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var filter = GetComponent<MeshFilter>();
        var source = filter.mesh;
        //var mesh = subdivision.Subdivide(subdivision.Weld(source, float.Epsilon, source.bounds.size.x), 1, false);
        var mesh = subdivision.Subdivide(source, 5);
        filter.sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
