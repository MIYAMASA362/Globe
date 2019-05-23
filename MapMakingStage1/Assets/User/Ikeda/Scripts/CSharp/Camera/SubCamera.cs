using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SubCamera : MonoBehaviour
{
    new Camera camera;

	// Use this for initialization
	void Start ()
    {
        camera = this.GetComponent<Camera>();
        transform.SetParent(Camera.main.transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
        camera.fieldOfView = Camera.main.fieldOfView;
	}
}
