using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreation : MonoBehaviour
{
    // Start is called before the first frame update
    public int coneLikeCount = 0;
    public int rectangleLikeCount = 0;
    void Start()
    {
        GameObject root = new GameObject("testing");
        Vector3 leftBotIndex = new Vector3(0, 0, 0);
        Vector3 rightBotIndex = new Vector3(0, 1, 1);
        Vector3 upIndex = new Vector3(1, 0, 0);
        for (int i = 0; i < 1; i++) {

            //rectangleLikeGeneration(5, new Vector3(i*6, 4, 0), new Vector3(0, 0, 0),new Vector3(8, 0, 0), new Vector3(0, 0, 6), new Vector3(8, 0, 6),new Vector3(1, 10, 0), new Vector3(5, 10, 0), new Vector3(1, 10, 5), new Vector3(5, 10, 5), "Body", 1);

            //rectangleLikeGeneration(5, new Vector3(i*6, 10, 0), new Vector3(0, 0, 0), new Vector3(6, 0, 0), new Vector3(0, 0, 12), new Vector3(6, 0, 12),new Vector3(0, 8, 0), new Vector3(6, 8, 0), new Vector3(0, 5, 12), new Vector3(6, 5, 12), "Body", 2);

            //rectangleLikeGeneration(5, new Vector3(i*6, 15, 0), new Vector3(0, 0, 0),new Vector3(8, 0, 0), new Vector3(0, 0, 8), new Vector3(8, 0, 8),new Vector3(0, 8, 0), new Vector3(8, 8, 0), new Vector3(0, 8, 8), new Vector3(8, 8, 8), "Body", 3);

            //rectangleLikeGeneration(5, new Vector3(i * 6, 20, 0),new Vector3(0, 0, 0), new Vector3(1.5f, 0, 0), new Vector3(0, 0, 1.5f), new Vector3(1.5f, 0, 1.5f),new Vector3(0, 8, 0), new Vector3(2.5f, 8, 0), new Vector3(0, 8, 2.5f), new Vector3(2.5f, 8, 2.5f),"Leg", 1);

            //rectangleLikeGeneration(5, new Vector3(i * 6, 20, 0),new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(0, 0, 2), new Vector3(2, 0, 2), new Vector3(0, 6, 2), new Vector3(4, 6, 2), new Vector3(0, 6, 6), new Vector3(4, 6, 6),"Leg2-", 1);            

            //rectangleLikeGeneration(5, new Vector3(i * 6, 20, 0),new Vector3(0, 0, 2), new Vector3(2, 0, 2), new Vector3(0, 0, 4), new Vector3(2, 0, 4),new Vector3(0, 4, 0), new Vector3(2, 4, 0), new Vector3(0, 4, 2), new Vector3(2, 4, 2),"Leg2-", 2);

            //coneLikeGeneration(5, new Vector3(i * 6, 30, 0), new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(1f, 0, Mathf.Sqrt(3)), new Vector3(1f, 5 , Mathf.Sqrt(3)/3), "finger", 0);

            //coneLikeGeneration(5, new Vector3(i * 2, 0, 0), new Vector3(0, 0, 0), new Vector3(6, 0, 0), new Vector3(3, 0, 3*Mathf.Sqrt(3)), new Vector3(3, 3, Mathf.Sqrt(3)), "foot", 0);

            //rectangleLikeGeneration(5, new Vector3(i * 6, 10, 0), new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(0, 0, 2), new Vector3(2, 0, 2), new Vector3(0, 3, 0), new Vector3(2, 3, 0), new Vector3(0, 3, 2), new Vector3(2, 3, 2), "leg3-", 1);

            //rectangleLikeGeneration(5, new Vector3(i * 6, 10, 0), new Vector3(0, 0, 0), new Vector3(0.5f, 0, 0), new Vector3(0, 0, 0.5f), new Vector3(0.5f, 0, 0.5f), new Vector3(0, 4, 0), new Vector3(1, 4, 0), new Vector3(0, 4, 1), new Vector3(1, 4, 1), "leg3", 2);

            //rectangleLikeGeneration(5, new Vector3(i * 6, 10, 0), new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(0, 0, 2), new Vector3(2, 0, 2),new Vector3(0.75f, 8, 0.75f), new Vector3(1.25f, 8, 0.75f), new Vector3(0.75f, 8, 1.25f), new Vector3(1.25f, 8, 1.25f), "test", 0);

            //rectangleLikeGeneration(5, new Vector3(i * 6, 10, 0), new Vector3(0, 0, 0), new Vector3(3, 0, 0), new Vector3(0, 0, 3), new Vector3(3, 0, 3),new Vector3(1, 5, 1), new Vector3(3, 5, 1), new Vector3(1, 5 ,3), new Vector3(3, 5, 3), "neck", 0);

            //rectangleLikeGeneration(5, new Vector3(i * 6, 10, 0), new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(0, 0, 5), new Vector3(2, 0, 5),new Vector3(0, 1.5f, 2), new Vector3(2, 1.5f, 2), new Vector3(0, 1.5f, 4), new Vector3(2, 1.5f, 4), "head", 0);

            //rectangleLikeGeneration(5, new Vector3(i * 6, 10, 0), new Vector3(0, 0, 0), new Vector3(1.5f, 0, 0), new Vector3(0, 0, 5), new Vector3(1.5f, 0, 5),new Vector3(0, 1, 4), new Vector3(1.5f, 1f, 4), new Vector3(0, 1.5f, 5), new Vector3(1.5f, 1.5f, 5), "head", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    GameObject rectangleLikeGeneration(int details, Vector3 position, Vector3 downLeftBot, Vector3 downRighttBot, Vector3 downLeftUp, Vector3 downRightUp,
        Vector3 topLeftBot, Vector3 topRighttBot, Vector3 topLeftUp, Vector3 topRightUp, string name, int count)
    {
        Vector3 v0 = downLeftBot;
        Vector3 v1 = downRighttBot;
        Vector3 v2 = downLeftUp;
        Vector3 v3 = downRightUp;

        Vector3 v4 = topLeftBot;
        Vector3 v5 = topRighttBot;
        Vector3 v6 = topLeftUp;
        Vector3 v7 = topRightUp;

        GameObject s = new GameObject("Triamgle");
        s.AddComponent<MeshFilter>();
        s.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[8];
        //Vector2[] uv = new Vector2[8];

        verts[0] = v0;
        verts[1] = v1;
        verts[2] = v2;
        verts[3] = v3;

        verts[4] = v4;
        verts[5] = v5;
        verts[6] = v6;
        verts[7] = v7;

        //uv[0] = new Vector2(0f, 0.5f);
        //uv[1] = new Vector2(0f, 0f);
        //uv[2] = new Vector2(1f, 0.5f);
        //uv[2] = new Vector2(0f, 1f);


        int[] tris = new int[36];


        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;

        tris[3] = 1;
        tris[4] = 3;
        tris[5] = 2;

        tris[6] = 0;
        tris[7] = 4;
        tris[8] = 1;

        tris[9] = 1;
        tris[10] = 4;
        tris[11] = 5;

        tris[12] = 4;
        tris[13] = 6;
        tris[14] = 5;

        tris[15] = 5;
        tris[16] = 6;
        tris[17] = 7;

        tris[18] = 2;
        tris[19] = 3;
        tris[20] = 6;

        tris[21] = 3;
        tris[22] = 7;
        tris[23] = 6;

        tris[24] = 2;
        tris[25] = 6;
        tris[26] = 0;

        tris[27] = 0;
        tris[28] = 6;
        tris[29] = 4;

        tris[30] = 1;
        tris[31] = 5;
        tris[32] = 3;

        tris[33] = 3;
        tris[34] = 5;
        tris[35] = 7;
    


        mesh.vertices = verts;
        mesh.triangles = tris;

        mesh.Optimize();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mesh = subdivision.Subdivide(mesh, details);


        mesh.Optimize();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        s.GetComponent<MeshFilter>().sharedMesh = mesh;

        Renderer renderer = s.GetComponent<Renderer>();
        Texture2D texture = Texture2D.whiteTexture;
        renderer.material.mainTexture = texture;
        texture.Apply();

        s.transform.position = position;

        UnityEditor.AssetDatabase.CreateAsset(mesh, "Assets/Mesh/" +name + count + ".asset");
        UnityEditor.AssetDatabase.SaveAssets();


        return s;
    }
    GameObject coneLikeGeneration(int details, Vector3 position, Vector3 baseLeft, Vector3 baseRight, Vector3 baseUp, Vector3 Up, string name, int count)
    {
        Vector3 v0 = baseLeft;
        Vector3 v1 = baseUp;
        Vector3 v2 = baseRight;
        Vector3 v3 = Up;

        GameObject s = new GameObject("Triamgle");
        s.AddComponent<MeshFilter>();
        s.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[4];
        Vector2[] uv = new Vector2[4];

        verts[0] = v0;
        verts[1] = v1;
        verts[2] = v2;
        verts[3] = v3;

        uv[0] = new Vector2(0f, 0.5f);
        uv[1] = new Vector2(0f, 0f);
        uv[2] = new Vector2(1f, 0.5f);
        uv[2] = new Vector2(0f, 1f);


        int[] tris = new int[12];

        tris[0] = 0;
        tris[1] = 2;
        tris[2] = 1;

        tris[3] = 0;
        tris[4] = 3;
        tris[5] = 2;

        tris[6] = 0;
        tris[7] = 1;
        tris[8] = 3;

        tris[9] = 3;
        tris[10] = 1;
        tris[11] = 2;


        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uv;  // save the uv texture coordinates 
        mesh.Optimize();

        mesh.RecalculateNormals();
        Renderer renderer = s.GetComponent<Renderer>();
        Texture2D texture = Texture2D.whiteTexture;
        renderer.material.mainTexture = texture;
        texture.Apply();

        mesh = subdivision.Subdivide(mesh, details);

        mesh.Optimize();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        s.GetComponent<MeshFilter>().mesh = mesh;
        


        s.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        s.transform.position = position;

        UnityEditor.AssetDatabase.CreateAsset(mesh, "Assets/Mesh/"+ name+ + count + ".asset");
        UnityEditor.AssetDatabase.SaveAssets();
        return s;
    }
} 
