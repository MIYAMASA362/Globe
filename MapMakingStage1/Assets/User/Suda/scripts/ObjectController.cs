using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    public GameObject StartUpObject;

    protected bool button;

	// initialization
	void Start ()
    {
        button = false;
	}
 
    public void OnBotton()
    {
        button = true;
    }
}
