using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreation : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] bodys = new Transform[3];
    public Transform[] legs = new Transform[3];
    public Color[] colorTable = new Color[6];
    public Transform tail;
    public Transform head;

    public int randomSeed = 0;
    private int previousRandomSeed = 0;

    private GameObject root;
    void Start()
    {
        Random.InitState(randomSeed);
        generation();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousRandomSeed != randomSeed)
        {
            Random.InitState(randomSeed);
            previousRandomSeed = randomSeed;

            Destroy(root); 
            generation();
        }
    }

    public void readIntInput(string i)
    {
        randomSeed = int.Parse(i);
    }

    public void generation() {

        root = new GameObject("root");
        var basePosition = Vector3.zero;
        basePosition.x = -20;
        bool[,] checkTable = new bool[3, 3];

        System.Array.Clear(checkTable, 0, 9);

        for (int i = 0; i < 5; i++) {
            int body = Random.Range(1, 4);
            int leg = Random.Range(1, 4);
            int color = Random.Range(0, 6);
            while (checkTable[body - 1, leg - 1]) {
                body = Random.Range(1, 4);
                leg = Random.Range(1, 4);
            }
            checkTable[body - 1, leg - 1] = true;
            creatureBuild(body,leg, basePosition,color,root);
            basePosition.x = basePosition.x + 10f;
        }
    }

    public void creatureBuild(int body, int leg, Vector3 position, int color, GameObject root) {
        var bodyPrefab = bodys[body - 1];
        var legPrefab = legs[leg - 1];

        GameObject s = new GameObject("creature");

        var connectPointAccess = new connectPointProperties();

        var connectPointTable = connectPointAccess.Body1legMatchPoint;

        float bodyScale = Random.Range(1f, 1.5f); 

        if (body == 1)
        {
            connectPointTable = connectPointAccess.Body1legMatchPoint;
        }
        else if (body == 2)
        {

            connectPointTable = connectPointAccess.Body2legMatchPoint;
        }
        else {

            connectPointTable = connectPointAccess.Body3legMatchPoint;
        }

        var temp = Random.Range(1f, 1.5f);
        //tail
        Transform tailinstance = Object.Instantiate(tail, connectPointAccess.tailMatchingPoint[body - 1] * bodyScale, Quaternion.identity);
        tailinstance.localScale = tailinstance.localScale * temp;
        coloring(color, tailinstance);
        tailinstance.SetParent(s.transform);
        //head
        temp = Random.Range(1f, 1.5f);
        Transform headinstance = Object.Instantiate(head, connectPointAccess.headMatchingPoint[body - 1] * bodyScale, Quaternion.identity);
        headinstance.localScale = headinstance.localScale * temp;
        coloring(color, headinstance);
        headinstance.SetParent(s.transform);
        //body
        Transform bodyinstance = Object.Instantiate(bodyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        bodyinstance.localScale = bodyinstance.localScale * bodyScale;
        bodyinstance.SetParent(s.transform);
        bodyinstance.GetComponent<MeshRenderer>().material.color = colorTable[color];
        //legs
        temp = Random.Range(1f, 1.5f);
        for (int i = 0; i < connectPointTable[leg - 1].Length; i++)
        {
            Transform leginstance = Object.Instantiate(legPrefab, connectPointTable[leg - 1][i]*bodyScale, Quaternion.identity);
            leginstance.localScale = leginstance.localScale * temp;
            leginstance.SetParent(s.transform);
            coloring(color,leginstance);
        }

        s.transform.position = position;
        s.transform.SetParent(root.transform);
    }

    public void coloring(int color, Transform current) {
        if (current.GetComponent<MeshRenderer>() && !current.tag.Equals("keepColor")) {
            current.GetComponent<MeshRenderer>().material.color = colorTable[color];
        }
        if (current.childCount > 0) {
            for (int i = 0; i < current.childCount; i++) {
                coloring(color, current.GetChild(i));
            }
        
        }
    
    }
}
