using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    //向くオブジェクトの変数
    public GameObject LookObject;

	// initialization
	void Start () {
		
	}
	
	// Update
	void Update () {
        transform.LookAt(LookObject.transform);
	}
}
