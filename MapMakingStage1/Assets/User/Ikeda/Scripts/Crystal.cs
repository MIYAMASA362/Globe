using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public float RotSpeed = 5f;
    //データを参照し、既に取得されてるかを保持
    public bool IsGet = false;

    [HideInInspector]public CrystalHandle handle = null;

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
            this.gameObject.SetActive(false);
            //未取得であれば
            if(!IsGet) IsGet = true;
            if (handle == null) handle.HitCrystal(this.gameObject);
        }
    }

}
