using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour {
    //プレイヤーの取得
    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Character");
        player.transform.position = transform.position;
	}
}
