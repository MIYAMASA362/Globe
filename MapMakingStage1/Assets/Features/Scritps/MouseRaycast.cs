using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycast : MonoBehaviour {


	public Vector3 position;
	public LayerMask ignoreLayers;
	public Camera cam;
	public GameObject mouseFol;
	

	
	// Update is called once per frame
	void Update () {


		
     RaycastHit hit; 
     Ray ray = cam.ScreenPointToRay(Input.mousePosition); 
     if ( Physics.Raycast (ray,out hit,100.0f,ignoreLayers)) {
		 Debug.Log(hit.transform.name); 
		 position = hit.point;
		 mouseFol.transform.position = position;
    }
		
	}
}
