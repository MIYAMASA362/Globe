using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace  {

	Mesh mesh;
	int resolution;
	Vector3 localUp;
	Vector3 axisA;
	Vector3 axisB;

/*	public TerrainFace(Mesh mesh,int resolution, Vector3 localUp)
	{
		this.mesh = mesh;
		this.resolution = resolution;
		this.localUp = localUp;

		axisA = new Vector3(localUp.y,localUp.z,localUp.x);
		axisB = Vector3.Cross(localUp,axisA);
	}

	public void ConstructMesh()
	{
		Vector3[] verticles = new Vector3[resolution * resolution];
		int[] triangles = new int[(resolution - 1) * (resolution -1) * 6];

		for(int y =0;y< resolution;y++)
		{
			for(int x = 0;x < resolution;x++){
				Vector2 percent = new Vector2(x,y) / ( resolution -1);
				Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + ( percent.y - .5f) * 2 

			}
		}
		/* */

	}


