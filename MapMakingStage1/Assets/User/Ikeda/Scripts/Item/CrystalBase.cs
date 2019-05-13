using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBase : MonoBehaviour
{
    public static float RotSpeed = 5f;

    //データを参照し、既に取得されてるかを保持
    public bool IsGet = false;

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
