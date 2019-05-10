using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour {
    //プレイヤーの変数
    private GameObject player;

	// Use this for initialization
	void Awake () {
        //プレイヤーオブジェクトの取得
        player = GameObject.Find("Character");
        //プレイヤーをスタート位置に設置
        player.transform.position = transform.position;
        //プレイヤーのrotationを修正
        player.transform.up = (player.transform.position - RotationManager.Instance.planetTransform.position).normalized;

        FlagManager.Instance.flagTransform.up = transform.forward;

        //BGMスタート
        AudioManager.Instance.PlayBGM(AUDIO.BGM_STAGE1_SAMPLE);
        AudioManager.Instance.ChangeVolume(0.5f);
	}
}
