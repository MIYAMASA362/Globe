using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SubCamera : MonoBehaviour {

    Camera camera;

	// Use this for initialization
	void Start ()
    {
        camera = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
