using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : CrystalBase
{
    private CrystalHandle handle = null;

	// Use this for initialization
	void Start ()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!handle)
        {
            Debug.Log("Not CrystalHandle!!");
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            if (!IsGet) IsGet = true;
            if (handle != null) handle.HitCrystal(this.gameObject);
        }
    }

    public void SetHandler(CrystalHandle handle)
    {
        this.handle = handle;
    }
}
