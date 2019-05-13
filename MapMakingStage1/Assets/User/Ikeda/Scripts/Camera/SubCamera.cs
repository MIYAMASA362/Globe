using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SubCamera : MonoBehaviour {

    [SerializeField]Camera MainCamera;

    Camera camera = null;

	// Use this for initialization
	void Start ()
    {
        camera = this.GetComponent<Camera>();
        camera.transform.SetParent(MainCamera.transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
        camera.fieldOfView = MainCamera.fieldOfView;
	}
}
