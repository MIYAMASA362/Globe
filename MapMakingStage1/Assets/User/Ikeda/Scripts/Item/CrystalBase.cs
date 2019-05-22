using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBase : MonoBehaviour
{
    public float RotSpeed = 3f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	public virtual void Update ()
    {
        this.transform.rotation *= Quaternion.AngleAxis(RotSpeed, Vector3.up);
    }

}
