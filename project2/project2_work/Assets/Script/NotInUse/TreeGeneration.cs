using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bud : IEquatable<bud>
{
    public Vector3 current { get; set; }
    public Vector3 pre { get; set; }
    public float length { get; set; }
    public float radius { get; set; }
    public float subBranchCount { get; set; }
    public float deathRate { get; set; }
    public float branchingRate { get; set; }
    public bool isTrunk { get; set; }
    public bool isDead { get; set; }
    public float growYear { get; set; }    
    public bud parent { get; set; }

    public bool Equals(bud other)
    {
        if (other == null) return false;
        return (current.Equals(other.current) && pre.Equals(other.pre) && growYear.Equals(other.growYear));
    }

    public Vector3 getT() {
        return Vector3.Normalize(current - pre);
    }

    public Vector3 getN() {
        return Vector3.Normalize(Vector3.Cross(getT(), Vector3.up));
    }

    public void recalculateCurrent() {
        //pre = parent.current;
        float rotationBaseParent = 360f / parent.subBranchCount * (growYear - parent.growYear);
        float rotationOutWard = 30 * (1 + UnityEngine.Random.Range(0f, 1f));
        Vector3 tangent = parent.getT();
        Vector3 normal = parent.getN();
        Vector3 nextPoint = parent.current + (Quaternion.AngleAxis(rotationBaseParent, tangent) * (Quaternion.AngleAxis(rotationOutWard, normal) * tangent)).normalized * length;
        current = nextPoint;

    }
}



public class TreeGeneration : MonoBehaviour
{
    // Start is called before the first frame update

    private int numOfRing = 200;
    private Vector3 startVertex = new Vector3(0, 0, 0);
    private float deathRate = 0.1f;
    private float branchingRate = 0.8f;
    private float trunkLength = 3f;
    private float trunkRadius = 0.25f;
    private float branchRadius = 0.1f;
    private float branchLength = 1f;
    private float pauseProb = 0.1f;


