using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

    // Use this for initialization
    void Awake () {
        
        
    }

    void Start()
    {
        if (PlanetScene.Instance.skipOpening)
        {
            GameObject player = GameObject.Find("Character");
            player.transform.position = this.transform.position;
            player.transform.rotation = this.transform.rotation;
        }

        GameObject.Destroy(this.gameObject);
    }
}
