using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connectPointProperties
{
    // Start is called before the first frame update

    //body2
    public Vector3[][] Body2legMatchPoint;

    private float body2Leg3RelateX = 1.5f;
    private float body2Leg3RelateY = -1.8f;
    private float body2Leg3RelateNZ = -3f;
    private float body2Leg3RelatePZ = 4f;

    private float body2Leg2RelateX = 2f;
    private float body2Leg2RelateZ = -2.7f;
    private float body2Leg2RelateY = -3f;

    private float body2Leg1RelateX = 1.8f;
    private float body2Leg1RelateY = -2.5f;
    private float body2Leg1RelateNZ = -3.5f;
    private float body2Leg1RelatePZ = 3.5f;

    //body1
    public Vector3[][] Body1legMatchPoint;

    private float body1Leg3RelateX = 1.7f;
    private float body1Leg3RelateY = -3.3f;
    private float body1Leg3RelateZ = 1.1f;

    private float body1Leg2RelateX = 2.6f;
    private float body1Leg2RelateY = -3f;
    private float body1Leg2RelateZ = -0.9f;

    private float body1Leg1RelateX = 2.3f;
    private float body1Leg1RelateY = -5f;
    private float body1Leg1RelateZ = 0.1f;

    //body3
    public Vector3[][] Body3legMatchPoint;

    private float body3Leg3RelateX = 2f;
    private float body3Leg3RelateY = -2.8f;
    private float body3Leg3RelateZ = -0.6f;

    private float body3Leg2RelateX = 3f;
    private float body3Leg2RelateY = -2.5f;
    private float body3Leg2RelateZ = -0.4f;

    private float body3Leg1RelateX = 2.3f;
    private float body3Leg1RelateY = -3f;
    private float body3Leg1RelateZ = -0.8f;

    private Vector3 body2Tail = new Vector3(0, -1.7f, 7.3f);
    private Vector3 body1Tail = new Vector3(0, -3.8f, 3.6f);
    private Vector3 body3Tail = new Vector3(0, -2.6f, 4.4f);

    public Vector3[] tailMatchingPoint = new Vector3[3];

    private Vector3 body2Head = new Vector3(0.6f, 5.8f, -5.6f);
    private Vector3 body1Head = new Vector3(0, 6.9f, -2.3f);
    private Vector3 body3Head = new Vector3(0.75f, 5.6f, -4f);

    public Vector3[] headMatchingPoint = new Vector3[3];


    public connectPointProperties()
    {

        tailMatchingPoint[0] = body1Tail;
        tailMatchingPoint[1] = body2Tail;
        tailMatchingPoint[2] = body3Tail;

        headMatchingPoint[0] = body1Head;
        headMatchingPoint[1] = body2Head;
        headMatchingPoint[2] = body3Head;

        //body2
        Body2legMatchPoint = new Vector3[3][];

        Body2legMatchPoint[0] = new Vector3[4];
        Body2legMatchPoint[1] = new Vector3[4];
        Body2legMatchPoint[2] = new Vector3[4];

        //body1
        Body1legMatchPoint = new Vector3[3][];

        Body1legMatchPoint[0] = new Vector3[2];
        Body1legMatchPoint[1] = new Vector3[2];
        Body1legMatchPoint[2] = new Vector3[3];

        //body3
        Body3legMatchPoint = new Vector3[3][];

        Body3legMatchPoint[0] = new Vector3[2];
        Body3legMatchPoint[1] = new Vector3[2];
        Body3legMatchPoint[2] = new Vector3[2];

        for (int i = 0; i < 3; i++) {
            if (i == 0)
            {
                //body2
                Body2legMatchPoint[i][0] = new Vector3(body2Leg1RelateX, body2Leg1RelateY, body2Leg1RelatePZ);
                Body2legMatchPoint[i][1] = new Vector3(-body2Leg1RelateX, body2Leg1RelateY, body2Leg1RelatePZ);
                Body2legMatchPoint[i][2] = new Vector3(body2Leg1RelateX, body2Leg1RelateY, body2Leg1RelateNZ);
                Body2legMatchPoint[i][3] = new Vector3(-body2Leg1RelateX, body2Leg1RelateY, body2Leg1RelateNZ);

                //body1
                Body1legMatchPoint[i][0] = new Vector3(body1Leg1RelateX, body3Leg1RelateY, body1Leg1RelateZ);
                Body1legMatchPoint[i][1] = new Vector3(-body1Leg1RelateX, body3Leg1RelateY, body1Leg1RelateZ);

                //body3
                Body3legMatchPoint[i][0] = new Vector3(body3Leg1RelateX, body1Leg1RelateY, body3Leg1RelateZ);
                Body3legMatchPoint[i][1] = new Vector3(-body3Leg1RelateX, body1Leg1RelateY, body3Leg1RelateZ);
            }
            else if (i == 1)
            {
                //body2
                Body2legMatchPoint[i][0] = new Vector3(body2Leg2RelateX, body2Leg2RelateY, body2Leg2RelateZ);
                Body2legMatchPoint[i][1] = new Vector3(-body2Leg2RelateX, body2Leg2RelateY, body2Leg2RelateZ);
                Body2legMatchPoint[i][2] = new Vector3(body2Leg2RelateX, body2Leg2RelateY, -body2Leg2RelateZ);
                Body2legMatchPoint[i][3] = new Vector3(-body2Leg2RelateX, body2Leg2RelateY, -body2Leg2RelateZ);

                //body1
                Body1legMatchPoint[i][0] = new Vector3(body1Leg2RelateX, body1Leg2RelateY, body1Leg2RelateZ);
                Body1legMatchPoint[i][1] = new Vector3(-body1Leg2RelateX, body1Leg2RelateY, body1Leg2RelateZ);

                //body3
                Body3legMatchPoint[i][0] = new Vector3(body3Leg2RelateX, body3Leg2RelateY, body3Leg2RelateZ);
                Body3legMatchPoint[i][1] = new Vector3(-body3Leg2RelateX, body3Leg2RelateY, body3Leg2RelateZ);
            }
            else {
                Body3legMatchPoint[i][0] = new Vector3(body3Leg3RelateX, body3Leg3RelateY, body3Leg3RelateZ);
                Body3legMatchPoint[i][1] = new Vector3(-body3Leg3RelateX, body3Leg3RelateY, body3Leg3RelateZ);

                //body2
                Body2legMatchPoint[i][0] = new Vector3(body2Leg3RelateX, body2Leg3RelateY, body2Leg3RelatePZ);
                Body2legMatchPoint[i][1] = new Vector3(-body2Leg3RelateX, body2Leg3RelateY, body2Leg3RelatePZ);
                Body2legMatchPoint[i][2] = new Vector3(body2Leg3RelateX, body2Leg3RelateY, body2Leg3RelateNZ);
                Body2legMatchPoint[i][3] = new Vector3(-body2Leg3RelateX, body2Leg3RelateY, body2Leg3RelateNZ);

                //body1
                Body1legMatchPoint[i][0] = new Vector3(body1Leg3RelateX, body1Leg3RelateY, body1Leg3RelateZ);
                Body1legMatchPoint[i][1] = new Vector3(-body1Leg3RelateX, body1Leg3RelateY, body1Leg3RelateZ);
                Body1legMatchPoint[i][2] = new Vector3(0, body1Leg3RelateY, -1.8f);
            }
        }
    }
}
