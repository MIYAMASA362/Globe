using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPiece : CrystalBase
{
    [SerializeField] public StarPieceHandle handle;

    // Use this for initialization
    void Start ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        handle.HitStarPiece(this.gameObject);
        this.gameObject.SetActive(false);
    }

}
