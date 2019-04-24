using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {

    private PlanetScene planetScene = null;

	// Use this for initialization
	void Start ()
    {
        planetScene = GameObject.Find("EventSystem").GetComponent<PlanetScene>();

        if (planetScene == null) Debug.LogError("PlanetScene.csが見つかりませんでした。PlanetScene.cs is not find");
	}

    //ゴールに触れたとき処理
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name== "Character")
        {
            //ゴールSE
            //AudioManager.Instance.PlaySE(AUDIO.SE_FANFARE_SAMPLE);

            //ゴール後処理(シーン遷移)
            Debug.Log("GOOOOOOOOOOOOOOAL!!!!!!!!!!!!!!!");

            planetScene.GameClear();
        }
    }
}
