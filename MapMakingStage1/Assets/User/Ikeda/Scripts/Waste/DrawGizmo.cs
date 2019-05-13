using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        OnDrawGizmos();
	}

    private void OnDrawGizmos()
    {

        var preColor = Gizmos.color;
        var preMatrix = Gizmos.matrix;
        Gizmos.color = Color.blue;
        Gizmos.matrix = transform.localToWorldMatrix;

        // 法線方向にラインを描画
        var mesh = GetComponent<MeshFilter>().sharedMesh;
        for (int i = 0; i < mesh.normals.Length; i++)
        {
            var from = mesh.vertices[i];
            var to = from + mesh.normals[i];
            Gizmos.DrawLine(from, to);
        }

        Gizmos.color = preColor;
        Gizmos.matrix = preMatrix;
    }
}
