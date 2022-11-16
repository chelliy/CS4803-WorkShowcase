// This sample code demonstrates how to create geometry "on demand" based on camera motion.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{

	int max_plane = -1;       // the number of planes that we've made
	float plane_size = 5.0f;  // size of the planes
	int regionCount = 0;
	private GameObject[][] terrainMap = new GameObject[1][];
	private int terrainCount = 0;
	private int xoffset = 20000;
	private int zoffset = 20000;

	public GameObject prefab;

    void Start()
	{

		// start with one plane
		terrainMap[0] = new GameObject[] { createMyTerrain(xoffset, zoffset, 0, 0, 0) };
	}

	// Move the camera, and maybe create a new plane
	void Update()
	{

		// get the horizontal and verticle controls (arrows, or WASD keys)
		float dx = Input.GetAxis("Horizontal");
		float dz = Input.GetAxis("Vertical");

		// sensitivity factors for translate and rotate
		float translate_factor = 0.3f;
		float rotate_factor = 5.0f;

		// move the camera based on the keyboard input
		if (Camera.current != null)
		{
			// translate forward or backwards
			Camera.current.transform.Translate(0, 0, dz * translate_factor);

			// rotate left or right
			Camera.current.transform.Rotate(0, dx * rotate_factor, 0);

		}

		// grab the main camera position
		Vector3 cam_pos = Camera.main.transform.position;
		//Debug.LogFormat ("x z: {0} {1}", cam_pos.x, cam_pos.z);

		// if the camera has moved far enough, create another plane
		if (cam_pos.z < -regionCount*85 + 20 + zoffset || cam_pos.z > (regionCount + 1)*85 - 20 + zoffset || cam_pos.x < -regionCount*85 + 20 + xoffset || cam_pos.x > (regionCount+1)*85 - 20 + zoffset)
		{
			regionCount++;
			newTerrainCreation();
			calculateNormalForSeam();
		}

	}

	private void calculateNormalForSeam() {
		int totalRowsColsCurrent = 2 * (regionCount) + 1;
		for (int i = 0; i < totalRowsColsCurrent; i++) {
			if (i == 0)
			{
				for (int j = 0; j < totalRowsColsCurrent; j++)
				{
					calculateNormalAboutUp(i, j);

					if (j + 1 < totalRowsColsCurrent)
					{
						calculateNormalAboutRight(i, j);

					}
				}
			}
			else if (i == totalRowsColsCurrent)
			{
				for (int j = 0; j < totalRowsColsCurrent; j++)
				{
					calculateNormalAboutUp(i - 1, j);

					if (j + 1 < totalRowsColsCurrent)
					{
						calculateNormalAboutRight(i, j);

					}
				}
			}
			else
			{
				calculateNormalAboutRight(i, 0);
				calculateNormalAboutRight(i, totalRowsColsCurrent - 2);
				if (i + 1 < totalRowsColsCurrent) {
					calculateNormalAboutUp(i, 0);
					calculateNormalAboutUp(i, totalRowsColsCurrent - 1);
				}

			}
			
		}
	}

    private void calculateNormalAboutUp(int i, int j)
    {
		GameObject current = terrainMap[i][j];
		GameObject currentUp = terrainMap[i + 1][j];

		Mesh currentMesh = current.GetComponentInChildren<MeshFilter>().mesh;
		Mesh currentUpMesh = currentUp.GetComponentInChildren<MeshFilter>().mesh;


		Vector3[] currentNormal = currentMesh.normals;
		Vector3[] currentUpNormalBottom = currentUpMesh.normals;

		for (int k = 0; k < 86; k++)
		{
			Vector3 newVector = currentNormal[86 * 85 + k] + currentUpNormalBottom[k];
			newVector = newVector / newVector.magnitude;
			currentNormal[86 * 85 + k] = newVector.normalized;
			currentUpNormalBottom[k] = newVector.normalized;
			//currentNormal[86 * 85 + k] = currentUpNormalBottom[k];

		}

		currentMesh.normals = currentNormal;
		currentUpMesh.normals = currentUpNormalBottom;
		current.GetComponentInChildren<MeshFilter>().mesh = currentMesh;
		currentUp.GetComponentInChildren<MeshFilter>().mesh = currentUpMesh;

	}

    private void calculateNormalAboutRight(int i, int j) {
		GameObject current = terrainMap[i][j];
		Mesh currentMesh = current.GetComponentInChildren<MeshFilter>().mesh;
		Vector3[] currentNormal = currentMesh.normals;


		GameObject currentRight = terrainMap[i][j + 1];
		Mesh currentRightMesh = currentRight.GetComponentInChildren<MeshFilter>().mesh;
		Vector3[] currentRightNormal = currentRightMesh.normals;

		for (int k = 0; k < 86; k++)
		{
			Vector3 newNormal = Vector3.Normalize(currentNormal[86 * (k + 1) - 1] + currentRightNormal[k * 86]);
			currentNormal[86 * (k + 1) - 1] = newNormal;
			currentRightNormal[k * 86] = newNormal;

			//currentNormal[86 * (k + 1) - 1] = currentRightNormal[k * 86];
		}

		currentMesh.normals = currentNormal;
		currentRightMesh.normals = currentRightNormal;
		current.GetComponentInChildren<MeshFilter>().mesh = currentMesh;
		currentRight.GetComponentInChildren<MeshFilter>().mesh = currentRightMesh;
	}

    private void newTerrainCreation()
    {
		int totalRowsColsBefore = 2 * (regionCount - 1) + 1;
		int totalRowsColsCurrent = 2 * (regionCount) + 1;
		GameObject[][] newTerrainMap = new GameObject[totalRowsColsCurrent][];

		

		for (int i = -regionCount; i <= regionCount; i++)
		{
			GameObject[] temp = new GameObject[totalRowsColsCurrent];
			if (i == -regionCount || i == regionCount)
			{
				for (int j = -regionCount; j <= regionCount; j++)
				{
					temp[j + regionCount] = createMyTerrain(i * 85 + xoffset, j * 85 + zoffset, 0, 0, 0);
				}
				newTerrainMap[i + regionCount] = temp;
			}
			else
			{
				temp[0] = createMyTerrain(i * 85 + xoffset, -regionCount * 85 + zoffset, 0, 0, 0);
				for (int z = 1; z < totalRowsColsBefore + 1; z++) {
					temp[z] = terrainMap[i + regionCount - 1][z - 1];
				}
				temp[totalRowsColsCurrent - 1] = createMyTerrain(i * 85 + xoffset, regionCount * 85 + zoffset, 0, 0, 0);
			}
			newTerrainMap[i + regionCount] = temp;
		}
		terrainMap = newTerrainMap;
	}

    private GameObject createMyTerrain(int xStart, int zStart, int posX, int posZ, int mode)
	{

		//float maxH = float.MinValue;
		//float minH = float.MaxValue;

		//terrainCount++;

		//Mesh fakeMesh = new Mesh();

		//int verticeRowOneMore = 88;
		//int verticeColumOneMore = 88;


		//// vertices of a cube
		//Vector3[] tempverts = new Vector3[verticeRowOneMore * verticeColumOneMore];
		//Vector2[] tempuv = new Vector2[verticeRowOneMore * verticeColumOneMore];
		//Color[] tempcolors = new Color[verticeRowOneMore * verticeColumOneMore];
		////Color[] colors = new Color[verticeRow * verticeColum*32];

		//int tempcount = 0;
		//// vertices for a quad
		//float step = 1f / verticeRowOneMore;
		//for (int i = zStart - 1; i < zStart + verticeRowOneMore - 1; i++)
		//{
		//	for (int j = xStart - 1; j < xStart + verticeColumOneMore - 1; j++)
		//	{
		//		//float x = scale * (j-xStart) / (float)(verticeRow);
		//		//float y = scale * (i-zStart) / (float)(verticeColum);
		//		float x = j / 30f;
		//		float y = i / 30f;
		//		float t1 = Mathf.PerlinNoise(x, y);
		//		float t2 = Mathf.PerlinNoise(2 * x, 2 * y);
		//		float t3 = Mathf.PerlinNoise(4 * x, 4 * y);
		//		float t4 = Mathf.PerlinNoise(8 * x, 8 * y);
		//		float finalY = 0;

		//		// different noise
		//		if (mode == 0)
		//		{
		//			finalY = 10f * (t1 + t2 / 2f + t3 / 4f + t4 / 8f);
		//			tempverts[tempcount] = new Vector3(j, finalY, i);
		//		}
		//		else if (mode == 1)
		//		{
		//			//finalY = 10 * (t1 + t5 / 2 + t6 / 4 + t7 / 8);
		//			//verts[count] = new Vector3(j, finalY, i);
		//			finalY = 10f * (t1 + t2 / 2f + t3 / 4f + t4 / 8f);
		//			tempverts[tempcount] = new Vector3(j, finalY, i);
		//		}
		//		else if (mode == 2)
		//		{
		//			//finalY = 5*(2*t5 + 4*t6 + 8*t7);
		//			//verts[count] = new Vector3(j, finalY, i);
		//			finalY = 10f * (t1 + t2 / 2f + t3 / 4f + t4 / 8f);
		//			tempverts[tempcount] = new Vector3(j, finalY, i);
		//		}

		//		//setting for position
		//		if (finalY > maxH)
		//		{
		//			maxH = finalY;
		//		}
		//		else if (finalY < minH)
		//		{
		//			minH = finalY;
		//		}
		//		tempuv[tempcount] = new Vector2((j - xStart) * step, (i - zStart) * step);


		//		//texture
		//		if (finalY > 10f)
		//		{
		//			tempcolors[tempcount] = new Color(1f, 1f, 1f);
		//			//for (int k = 0; k < 32; k++)
		//			//{
		//			//	colors[textureCount] = new Color(1f, 1f, 1f, 1.0f);
		//			//	textureCount++;
		//			//}
		//		}
		//		else if (finalY > 7f && finalY <= 10f)
		//		{
		//			tempcolors[tempcount] = new Color(150f / 255f, 75f / 255f, 0f);
		//		}
		//		else if (finalY > 5f && finalY <= 7f)
		//		{
		//			tempcolors[tempcount] = new Color(0f, 128f / 255f, 0f);
		//			//for (int k = 0; k < 32; k++)
		//			//{
		//			//	colors[textureCount] = new Color(150f / 255f, 75f / 255f, 0f, 1.0f);
		//			//	textureCount++;
		//			//}
		//		}
		//		else
		//		{
		//			tempcolors[tempcount] = new Color(0f, 0f, 1f);
		//			//for (int k = 0; k < 32; k++)
		//			//{
		//			//	colors[textureCount] = new Color(0f, 0f, 1f, 1.0f);
		//			//	textureCount++;
		//			//}
		//		}
		//		tempcount++;
		//	}
		//}

		//// two triangles for the face

		//int[] temptris = new int[(verticeRowOneMore - 1) * (verticeRowOneMore - 1) * 2 * 3];  // need 3 vertices per triangle
		//int newCount = 0;
		//for (int i = 0; i < verticeRowOneMore - 1; i++)
		//{
		//	for (int j = 0; j < verticeRowOneMore - 1; j++)
		//	{
		//		int leftBotIndex = i * (verticeRowOneMore) + j;
		//		int rightBotIndex = i * (verticeRowOneMore) + j + 1;
		//		int leftUpIndex = (i + 1) * (verticeRowOneMore) + j;
		//		int rightUpIndex = (i + 1) * (verticeRowOneMore) + j + 1;
		//		temptris[newCount] = leftBotIndex;
		//		newCount++;
		//		temptris[newCount] = leftUpIndex;
		//		newCount++;
		//		temptris[newCount] = rightBotIndex;
		//		newCount++;

		//		temptris[newCount] = rightBotIndex;
		//		newCount++;
		//		temptris[newCount] = leftUpIndex;
		//		newCount++;
		//		temptris[newCount] = rightUpIndex;
		//		newCount++;

		//	}
		//}

		//// save the vertices and triangles in the mesh object
		//fakeMesh.vertices = tempverts;
		//fakeMesh.triangles = temptris;
		//fakeMesh.uv = tempuv;  // save the uv texture coordinates 
		//fakeMesh.Optimize();

		//fakeMesh.RecalculateNormals();  // automatically calculate the vertex normals

		//Vector3[] tempnormal = fakeMesh.normals;



		//GameObject s = new GameObject("Grids" + terrainCount.ToString());
		//s.AddComponent<MeshFilter>();
		//s.AddComponent<MeshRenderer>();

		//Mesh mesh = new Mesh();

		//int verticeRow = 86;
		//int verticeColum = 86;

		//Texture2D texture = new Texture2D(verticeRow, verticeColum);
		//Renderer renderer = s.GetComponent<Renderer>();

		//// vertices of a cube
		//Vector3[] verts = new Vector3[verticeColum * verticeRow];
		//Vector2[] uv = new Vector2[verticeRow * verticeColum];
		//Color[] colors = new Color[verticeRow * verticeColum];
		//Vector3[] normals = new Vector3[verticeColum * verticeRow];

		//int count = 0;
		//// vertices for a quad
		//step = 1f / verticeRow;

		//for (int i = 1; i < verticeColumOneMore - 1; i++) {
		//	for (int j = 1; j < verticeColumOneMore - 1; j++) {
		//		verts[count] = tempverts[j + i * verticeColumOneMore];
		//		uv[count] = tempuv[j + i * verticeColumOneMore];
		//		colors[count] = tempcolors[j + i * verticeColumOneMore];
		//		normals[count] = tempnormal[j + i * verticeColumOneMore];
		//		count++;
		//	}
		//}




		//// two triangles for the face

		//int[] tris = new int[(verticeColum - 1) * (verticeRow - 1) * 2 * 3];  // need 3 vertices per triangle
		//newCount = 0;
		//for (int i = 0; i < verticeRow - 1; i++)
		//{
		//	for (int j = 0; j < verticeColum - 1; j++)
		//	{
		//		int leftBotIndex = i * (verticeRow) + j;
		//		int rightBotIndex = i * (verticeRow) + j + 1;
		//		int leftUpIndex = (i + 1) * (verticeRow) + j;
		//		int rightUpIndex = (i + 1) * (verticeRow) + j + 1;
		//		tris[newCount] = leftBotIndex;
		//		newCount++;
		//		tris[newCount] = leftUpIndex;
		//		newCount++;
		//		tris[newCount] = rightBotIndex;
		//		newCount++;

		//		tris[newCount] = rightBotIndex;
		//		newCount++;
		//		tris[newCount] = leftUpIndex;
		//		newCount++;
		//		tris[newCount] = rightUpIndex;
		//		newCount++;

		//	}
		//}

		//// save the vertices and triangles in the mesh object
		//mesh.vertices = verts;
		//mesh.triangles = tris;
		//mesh.uv = uv;  // save the uv texture coordinates 
		//mesh.normals = normals;
		//mesh.Optimize();



		////Mesh my_mesh = CreateMyMesh(x, z);

		//s.GetComponent<MeshFilter>().mesh = mesh;
		//texture.SetPixels(colors);
		//texture.Apply();
		//renderer.material.mainTexture = texture;

		//s.transform.Translate(posX, -5, posZ);

		//return s;



		GameObject s = new GameObject("Grids" + terrainCount.ToString());
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
		// vertices for a quad
		float step = 1f / verticeRow;
		for (int i = zStart; i < zStart + verticeRow; i++)
		{
			for (int j = xStart; j < xStart + verticeColum; j++)
			{
				//float x = scale * (j-xStart) / (float)(verticeRow);
				//float y = scale * (i-zStart) / (float)(verticeColum);
				float x = j / 30f;
				float y = i / 30f;
				float t1 = Mathf.PerlinNoise(x, y);
				float t2 = Mathf.PerlinNoise(2 * x, 2 * y);
				float t3 = Mathf.PerlinNoise(4 * x, 4 * y);
				float t4 = Mathf.PerlinNoise(8 * x, 8 * y);
				float finalY = 0;

				// different noise
				if (mode == 0)
				{
					finalY = 10f * (t1 + t2 / 2f + t3 / 4f + t4 / 8f);
					verts[count] = new Vector3(j, finalY, i);
				}
				else if (mode == 1)
				{
					//finalY = 10 * (t1 + t5 / 2 + t6 / 4 + t7 / 8);
					//verts[count] = new Vector3(j, finalY, i);
					finalY = 10f * (t1 + t2 / 2f + t3 / 4f + t4 / 8f);
					verts[count] = new Vector3(j, finalY, i);
				}
				else if (mode == 2)
				{
					//finalY = 5*(2*t5 + 4*t6 + 8*t7);
					//verts[count] = new Vector3(j, finalY, i);
					finalY = 10f * (t1 + t2 / 2f + t3 / 4f + t4 / 8f);
					verts[count] = new Vector3(j, finalY, i);
				}

				uv[count] = new Vector2((j - xStart) * step, (i - zStart) * step);


				//texture
				if (finalY > 10f)
				{
					colors[count] = new Color(1f, 1f, 1f);
				}
				else if (finalY > 7f && finalY <= 10f)
				{
					colors[count] = new Color(150f / 255f, 75f / 255f, 0f);
				}
				else if (finalY > 5f && finalY <= 7f)
				{
					colors[count] = new Color(0f, 128f / 255f, 0f);
				}
				else
				{
					colors[count] = new Color(0f, 0f, 1f);
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

		s.transform.Translate(posX, -5, posZ);

		for (int i = 0; i < 10; i++) {
			int x = UnityEngine.Random.Range(0, 86);
			int z = UnityEngine.Random.Range(0, 86);
			float check = verts[z * 86 + x][1];
			if (check > 5f && check <= 10f)
			{
				var position = new Vector3(xStart + x, check - 5,zStart + z);
				Instantiate(prefab, position, Quaternion.identity);
			}
		}

		return s;
	}
}
