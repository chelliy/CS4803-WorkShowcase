using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGeneration : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 center = new Vector3(0, 0, 0);

    Color red = Color.red;
    Color blue = Color.blue;
    Color white = Color.white;
    Color black = Color.black;
    Color gray = Color.gray;

    public float heightVariation = 0.7f;
    public float roofProb = 0.3f;
    public Texture2D roofTexture;

    public int randomSeed = 0;

    private int previousRandomSeed = 0;

    private List<Vector2>[] footPrints;
    private List<GameObject> buildings = new List<GameObject>();
    public bool workShowCase = false;
    void Start()
    {
        Random.InitState(randomSeed);
        footPrintsLiberary();
        buildingGeneratePreProcess();
        //SomeMethod();
        //clean();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousRandomSeed != randomSeed) {
            Random.InitState(randomSeed);
            previousRandomSeed = randomSeed;
            print("detected");
            clean();
            buildingGeneratePreProcess();
        }
    }


    //public void SomeMethod()
    //{
    //    StartCoroutine(SomeCoroutine());
    //}

    //private IEnumerator SomeCoroutine()
    //{
    //    yield return new WaitForSecondsRealtime(3);
    //}

    public void readIntInput(string i) {
        randomSeed = int.Parse(i);
    }

    void clean() {
        int length = buildings.Count;
        for (int i = 0; i < length; i++) {
            Destroy(buildings[0]);
            buildings.RemoveAt(0);
        }
    }
    void footPrintsLiberary() {
        footPrints = new List<Vector2>[6];
        footPrints[0] =  new List<Vector2> { new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1) };

        footPrints[1] = new List<Vector2> { new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, -1) };

        footPrints[2] = new List<Vector2> { new Vector2(-2, 0), new Vector2(-1, 0), new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0) };

        footPrints[3] = new List<Vector2> { new Vector2(0, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(1, 0), new Vector2(1, 1) };

        footPrints[4] = new List<Vector2> { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(-1, -1) };

        footPrints[5] = new List<Vector2> { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(1, 1), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(-1, 1)};
    }

    void buildingGeneratePreProcess() {

        if (!workShowCase)
        {
            int first = Random.Range(0, 6);
            int second = Random.Range(0, 6);
            int third = Random.Range(0, 6);
            while (second == first)
            {
                second = Random.Range(0, 6);
            }
            while (third == first || third == second)
            {
                third = Random.Range(0, 6);
            }



            List<Vector2> footPrint = footPrintPreProcessing(new Vector2(0, 0), first);
            int[] height = heightGeneration(footPrint.Count, heightVariation);

            List<Vector2> footPrint2 = footPrintPreProcessing(new Vector2(5, 0), second);
            int[] height2 = heightGeneration(footPrint2.Count, heightVariation);

            List<Vector2> footPrint3 = footPrintPreProcessing(new Vector2(-5, 0), third);
            int[] height3 = heightGeneration(footPrint3.Count, heightVariation);

            buildingGenerate(footPrint, height);
            buildingGenerate(footPrint2, height2);
            buildingGenerate(footPrint3, height3);
        }
        else
        {

            List<Vector2> footPrint = footPrintPreProcessing(new Vector2(0, 0), 0);
            int[] height = heightGeneration(footPrint.Count, heightVariation);

            List<Vector2> footPrint2 = footPrintPreProcessing(new Vector2(5, 0), 1);
            int[] height2 = heightGeneration(footPrint2.Count, heightVariation);

            List<Vector2> footPrint3 = footPrintPreProcessing(new Vector2(10, 0), 2);
            int[] height3 = heightGeneration(footPrint3.Count, heightVariation);

            List<Vector2> footPrint4 = footPrintPreProcessing(new Vector2(15, 0), 3);
            int[] height4 = heightGeneration(footPrint4.Count, heightVariation);

            List<Vector2> footPrint5 = footPrintPreProcessing(new Vector2(20, 0), 4);
            int[] height5 = heightGeneration(footPrint5.Count, heightVariation);

            List<Vector2> footPrint6 = footPrintPreProcessing(new Vector2(25, 0), 5);
            int[] height6 = heightGeneration(footPrint6.Count, heightVariation);

            buildingGenerate(footPrint, height);
            buildingGenerate(footPrint2, height2);
            buildingGenerate(footPrint3, height3);
            buildingGenerate(footPrint4, height4);
            buildingGenerate(footPrint5, height5);
            buildingGenerate(footPrint6, height6);
        }
    }

    List<Vector2> footPrintPreProcessing(Vector2 center, int index) {
        List<Vector2> output = new List<Vector2>();
        foreach (Vector2 cell in footPrints[index]) {
            Vector2 processed = cell + center;
            output.Add(processed);
        }
        return output;
    }

    int[] heightGeneration(int length, float variation) {
        int[] result = new int[length];
        bool isRandom = Random.Range(0f, 1f) > variation;
        for (int i = 0; i < length; i++)
        {
            int height;
            if (isRandom)
            {
                height = 3 + Random.Range(0, 3);
            }
            else {
                height = 4;
            }
            result[i] = height;
        }
        return result;
    }

    void buildingGenerate(List<Vector2> footPrint, int[] height) {
        GameObject root = new GameObject("building");
        Vector2[] orginial = footPrint.ToArray();
        bool isDoor = false;
        bool isWindow = false;
        int windowType = Random.Range(0, 2);
        int doorType = Random.Range(0, 2);
        bool roof = Random.Range(0f, 1f) > roofProb;

        int count = 0;
        foreach (Vector2 center in orginial)
        {
            for (int i = 0; i < height[count]; i++)
            {
                Vector3 newCenter = new Vector3(center.x, i, center.y);
                if (i == 0)
                {
                    isDoor = true;
                    isWindow = false;
                }
                else
                {
                    isDoor = false;
                    isWindow = true;
                }
                XAxisFrontQuadGeneration(newCenter, isDoor, isWindow, root, doorType, windowType);
                XAxisBackQuadGeneration(newCenter, isWindow, root, windowType);
                ZAxisLeftQuadGeneration(newCenter, isWindow, root, windowType);
                ZAxisRightQuadGeneration(newCenter, isWindow, root, windowType);
                if (i == height[count] - 1)
                {
                    topQuadGeneration(newCenter, root);
                    if (roof) {
                        roofGeneration(footPrint, height, root);
                    }
                }
            }
            count++;
        }

        buildings.Add(root);

    }

    void roofGeneration(List<Vector2> footPrint, int[]height, GameObject root) {
        foreach (Vector2 point in footPrint)
        {
            Vector2 left = new Vector2(point.x - 1, point.y);
            Vector2 right = new Vector2(point.x + 1, point.y);
            Vector2 up = new Vector2(point.x, point.y + 1);
            Vector2 down = new Vector2(point.x, point.y - 1);

            bool leftcheck = footPrint.Contains(left);
            if (leftcheck) {
                leftcheck = height[footPrint.IndexOf(point)] == height[footPrint.IndexOf(left)];
            }

            bool upcheck = footPrint.Contains(up);
            if (upcheck)
            {
                upcheck = height[footPrint.IndexOf(point)] == height[footPrint.IndexOf(up)];
            }

            bool rightcheck = footPrint.Contains(right);
            if (rightcheck)
            {
                rightcheck = height[footPrint.IndexOf(point)] == height[footPrint.IndexOf(right)];
            }

            bool downcheck = footPrint.Contains(down);
            if (downcheck)
            {
                downcheck = height[footPrint.IndexOf(point)] == height[footPrint.IndexOf(down)];
            }

            Vector3 center = new Vector3(point.x, height[footPrint.IndexOf(point)] - 1,point.y);
            Vector3 highcenter = new Vector3(point.x, height[footPrint.IndexOf(point)],point.y);

            Vector3 leftBotIndex = new Vector3(center.x - 0.5f, center.y + 0.5f, center.z - 0.5f);
            Vector3 leftUpIndex = new Vector3(center.x - 0.5f, center.y + 0.5f, center.z + 0.5f);
            Vector3 rightBotIndex = new Vector3(center.x + 0.5f, center.y + 0.5f, center.z - 0.5f);
            Vector3 rightUpIndex = new Vector3(center.x + 0.5f, center.y + 0.5f, center.z + 0.5f);

            Vector3 leftMiddleIndex = Vector3.Lerp(leftBotIndex, leftUpIndex, 0.5f);
            Vector3 rightMiddleIndex = Vector3.Lerp(rightBotIndex, rightUpIndex, 0.5f);
            Vector3 upMiddleIndex = Vector3.Lerp(leftUpIndex, rightUpIndex, 0.5f) ;
            Vector3 downMiddleIndex = Vector3.Lerp(leftBotIndex, rightBotIndex, 0.5f) ;

            Vector3 leftHighMiddleIndex = leftMiddleIndex + new Vector3(0f,0.5f,0f);
            Vector3 rightHighMiddleIndex = rightMiddleIndex + new Vector3(0f, 0.5f, 0f);
            Vector3 upHighMiddleIndex = upMiddleIndex + new Vector3(0f, 0.5f, 0f);
            Vector3 downHighMiddleIndex = downMiddleIndex + new Vector3(0f, 0.5f, 0f);



            if (leftcheck && upcheck && rightcheck && downcheck)
            {
                //horizontal
                QuadGenerationVertexs(leftBotIndex, leftHighMiddleIndex, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);

                QuadGenerationVertexs(rightUpIndex, rightHighMiddleIndex, leftUpIndex, leftHighMiddleIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(leftUpIndex, leftHighMiddleIndex, leftBotIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(rightUpIndex, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);

                //vertical
                QuadGenerationVertexs(leftBotIndex, leftUpIndex, downHighMiddleIndex, upHighMiddleIndex, Color.magenta, root, roofTexture);

                QuadGenerationVertexs(downHighMiddleIndex, upHighMiddleIndex, rightBotIndex, rightUpIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(leftBotIndex, rightBotIndex, downHighMiddleIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(rightUpIndex, rightBotIndex, upHighMiddleIndex, Color.magenta, root, roofTexture);

            }
            else if (leftcheck && rightcheck)
            {
                //horizontal
                QuadGenerationVertexs(leftBotIndex, leftHighMiddleIndex, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);

                QuadGenerationVertexs(rightUpIndex, rightHighMiddleIndex, leftUpIndex, leftHighMiddleIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(leftUpIndex, leftHighMiddleIndex, leftBotIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(rightUpIndex, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);
                if (upcheck)
                {
                    QuadGenerationVertexs(leftMiddleIndex, leftUpIndex, highcenter, upHighMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(highcenter, upHighMiddleIndex, rightBotIndex, rightUpIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftUpIndex, rightUpIndex, upHighMiddleIndex, Color.magenta, root, roofTexture);
                }
                else if (downcheck)
                {
                    //down
                    QuadGenerationVertexs(leftBotIndex, leftMiddleIndex, downHighMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(downHighMiddleIndex, highcenter, rightBotIndex, rightMiddleIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightBotIndex, leftBotIndex, downHighMiddleIndex, Color.magenta, root, roofTexture);
                }
            }
            else if (upcheck && downcheck)
            {
                //vertical
                QuadGenerationVertexs(leftBotIndex, leftUpIndex, downHighMiddleIndex, upHighMiddleIndex, Color.magenta, root, roofTexture);

                QuadGenerationVertexs(downHighMiddleIndex, upHighMiddleIndex, rightBotIndex, rightUpIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(rightBotIndex,leftBotIndex, downHighMiddleIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(rightBotIndex,rightUpIndex, upHighMiddleIndex, Color.magenta, root, roofTexture);

                if (rightcheck)
                {
                    QuadGenerationVertexs(highcenter, upMiddleIndex, rightHighMiddleIndex, rightUpIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(downMiddleIndex, highcenter, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightUpIndex, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);
                }
                else if (leftcheck)
                {
                    QuadGenerationVertexs(leftHighMiddleIndex, leftUpIndex, highcenter, upMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(leftBotIndex, leftHighMiddleIndex, downMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftBotIndex, leftUpIndex, leftHighMiddleIndex, Color.magenta, root, roofTexture);
                }
            }
            else if (leftcheck)
            {
                //left
                QuadGenerationVertexs(leftHighMiddleIndex, leftUpIndex, highcenter, upMiddleIndex, Color.magenta, root, roofTexture);

                QuadGenerationVertexs(leftBotIndex, leftHighMiddleIndex, downMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(leftBotIndex, leftUpIndex, leftHighMiddleIndex, Color.magenta, root, roofTexture);

                if (upcheck)
                {
                    //up
                    QuadGenerationVertexs(leftMiddleIndex, leftUpIndex, highcenter, upHighMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(highcenter, upHighMiddleIndex, rightBotIndex, rightUpIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftUpIndex, rightUpIndex, upHighMiddleIndex, Color.magenta, root, roofTexture);

                    //remaining
                    triangleGenerationVertexs(downMiddleIndex, highcenter, rightBotIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightBotIndex, highcenter, rightMiddleIndex, Color.magenta, root, roofTexture);
                }
                else if (downcheck)
                {
                    //down
                    QuadGenerationVertexs(leftBotIndex, leftMiddleIndex, downHighMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(downHighMiddleIndex, highcenter, rightBotIndex, rightMiddleIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightBotIndex, leftBotIndex, downHighMiddleIndex, Color.magenta, root, roofTexture);

                    //remaining
                    triangleGenerationVertexs(rightUpIndex, rightMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightUpIndex, highcenter, upMiddleIndex, Color.magenta, root, roofTexture);
                }
                else {
                    //rightFix

                    QuadGenerationVertexs(downMiddleIndex, highcenter, rightBotIndex, rightMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(upMiddleIndex, rightUpIndex, highcenter,rightMiddleIndex,Color.magenta, root, roofTexture);
                }
            }
            else if (rightcheck)
            {
                //right
                QuadGenerationVertexs(highcenter, upMiddleIndex, rightHighMiddleIndex, rightUpIndex, Color.magenta, root, roofTexture);

                QuadGenerationVertexs(downMiddleIndex, highcenter, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(rightUpIndex, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);
                if (upcheck)
                {

                    //up
                    QuadGenerationVertexs(leftMiddleIndex, leftUpIndex, highcenter, upHighMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(highcenter, upHighMiddleIndex, rightBotIndex, rightUpIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftUpIndex, rightUpIndex, upHighMiddleIndex, Color.magenta, root, roofTexture);

                    //remaining
                    triangleGenerationVertexs(downMiddleIndex, leftBotIndex, highcenter, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftBotIndex, leftMiddleIndex, highcenter, Color.magenta, root, roofTexture);


                }
                else if (downcheck)
                {

                    //down
                    QuadGenerationVertexs(leftBotIndex, leftMiddleIndex, downHighMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(downHighMiddleIndex, highcenter, rightBotIndex, rightMiddleIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightBotIndex, leftBotIndex, downHighMiddleIndex, Color.magenta, root, roofTexture);

                    //remaining
                    triangleGenerationVertexs(leftMiddleIndex, leftUpIndex, highcenter, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftUpIndex, upMiddleIndex, highcenter, Color.magenta, root, roofTexture);


                }
                else
                {
                    //leftFix

                    QuadGenerationVertexs(leftMiddleIndex, highcenter, leftBotIndex, downMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(leftMiddleIndex, leftUpIndex, highcenter, upMiddleIndex, Color.magenta, root, roofTexture);
                }
            }
            else if (upcheck)
            {
                //up
                QuadGenerationVertexs(leftMiddleIndex, leftUpIndex, highcenter, upHighMiddleIndex, Color.magenta, root, roofTexture);

                QuadGenerationVertexs(highcenter, upHighMiddleIndex, rightMiddleIndex, rightUpIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(leftUpIndex, rightUpIndex, upHighMiddleIndex, Color.magenta, root, roofTexture);
                if (leftcheck)
                {
                    //left
                    QuadGenerationVertexs(leftHighMiddleIndex, leftUpIndex, highcenter, upMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(leftBotIndex, leftHighMiddleIndex, downMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftBotIndex, leftUpIndex, leftHighMiddleIndex, Color.magenta, root, roofTexture);

                    //remaining
                    triangleGenerationVertexs(downMiddleIndex, highcenter, rightBotIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightBotIndex, highcenter, rightMiddleIndex, Color.magenta, root, roofTexture);
                }
                else if (rightcheck)
                {
                    //right
                    QuadGenerationVertexs(highcenter, upMiddleIndex, rightHighMiddleIndex, rightUpIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(downMiddleIndex, highcenter, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightUpIndex, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);

                    //remaining
                    triangleGenerationVertexs(downMiddleIndex, leftBotIndex, highcenter, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftBotIndex, leftMiddleIndex, highcenter, Color.magenta, root, roofTexture);
                }
                else
                {
                    //down fix
                    QuadGenerationVertexs(leftMiddleIndex, highcenter, leftBotIndex, downMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(downMiddleIndex, highcenter, rightBotIndex, rightMiddleIndex, Color.magenta, root, roofTexture);
                }
            }
            else if (downcheck)
            {
                //down
                QuadGenerationVertexs(leftBotIndex, leftMiddleIndex, downHighMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                QuadGenerationVertexs(downHighMiddleIndex, highcenter, rightBotIndex, rightMiddleIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(rightBotIndex, leftBotIndex, downHighMiddleIndex, Color.magenta, root, roofTexture);
                if (leftcheck)
                {
                    //left
                    QuadGenerationVertexs(leftHighMiddleIndex, leftUpIndex, highcenter, upMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(leftBotIndex, leftHighMiddleIndex, downMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftBotIndex, leftUpIndex, leftHighMiddleIndex, Color.magenta, root, roofTexture);

                    //remaining
                    triangleGenerationVertexs(rightUpIndex, rightMiddleIndex, highcenter, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightUpIndex, highcenter, upMiddleIndex, Color.magenta, root, roofTexture);


                }
                else if (rightcheck)
                {
                    //right
                    QuadGenerationVertexs(highcenter, upMiddleIndex, rightHighMiddleIndex, rightUpIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(downMiddleIndex, highcenter, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(rightUpIndex, rightBotIndex, rightHighMiddleIndex, Color.magenta, root, roofTexture);


                    //remaining
                    triangleGenerationVertexs(leftMiddleIndex, leftUpIndex, highcenter, Color.magenta, root, roofTexture);

                    triangleGenerationVertexs(leftUpIndex, upMiddleIndex, highcenter, Color.magenta, root, roofTexture);


                }
                else
                {
                    //up fix
                    QuadGenerationVertexs(leftMiddleIndex, leftUpIndex, highcenter, upMiddleIndex, Color.magenta, root, roofTexture);

                    QuadGenerationVertexs(upMiddleIndex, rightUpIndex, highcenter, rightMiddleIndex, Color.magenta, root, roofTexture);
                }
            }
            else
                {
                triangleGenerationVertexs(leftBotIndex, highcenter, rightBotIndex, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(rightUpIndex, rightBotIndex, highcenter, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(leftUpIndex, rightUpIndex, highcenter, Color.magenta, root, roofTexture);

                triangleGenerationVertexs(leftBotIndex, leftUpIndex, highcenter, Color.magenta, root, roofTexture);
            }
        }
    }

    void doorGeneration(Vector3 leftBotIndex, Vector3 leftUpIndex, Vector3 rightBotIndex, Vector3 rightUpIndex, Color color, GameObject root, int type) {
        if (type == 1) {
            Vector3 doorleftBotIndex = Vector3.Lerp(leftBotIndex, rightBotIndex, 0.3f);
            Vector3 doorrightBotIndex = Vector3.Lerp(rightBotIndex, leftBotIndex, 0.3f);

            Vector3 leftNewAxis = Vector3.Lerp(leftBotIndex, leftUpIndex, 0.7f);
            Vector3 rightNewAxis = Vector3.Lerp(rightBotIndex, rightUpIndex, 0.7f);

            Vector3 doorleftUpIndex = Vector3.Lerp(leftNewAxis, rightNewAxis, 0.3f);
            Vector3 doorrightUpIndex = Vector3.Lerp(rightNewAxis, leftNewAxis, 0.3f);

            QuadGenerationVertexs(doorleftBotIndex, doorleftUpIndex, doorrightBotIndex, doorrightUpIndex, Color.yellow, root, null);

            QuadGenerationVertexs(leftBotIndex, leftNewAxis, doorleftBotIndex, doorleftUpIndex, color, root, null);

            QuadGenerationVertexs(doorrightBotIndex, doorrightUpIndex, rightBotIndex, rightNewAxis, color, root, null);

            QuadGenerationVertexs(leftNewAxis, leftUpIndex, rightNewAxis, rightUpIndex, color, root, null);
        }
        else {

            Vector3 leftNewAxis = Vector3.Lerp(leftBotIndex, leftUpIndex, 0.6f);
            Vector3 rightNewAxis = Vector3.Lerp(rightBotIndex, rightUpIndex, 0.6f);

            Vector3 leftdoorleftBotIndex = Vector3.Lerp(leftBotIndex, rightBotIndex, 0.1f);
            Vector3 leftdoorrightBotIndex = Vector3.Lerp(leftBotIndex, rightBotIndex, 0.45f);
            Vector3 leftdoorleftUpIndex = Vector3.Lerp(leftNewAxis, rightNewAxis, 0.1f);
            Vector3 leftdoorrightUpIndex = Vector3.Lerp(leftNewAxis, rightNewAxis, 0.45f);

            Vector3 rightdoorleftBotIndex = Vector3.Lerp(rightBotIndex, leftBotIndex, 0.45f);
            Vector3 rightdoorrightBotIndex = Vector3.Lerp(rightBotIndex, leftBotIndex, 0.1f);
            Vector3 rightdoorleftUpIndex = Vector3.Lerp(rightNewAxis, leftNewAxis, 0.45f);
            Vector3 rightdoorrightUpIndex = Vector3.Lerp(rightNewAxis, leftNewAxis, 0.1f);

            //leftDoor
            QuadGenerationVertexs(leftdoorleftBotIndex, leftdoorleftUpIndex, leftdoorrightBotIndex, leftdoorrightUpIndex, Color.yellow, root, null);
            //rightDoor
            QuadGenerationVertexs(rightdoorleftBotIndex, rightdoorleftUpIndex, rightdoorrightBotIndex, rightdoorrightUpIndex, Color.yellow, root, null);
            //left quad
            QuadGenerationVertexs(leftBotIndex, leftNewAxis, leftdoorleftBotIndex, leftdoorleftUpIndex, color, root, null);
            //right quad
            QuadGenerationVertexs(rightdoorrightBotIndex, rightdoorrightUpIndex, rightBotIndex, rightNewAxis, color, root, null);
            //topquad
            QuadGenerationVertexs(leftNewAxis, leftUpIndex, rightNewAxis, rightUpIndex, color, root, null);
            //center quad
            QuadGenerationVertexs(leftdoorrightBotIndex, leftdoorrightUpIndex, rightdoorleftBotIndex, rightdoorleftUpIndex, Color.black, root, null);
        }

    }


    void windowGeneration(Vector3 leftBotIndex, Vector3 leftUpIndex, Vector3 rightBotIndex, Vector3 rightUpIndex, Color color, GameObject root, int windowType)
    {
        if (windowType == 1)
        {
            Vector3 newleftBotIndex = Vector3.Lerp(leftBotIndex, rightBotIndex, 0.25f);
            Vector3 newrightBotIndex = Vector3.Lerp(rightBotIndex, leftBotIndex, 0.25f);
            Vector3 newleftUpIndex = Vector3.Lerp(leftUpIndex, rightUpIndex, 0.25f);
            Vector3 newrightUpIndex = Vector3.Lerp(rightUpIndex, leftUpIndex, 0.25f);

            Vector3 windowleftBotIndex = Vector3.Lerp(newleftBotIndex, newleftUpIndex, 0.25f);
            Vector3 windowleftUpIndex = Vector3.Lerp(newleftUpIndex, newleftBotIndex, 0.25f);
            Vector3 windowrightBotIndex = Vector3.Lerp(newrightBotIndex, newrightUpIndex, 0.25f);
            Vector3 windowrightUpIndex = Vector3.Lerp(newrightUpIndex, newrightBotIndex, 0.25f);

            QuadGenerationVertexs(windowleftBotIndex, windowleftUpIndex, windowrightBotIndex, windowrightUpIndex, Color.cyan, root, null);

            QuadGenerationVertexs(leftBotIndex, leftUpIndex, newleftBotIndex, newleftUpIndex, color, root, null);

            QuadGenerationVertexs(newrightBotIndex, newrightUpIndex, rightBotIndex, rightUpIndex, color, root, null);

            QuadGenerationVertexs(newleftBotIndex, windowleftBotIndex, newrightBotIndex, windowrightBotIndex, color, root, null);

            QuadGenerationVertexs(windowleftUpIndex, newleftUpIndex, windowrightUpIndex, newrightUpIndex, color, root, null);
        }
        else {

            Vector3 newLeftdownAxis = Vector3.Lerp(leftBotIndex, leftUpIndex, 0.1f);
            Vector3 newRightdownAxis = Vector3.Lerp(rightBotIndex, rightUpIndex, 0.1f);
            Vector3 newLeftupAxis = Vector3.Lerp(leftUpIndex, leftBotIndex, 0.3f);
            Vector3 newRightupAxis = Vector3.Lerp(rightUpIndex, rightBotIndex, 0.3f);

            Vector3 leftwindowleftBotIndex = Vector3.Lerp(newLeftdownAxis, newRightdownAxis, 0.2f);
            Vector3 leftwindowleftUpIndex = Vector3.Lerp(newLeftupAxis, newRightupAxis, 0.2f);
            Vector3 leftwindowrightBotIndex = Vector3.Lerp(newLeftdownAxis, newRightdownAxis, 0.55f);
            Vector3 leftwindowrightUpIndex = Vector3.Lerp(newLeftupAxis, newRightupAxis, 0.55f);


            Vector3 rightwindowrightUptIndex = Vector3.Lerp(newRightupAxis, newLeftupAxis, 0.2f);
            Vector3 rightwindowrightBottIndex = Vector3.Lerp(newRightdownAxis, newLeftdownAxis, 0.2f);
            Vector3 rightwindowLeftUptIndex = Vector3.Lerp(newRightupAxis, newLeftupAxis, 0.4f);
            Vector3 rightwindowrLeftBottIndex = Vector3.Lerp(newRightdownAxis, newLeftdownAxis, 0.4f);


            //left window
            QuadGenerationVertexs(leftwindowleftBotIndex, leftwindowleftUpIndex, leftwindowrightBotIndex, leftwindowrightUpIndex, Color.cyan, root, null);
            //right window
            QuadGenerationVertexs(rightwindowrLeftBottIndex, rightwindowLeftUptIndex, rightwindowrightBottIndex, rightwindowrightUptIndex, Color.cyan, root, null);
            //middle
            QuadGenerationVertexs(leftwindowrightBotIndex, leftwindowrightUpIndex, rightwindowrLeftBottIndex, rightwindowLeftUptIndex, Color.black, root, null);
            //fill
            QuadGenerationVertexs(leftBotIndex, newLeftdownAxis, rightBotIndex, newRightdownAxis, color, root, null);

            QuadGenerationVertexs(newLeftupAxis, leftUpIndex, newRightupAxis, rightUpIndex, color, root, null);

            QuadGenerationVertexs(newLeftdownAxis, newLeftupAxis, leftwindowleftBotIndex, leftwindowleftUpIndex, color, root, null);

            QuadGenerationVertexs(rightwindowrightBottIndex, rightwindowrightUptIndex, newRightdownAxis, newRightupAxis, color, root, null);
        }

    }


    void topQuadGeneration(Vector3 center, GameObject root)
    {

        Vector3 leftBotIndex = new Vector3(center.x - 0.5f, center.y + 0.5f, center.z - 0.5f);
        Vector3 leftUpIndex = new Vector3(center.x - 0.5f, center.y + 0.5f, center.z + 0.5f);
        Vector3 rightBotIndex = new Vector3(center.x + 0.5f, center.y + 0.5f, center.z - 0.5f);
        Vector3 rightUpIndex = new Vector3(center.x + 0.5f, center.y + 0.5f, center.z + 0.5f);

        QuadGenerationVertexs(leftBotIndex, leftUpIndex, rightBotIndex, rightUpIndex, gray, root, roofTexture);

    }


    void XAxisFrontQuadGeneration(Vector3 center,bool isDoor, bool isWindow, GameObject root, int doorType, int windowType) {


        Vector3 leftBotIndex = new Vector3(center.x - 0.5f, center.y - 0.5f, center.z - 0.5f);
        Vector3 leftUpIndex = new Vector3(center.x - 0.5f, center.y + 0.5f, center.z - 0.5f);
        Vector3 rightBotIndex = new Vector3(center.x + 0.5f, center.y - 0.5f, center.z - 0.5f);
        Vector3 rightUpIndex = new Vector3(center.x + 0.5f, center.y + 0.5f, center.z - 0.5f);
        if (!isDoor)
        {
            if (isWindow)
            {
                windowGeneration(leftBotIndex, leftUpIndex, rightBotIndex, rightUpIndex, white, root, windowType);
            }
        }
        else { 
            doorGeneration(leftBotIndex, leftUpIndex, rightBotIndex, rightUpIndex, white, root, doorType);
        }




    }
    void XAxisBackQuadGeneration(Vector3 center, bool isWindow, GameObject root, int windowType)
    {

        Vector3 leftBotIndex = new Vector3(center.x + 0.5f, center.y - 0.5f, center.z + 0.5f);
        Vector3 leftUpIndex = new Vector3(center.x + 0.5f, center.y + 0.5f, center.z + 0.5f);
        Vector3 rightBotIndex = new Vector3(center.x - 0.5f, center.y - 0.5f, center.z + 0.5f);
        Vector3 rightUpIndex = new Vector3(center.x - 0.5f, center.y + 0.5f, center.z + 0.5f);

        if (isWindow)
        {
            windowGeneration(leftBotIndex, leftUpIndex, rightBotIndex, rightUpIndex, blue, root, windowType);
        }
        else
        {
            QuadGenerationVertexs(leftBotIndex, leftUpIndex, rightBotIndex, rightUpIndex, blue, root, null);
        }
    }

    void ZAxisLeftQuadGeneration(Vector3 center, bool isWindow, GameObject root, int windowType)
    {

        Vector3 leftBotIndex = new Vector3(center.x - 0.5f, center.y - 0.5f, center.z + 0.5f);
        Vector3 leftUpIndex = new Vector3(center.x - 0.5f, center.y + 0.5f, center.z + 0.5f);
        Vector3 rightBotIndex = new Vector3(center.x - 0.5f, center.y - 0.5f, center.z - 0.5f);
        Vector3 rightUpIndex = new Vector3(center.x - 0.5f, center.y + 0.5f, center.z - 0.5f);

        if (isWindow)
        {
            windowGeneration(leftBotIndex, leftUpIndex, rightBotIndex, rightUpIndex, red, root, windowType);
        }
        else
        {
            QuadGenerationVertexs(leftBotIndex, leftUpIndex, rightBotIndex, rightUpIndex, red, root, null);
        }

    }
    void ZAxisRightQuadGeneration(Vector3 center, bool isWindow, GameObject root, int windowType)
    {

        Vector3 leftBotIndex = new Vector3(center.x + 0.5f, center.y - 0.5f, center.z - 0.5f);
        Vector3 leftUpIndex = new Vector3(center.x + 0.5f, center.y + 0.5f, center.z - 0.5f);
        Vector3 rightBotIndex = new Vector3(center.x + 0.5f, center.y - 0.5f, center.z + 0.5f);
        Vector3 rightUpIndex = new Vector3(center.x + 0.5f, center.y + 0.5f, center.z + 0.5f);

        if (isWindow)
        {
            windowGeneration(leftBotIndex, leftUpIndex, rightBotIndex, rightUpIndex, black, root, windowType);
        }
        else
        {
            QuadGenerationVertexs(leftBotIndex, leftUpIndex, rightBotIndex, rightUpIndex, black, root, null);
        }

    }

    void QuadGenerationVertexs(Vector3 leftBotIndex, Vector3 leftUpIndex, Vector3 rightBotIndex, Vector3 rightUpIndex, Color color, GameObject root, Texture2D premadeTexture)
    {
        GameObject s = new GameObject("Quad");
        s.AddComponent<MeshFilter>();
        s.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[4];
        Vector2[] uv = new Vector2[4];

        verts[0] = leftBotIndex;
        verts[1] = leftUpIndex;
        verts[2] = rightBotIndex;
        verts[3] = rightUpIndex;
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 0);
        uv[3] = new Vector2(1, 1);

        int[] tris = new int[6];

        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;

        tris[3] = 2;
        tris[4] = 1;
        tris[5] = 3;

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uv;  // save the uv texture coordinates 
        mesh.Optimize();

        mesh.RecalculateNormals();
        s.GetComponent<MeshFilter>().mesh = mesh;
        Renderer renderer = s.GetComponent<Renderer>();
        Texture2D texture = Texture2D.whiteTexture;
        if (premadeTexture)
        {
            texture = premadeTexture;
        }
        else
        {
            renderer.material.color = color;
        }
        renderer.material.mainTexture = texture;

        texture.Apply();
        s.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        s.transform.SetParent(root.transform);
    }

    void triangleGenerationVertexs(Vector3 leftBotIndex, Vector3 rightBotIndex, Vector3 upIndex, Color color, GameObject root, Texture2D premadeTexture) {
        GameObject s = new GameObject("Triamgle");
        s.AddComponent<MeshFilter>();
        s.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[3];
        Vector2[] uv = new Vector2[3];

        verts[0] = leftBotIndex;
        verts[1] = rightBotIndex;
        verts[2] = upIndex;

        uv[0] = new Vector2(0f, 0f);
        uv[1] = new Vector2(1f, 0f);
        uv[2] = new Vector2(0.5f, 0.5f);


        int[] tris = new int[3];

        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;


        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uv;  // save the uv texture coordinates 
        mesh.Optimize();

        mesh.RecalculateNormals();
        s.GetComponent<MeshFilter>().mesh = mesh;
        Renderer renderer = s.GetComponent<Renderer>();
        Texture2D texture = Texture2D.whiteTexture;
        if (premadeTexture)
        {
            texture = premadeTexture;
        }
        else { 
            renderer.material.color = color; 
        }
        renderer.material.mainTexture = texture;

        texture.Apply();
        s.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        s.transform.SetParent(root.transform);
    }
}
