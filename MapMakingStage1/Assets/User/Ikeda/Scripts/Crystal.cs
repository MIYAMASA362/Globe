using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public float RotSpeed = 5f;
    public PlanetScene planetScene;

	// Use this for initialization
	void Start ()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        this.transform.rotation *= Quaternion.AngleAxis(RotSpeed,Vector3.up);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {  
            planetScene.HitCrystal(other.gameObject);
        }
    }

}
