using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPiece : CrystalBase
{
    private StarPieceHandle handle;

    // Use this for initialization
    void Start ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!handle)
        {
            Debug.Log("Not StarPieceHandle!!");
            return;
        }

        if (!other.CompareTag("Player")) return;
        handle.HitStarPiece(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void SetHandler(StarPieceHandle handle)
    {
        this.handle = handle;
    }
}
