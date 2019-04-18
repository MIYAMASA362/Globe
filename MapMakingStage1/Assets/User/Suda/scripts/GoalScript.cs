using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {
    //ゴールテキスト
    public GameObject goalObject;

	// Use this for initialization
	void Start () {
        goalObject.SetActive(false);
	}

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name== "Character")
        {
            Debug.Log("GOOOOOOOOOOOOOOAL!!!!!!!!!!!!!!!");
        }
    }
}
