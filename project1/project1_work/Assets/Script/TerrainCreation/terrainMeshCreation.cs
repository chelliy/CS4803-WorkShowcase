using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainMeshCreation : MonoBehaviour
{
	// Start is called before the first frame update
	public float scale = 20f;
    void Start()
    {

		GameObject origin = createMyTerrain(0,0,0,0,0);
		//GameObject left = createMyTerrain(-85,0,0 ,0, 1);
		//GameObject right = createMyTerrain(85, 0, 0, 0, 2);
		//GameObject leftLeft = createMyTerrain(-170, 0, 0, 0, 1);
		//s.transform.Rotate(90, 0, 130);

		int regionCount = 2;
		for (int i = -regionCount; i <= regionCount; i++) {
			if (i == -regionCount || i == regionCount)
			{
				for (int j = -regionCount; j <= regionCount; j++)
				{
					createMyTerrain(i * 85, j * 85, 0, 0, 0);
				}
			}
			else {
				createMyTerrain(i * 85, regionCount * 85, 0, 0, 0);
				createMyTerrain(i * 85,-regionCount * 85, 0, 0, 0);
			}
		}
	}

	private GameObject createMyTerrain(int xStart, int zStart, int posX, int posZ, int mode) {

		float maxH = float.MinValue;
		float minH = float.MaxValue;

		GameObject s = new GameObject("Grids");
		s.AddComponent<MeshFilter>();
		s.AddComponent<MeshRenderer>();

		Mesh mesh = new Mesh();

		int verticeRow = 86;
		int verticeColum = 86;

		Texture2D texture = new Texture2D(verticeRow, verticeColum);
		Renderer renderer = s.GetComponent<Renderer>();

		// vertices of a cube
		Vector3[] verts = new Vector3[verticeColum * verticeRow];
		Vector2[] uv = new Vector2[verticeRow * verticeColum];
		Color[] colors = new Color[verticeRow * verticeColum];
		//Color[] colors = new Color[verticeRow * verticeColum*32];

		int count = 0;
		int textureCount = 0;
		// vertices for a quad
		float step = 1f / verticeRow;
		for (int i = zStart; i < zStart + verticeRow; i++)
		{
			for (int j = xStart; j < xStart + verticeColum; j++)
			{
				//float x = scale * (j-xStart) / (float)(verticeRow);
				//float y = scale * (i-zStart) / (float)(verticeColum);
				float x = j;
				float y = i;
				float t1 = Mathf.PerlinNoise(x, y);
				float t2 = Mathf.PerlinNoise(2*x, 2*y);
				float t3 = Mathf.PerlinNoise(4*x, 4*y);
				//float t4 = Mathf.PerlinNoise(8*x, 8*y);
				//float t33 = Mathf.PerlinNoise(16 * x, 16 * y);
				//float t44 = Mathf.PerlinNoise(32 * x, 32 * y);
				//float t5 = Mathf.PerlinNoise(x/2, y/2);
				//float t6 = Mathf.PerlinNoise(x/4, y/4);
				//float t7 = Mathf.PerlinNoise(x/8, y/8);
				float finalY = 0;
				
				// different noise
				if (mode == 0)
				{
					finalY =  (t1 + t2 / 2 + t3 / 4);
					verts[count] = new Vector3(j, finalY, i);
				}
				else if (mode == 1) {
					//finalY = 10 * (t1 + t5 / 2 + t6 / 4 + t7 / 8);
					//verts[count] = new Vector3(j, finalY, i);
					finalY = (t1 + t2 / 2 + t3 / 4);
					verts[count] = new Vector3(j, finalY, i);
				}
				else if (mode == 2)
				{
					//finalY = 5*(2*t5 + 4*t6 + 8*t7);
					//verts[count] = new Vector3(j, finalY, i);
					finalY = (t1 + t2 / 2 + t3 / 4);
					verts[count] = new Vector3(j, finalY, i);
				}

				//setting for position
				if (finalY > maxH)
				{
					maxH = finalY;
				}
				else if (finalY < minH) {
					minH = finalY;
				}
				uv[count] = new Vector2((j-xStart) * step, (i-zStart) * step);


				//texture
				if (finalY > 10)
				{
					colors[count] = new Color(1f, 1f, 1f, 1.0f);
					//for (int k = 0; k < 32; k++)
					//{
					//	colors[textureCount] = new Color(1f, 1f, 1f, 1.0f);
					//	textureCount++;
					//}
				}
				else if (finalY > 7 && finalY <= 10) {
					colors[count] = new Color(150f / 255f, 75f / 255f, 0f, 1.0f);
				}
				else if (finalY > 5 && finalY <= 7)
				{
					colors[count] = new Color(0f, 128f / 255f, 0f, 1.0f);
					//for (int k = 0; k < 32; k++)
					//{
					//	colors[textureCount] = new Color(150f / 255f, 75f / 255f, 0f, 1.0f);
					//	textureCount++;
					//}
				}
				else
				{
					colors[count] = new Color(0f, 0f, 1f, 1.0f);
					//for (int k = 0; k < 32; k++)
					//{
					//	colors[textureCount] = new Color(0f, 0f, 1f, 1.0f);
					//	textureCount++;
					//}
				}
				count++;
			}
		}

		// two triangles for the face

		int[] tris = new int[(verticeColum - 1) * (verticeRow - 1) * 2 * 3];  // need 3 vertices per triangle
		int newCount = 0;
		for (int i = 0; i < verticeRow - 1; i++)
		{
			for (int j = 0; j < verticeColum - 1; j++)
			{
				int leftBotIndex = i * (verticeRow) + j;
				int rightBotIndex = i * (verticeRow) + j + 1;
				int leftUpIndex = (i + 1) * (verticeRow) + j;
				int rightUpIndex = (i + 1) * (verticeRow) + j + 1;
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
		}

		// save the vertices and triangles in the mesh object
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.uv = uv;  // save the uv texture coordinates 
		mesh.Optimize();

		mesh.RecalculateNormals();  // automatically calculate the vertex normals


		//Mesh my_mesh = CreateMyMesh(x, z);

		s.GetComponent<MeshFilter>().mesh = mesh;
		texture.SetPixels(colors);
		texture.Apply();
		renderer.material.mainTexture = texture;

		float midH = minH + (maxH - minH)/2;

		s.transform.Translate(posX, -5, posZ);

		return s;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
