using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPlanet : MonoBehaviour {

    //[SerializeField]
    static float RotSpeed = 0.5f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.rotation = Quaternion.AngleAxis(RotSpeed,this.transform.up) * this.transform.rotation; 
	}
}
