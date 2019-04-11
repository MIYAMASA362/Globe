using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Fire : MonoBehaviour
{

    [SerializeField] float value = 0f;
    [SerializeField] string tag = "";

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	    	
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(tag))
        {
            Debug.Log("Stay!!!");
        }
    }

}
