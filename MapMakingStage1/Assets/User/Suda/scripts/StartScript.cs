using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

    public Transform player;


    // Use this for initialization
    void Awake () {
        
        
    }

    void Start()
    {
        if (PlanetScene.Instance.skipOpening)
        {
            player.position = this.transform.position;
            player.rotation = this.transform.rotation;
        }

        GameObject.Destroy(this.gameObject);
    }
}