    void Start()
    {
        treeGrowSimulation();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void treeGrowSimulation() {
        List<bud> buds = new List<bud> { };
        Vector3 currentVertex = new Vector3(startVertex.x, startVertex.y + trunkLength, startVertex.z);
        bud startTrunk = new bud() {
            pre = startVertex,
            current = currentVertex,
            branchingRate = branchingRate,
            subBranchCount = 0,
            deathRate = deathRate,
            isTrunk = true,
            length = trunkLength,
            radius = trunkRadius,
            isDead = false,
            parent = null,
            growYear = 0
        };
        buds.Add(startTrunk);

     

        for (int i = 1; i <= 6; i++)
        {
            bud[] copy = buds.ToArray();
            foreach (bud element in copy)
            {
                bud original = buds.Find(x=> x.Equals(element));
                if (element.isDead == false)
                {
                    if (UnityEngine.Random.Range(0f, 1f) < element.deathRate)
                    {
                        original.isDead = true;
                    }
                    else if (UnityEngine.Random.Range(0f, 1f) > pauseProb)
                    {
                        bud newBud = new bud()
                        {
                            pre = element.current,
                            current = (element.current + element.getT() * element.length),
                            branchingRate = element.branchingRate - 0.05f,
                            subBranchCount = 0,
                            deathRate = element.deathRate + 0.05f,
                            isTrunk = element.isTrunk,
                            length = element.length,
                            radius = element.radius,
                            isDead = false,
                            growYear = i,
                            parent = original
                        };
                        buds.Add(newBud);
                        if (UnityEngine.Random.Range(0f, 1f) < element.branchingRate)
                        {
                            Vector3 nextPoint  = element.current + (Quaternion.AngleAxis(36f * (element.subBranchCount + UnityEngine.Random.Range(0f,1f)), element.getT()) * (Quaternion.AngleAxis(30 * (1 + UnityEngine.Random.Range(0f, 1f)), element.getN()) * element.getT())).normalized * element.length;
                            if (element.isTrunk) {
                                nextPoint = element.current + (Quaternion.Euler(0f, 36f*(element.subBranchCount + UnityEngine.Random.Range(0f, 1f)), 30f*(1+UnityEngine.Random.Range(0f,1f))) * element.getT()).normalized * element.length;
                            }
                            bud newBranch = new bud()
                            {
                                pre = element.current,
                                current = nextPoint,
                                branchingRate = element.branchingRate - 0.1f,
                                deathRate = element.deathRate + 0.05f,
                                subBranchCount = 0,
                                isTrunk = false,
                                length = branchLength,
                                radius = branchRadius,
                                isDead = false,
                                growYear = i,
                                parent = original
                            };
                            print(newBranch.getN());
                            print(newBranch.current);
                            print(newBranch.pre);
                            original.subBranchCount++;
                            buds.Add(newBranch);
                        }
                    }
                }
            }
        }

        foreach (bud element in buds) { 
            if (element.isTrunk)
            {
                GenerateTrunkWithoutTop(element);
            }
            else
            {
                GenerateBranch(element);
            }
        
        }


    }





    GameObject GenerateTrunkWithoutTop(bud currentBud)
    {
        GameObject s = new GameObject("test");
        s.AddComponent<MeshFilter>();
        s.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[2 * numOfRing];
        Vector2[] uv = new Vector2[2 * numOfRing];

        Vector3 position = currentBud.pre;
        Vector3 postionUp = currentBud.current;

        Vector3 tangent = (postionUp - position).normalized;
        Vector3 normal = Vector3.Cross(Vector3.up, tangent);
        normal = normal.normalized;
        Vector3 start = new Vector3(0,0,1);


        int[] tris = new int[numOfRing * 2 * 3];

        for (int i = 0; i < numOfRing; i++)
        {
            Vector3 change = Quaternion.AngleAxis(-360f / numOfRing * i, tangent) * start;
            change = change.normalized;
            Vector3 newCurrent = position + change * currentBud.radius;
            Vector3 newCurrentUp = postionUp + change *currentBud.radius;
            verts[i * 2] = newCurrent;
            verts[i * 2 + 1] = newCurrentUp;
            uv[i * 2] = new Vector2(1 / numOfRing * i, 0);
            uv[i * 2 + 1] = new Vector2(1 / numOfRing * i, 1);

        }




        int newCount = 0;
        for (int j = 0; j < numOfRing; j++)
        {
            int leftBotIndex = j * 2;
            int leftUpIndex = j * 2 + 1;
            int rightBotIndex = (j + 1) * 2;
            int rightUpIndex = (j + 1) * 2 + 1;

            if ((j + 1) * 2 > (numOfRing * 2 - 1))
            {
                rightBotIndex = 0;
                rightUpIndex = 1;
            }
            tris[newCount] = leftBotIndex;
            newCount++;
            tris[newCount] = leftUpIndex;
            newCount++;
            tris[newCount] = rightBotIndex;
            newCount++;

            tris[newCount] = rightBotIndex;
            newCount++;
            tris[newCount] = leftUpIndex;
            newCount++;
            tris[newCount] = rightUpIndex;
            newCount++;

        }

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uv;  // save the uv texture coordinates 
        mesh.Optimize();

        mesh.RecalculateNormals();
        s.GetComponent<MeshFilter>().mesh = mesh;

        Renderer renderer = s.GetComponent<Renderer>();
        Texture2D texture = Texture2D.whiteTexture;
        texture.Apply();
        renderer.material.mainTexture = texture;
        renderer.material.color = new Color(165f / 255f, 42f / 255f, 42f / 255f);

        s.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        return s;
    }

    GameObject GenerateBranch(bud currentBud)
    {
        //currentBud.recalculateCurrent();
        GameObject s = new GameObject("Trunk");
        s.AddComponent<MeshFilter>();
        s.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[2 * numOfRing];
        Vector2[] uv = new Vector2[2 * numOfRing];


        Vector3 position = currentBud.pre;
        Vector3 postionUp = currentBud.current;

        Vector3 tangent = (postionUp - position).normalized;
        Vector3 normal = Vector3.Cross(Vector3.up, tangent);
        normal = normal.normalized;
        Vector3 start = normal;


        int[] tris = new int[numOfRing * 2 * 3];

        for (int i = 0; i < numOfRing; i++)
        {            
            
            Vector3 change = Quaternion.AngleAxis(-360f / numOfRing * i, tangent) * start;
            change = change.normalized;
            Vector3 newCurrent = position + change * currentBud.radius;
            Vector3 newCurrentUp = postionUp + change * currentBud.radius;
            print(change);
            verts[i * 2] = newCurrent;
            verts[i * 2 + 1] = newCurrentUp;
            uv[i * 2] = new Vector2(1 / numOfRing * i, 0);
            uv[i * 2 + 1] = new Vector2(1 / numOfRing * i, 1);

        }




        int newCount = 0;
        for (int j = 0; j < numOfRing; j++)
        {
            int leftBotIndex = j * 2;
            int leftUpIndex = j * 2 + 1;
            int rightBotIndex = (j + 1) * 2;
            int rightUpIndex = (j + 1) * 2 + 1;

            if ((j + 1) * 2 > (numOfRing * 2 - 1))
            {
                rightBotIndex = 0;
                rightUpIndex = 1;
            }
            tris[newCount] = leftBotIndex;
            newCount++;
            tris[newCount] = leftUpIndex;
            newCount++;
            tris[newCount] = rightBotIndex;
            newCount++;

            tris[newCount] = rightBotIndex;
            newCount++;
            tris[newCount] = leftUpIndex;
            newCount++;
            tris[newCount] = rightUpIndex;
            newCount++;

        }

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uv;  // save the uv texture coordinates 
        mesh.Optimize();

        mesh.RecalculateNormals();
        s.GetComponent<MeshFilter>().mesh = mesh;

        Renderer renderer = s.GetComponent<Renderer>();
        Texture2D texture = Texture2D.whiteTexture;
        texture.Apply();
        renderer.material.color = new Color(165f/255f, 42f / 255f, 42f / 255f);

        s.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        return s;
    }
}
