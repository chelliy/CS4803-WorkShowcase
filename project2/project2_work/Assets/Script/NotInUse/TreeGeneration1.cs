using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node : IEquatable<node> {
    public Vector3 position { get; set; }
    public List<_bud> buds { get; set; }

    public bool Equals(node other)
    {
        if (other == null) return false;
        return position.Equals(other.position);
    }
}

public class internode : IEquatable<internode>
{
    public node node1 { get; set; }
    public node node2 { get; set; }

    public bool Equals(internode other)
    {
        if (other == null) return false;
        return node1.position.Equals(other.node1.position) && node2.position.Equals(other.node2.position);
    }
}
public class _bud : IEquatable<_bud>
{
    public Vector3 direction { get; set; }
    public bool isPause { get; set; }
    public bool isDead { get; set; }
    public float growYear { get; set; }      
    public float order { get; set; } 
    public float dieProb { get; set; }      
    public float pauseProb { get; set; }    

    public bool Equals(_bud other)
    {
        if (other == null) return false;
        return direction.Equals(other.direction) && order == other.order;
    }
}



public class TreeGeneration1 : MonoBehaviour
{
    // Start is called before the first frame update

    private int numOfRing = 200;
    private Vector3 startVertex = new Vector3(0, 0, 0);
    private float deathRate = 0.1f;
    private float branchingRate = 1f;
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

