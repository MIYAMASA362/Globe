using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {

    private PlanetScene planetScene = null;

    private AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        planetScene = GameObject.Find("EventSystem").GetComponent<PlanetScene>();

        if (planetScene == null) Debug.LogError("PlanetScene.csが見つかりませんでした。PlanetScene.cs is not find");

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    //ゴールに触れたとき処理
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name== "Character")
        {
            //ゴールファンファーレ
            audioSource.Play();
            //ゴール後処理(シーン遷移)
            Debug.Log("GOOOOOOOOOOOOOOAL!!!!!!!!!!!!!!!");

            planetScene.GameClear();
        }
    }
}
