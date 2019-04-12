using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DecreaseObject : MonoBehaviour {

    [Tag] public string Tag;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Tag);
	}
}