    void treeGrowSimulation()
    {
        List<node> nodes = new List<node> { };
        List<internode> internodes = new List<internode> { };
        Vector3 currentVertex = new Vector3(startVertex.x, startVertex.y + trunkLength, startVertex.z);
        node startNode = new node()
        {
            position = currentVertex,
            buds = new List<_bud> { }
        };
        _bud startBud = new _bud
        {
            direction = new Vector3(0f, 1f, 0f),
            isPause = false,
            pauseProb = pauseProb,
            isDead = false,
            growYear = 0,
            order = 0,
            dieProb = deathRate
        };
        startNode.buds.Add(startBud);
        nodes.Add(startNode);



        for (int i = 1; i <= 4; i++)
        {
            node[] copy = nodes.ToArray();
            foreach (node node in copy)
            {
                _bud[] budcopy = node.buds.ToArray();
                node originalNode = nodes.Find(x => x.Equals(node));
                foreach (_bud bud in budcopy)
                {
                    _bud original = originalNode.buds.Find(x => x.Equals(bud));
                    if (bud.isDead == false)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) < bud.dieProb)
                        {
                            original.isDead = true;
                        }
                        else if (UnityEngine.Random.Range(0f, 1f) > bud.pauseProb)
                        {
                            node newNode = new node
                            {
                                position = node.position + bud.direction * branchLength,
                                buds = new List<_bud> { }
                            };

                            internode newInternode = new internode
                            {
                                node1 = originalNode,
                                node2 = newNode
                            };

                            if (Vector3.Angle(newInternode.node1.position, newInternode.node2.position) > 0f && Vector3.Angle(newInternode.node1.position, newInternode.node2.position) < 180f)
                            {
                                Vector3 newNodePosition = GenerateCurverdBranch(newInternode);
                                newNode.position = newNodePosition;
                            }
                            else
                            {
                                GenerateTrunkWithoutTop(newInternode);
                            }

                            _bud newBud = new _bud()
                            {
                                direction = bud.direction,
                                isPause = bud.isPause,
                                isDead = bud.isDead,
                                dieProb = bud.dieProb,
                                pauseProb = bud.pauseProb,
                                growYear = 0,
                                order = i

                            };
                            internodes.Add(newInternode);
                            newNode.buds.Add(newBud);
                            if (UnityEngine.Random.Range(0f, 1f) < branchingRate)
                            {
                                _bud newSideBud = new _bud()
                                {
                                    direction = (Quaternion.Euler(30f, 89f, 0) * bud.direction).normalized,
                                    isPause = bud.isPause,
                                    isDead = bud.isDead,
                                    dieProb = bud.dieProb,
                                    pauseProb = bud.pauseProb,
                                    growYear = 0,
                                    order = i

                                };
                                originalNode.buds.Add(newSideBud);

                                _bud newSideBud2 = new _bud()
                                {
                                    direction = (Quaternion.Euler(-30f, -89, 0) * bud.direction).normalized,
                                    isPause = bud.isPause,
                                    isDead = bud.isDead,
                                    dieProb = bud.dieProb,
                                    pauseProb = bud.pauseProb,
                                    growYear = 0,
                                    order = i

                                };
                                originalNode.buds.Add(newSideBud2);
                            }
                            nodes.Add(newNode);
                            print(originalNode.buds.Count);
                            originalNode.buds.Remove(original);
                            print(originalNode.buds.Count);
                        }
                        original.growYear++;
                    }
                }
            }
        }


        //foreach (internode internode in internodes)
        //{
        //    if (Vector3.Angle(internode.node1.position, internode.node2.position) > 0f && Vector3.Angle(internode.node1.position, internode.node2.position) < 180f)
        //    {
        //        GenerateCurverdBranch(internode);
        //    }
        //    else
        //    {
        //        GenerateTrunkWithoutTop(internode);
        //    }

        //}

    }




    GameObject GenerateTrunkWithoutTop(internode currentInternode)
    {
        GameObject s = new GameObject("test");
        s.AddComponent<MeshFilter>();
        s.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[2 * numOfRing];
        Vector2[] uv = new Vector2[2 * numOfRing];


        Vector3 position = currentInternode.node1.position;
        Vector3 postionUp = currentInternode.node2.position;

        Vector3 tangent = (postionUp - position).normalized;
        Vector3 normal = Vector3.Cross(Vector3.up, tangent);
        normal = normal.normalized;
        Vector3 start = new Vector3(0, 0, 1);


        int[] tris = new int[numOfRing * 2 * 3];

        for (int i = 0; i < numOfRing; i++)
        {
            Vector3 change = Quaternion.AngleAxis(-360f / numOfRing * i, tangent) * start;
            change = change.normalized;
            Vector3 newCurrent = position + change * trunkRadius;
            Vector3 newCurrentUp = postionUp + change * trunkRadius;
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

    GameObject GenerateBranch(internode currentInternode)
    {
        //currentBud.recalculateCurrent();
        GameObject s = new GameObject("Trunk");
        s.AddComponent<MeshFilter>();
        s.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[2 * numOfRing];
        Vector2[] uv = new Vector2[2 * numOfRing];


        Vector3 position = currentInternode.node1.position;
        Vector3 postionUp = currentInternode.node2.position;

        Vector3 tangent = (postionUp - position).normalized;
        Vector3 normal = Vector3.Cross(Vector3.up, tangent);
        normal = normal.normalized;
        Vector3 start = normal;


        int[] tris = new int[numOfRing * 2 * 3];

        for (int i = 0; i < numOfRing; i++)
        {

            Vector3 change = Quaternion.AngleAxis(-360f / numOfRing * i, tangent) * start;
            change = change.normalized;
            Vector3 newCurrent = position + change * branchRadius;
            Vector3 newCurrentUp = postionUp + change * branchRadius;

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
        renderer.material.color = new Color(165f / 255f, 42f / 255f, 42f / 255f);

        s.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        return s;
    }

    Vector3 GenerateCurverdBranch(internode currentInternode) {

        Vector3[] verts = new Vector3[2 * numOfRing];
        Vector2[] uv = new Vector2[2 * numOfRing];

        float piece = 200f;
        float k = 0.001f;


        Vector3 position = currentInternode.node1.position;
        Vector3 positionUp = currentInternode.node2.position;



        internode temp = new internode
        {
            node1 = new node { 
                position = position,
                buds = new List<_bud> { }
            },
            node2 = new node
            {
                position = positionUp,
                buds = new List<_bud> { }
            }
        };

        float length = (positionUp - position).magnitude;

        Vector3 tangent = (positionUp - position).normalized;
        Vector3 normal = Vector3.Cross(Vector3.up, tangent);

        temp.node2.position = Vector3.Lerp(position, positionUp, length / piece);

        for (int i = 0; i < piece; i++) {
            GenerateBranch(temp);

            position = positionUp;
            temp.node1.position = position;
            tangent = Vector3.Normalize(tangent + k*Vector3.up);
            positionUp = position + tangent * length / piece;
            temp.node2.position = positionUp;
        }

        return position;

    }

}

