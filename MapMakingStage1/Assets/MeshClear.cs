using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshClear : MonoBehaviour {

    MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        if (PlanetScene.Instance.state == PlanetScene.STATE.RESULT)
        {
            meshRenderer.enabled = false;
        }
    }
}
