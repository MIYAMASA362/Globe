using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    [SerializeField] private GameObject hitObject1;
    [SerializeField] private GameObject hitObject2;
    private bool hit1 = false;
    private bool hit2 = false;
    public bool isTrigger = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hit1 && hit2) isTrigger = true;
        else isTrigger = false;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (hitObject1 == other.gameObject) hit1 = true;
        if (hitObject2 == other.gameObject) hit2 = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (hitObject1 == other.gameObject) hit1 = false;
        if (hitObject2 == other.gameObject) hit2 = false;
    }
}
