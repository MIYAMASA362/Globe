using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

    public OpeningAnimationHandler openingHandler = null;

    // Use this for initialization
    void Awake () {
        
        if (openingHandler && openingHandler.gameObject.activeInHierarchy)
        {
            openingHandler.transform.position = RotationManager.Instance.planetTransform.position;
            openingHandler.transform.rotation = this.transform.rotation;
        }
        else
        {
            GameObject player = GameObject.Find("Character");
            player.transform.position = this.transform.position;
            player.transform.rotation = this.transform.rotation;
        }
    }

    void Start()
    {
        GameObject.Destroy(this.gameObject);
    }
}
