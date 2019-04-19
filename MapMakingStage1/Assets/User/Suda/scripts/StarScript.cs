using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour {
    //プレイヤーの変数
    private GameObject player;

	// Use this for initialization
	void Start () {
        //プレイヤーオブジェクトの取得
        player = GameObject.Find("Character");
        //プレイヤーをスタート位置に設置
        player.transform.position = transform.position;
	}
}
