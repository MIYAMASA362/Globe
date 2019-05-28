﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GoalScript : MonoBehaviour {

    private PlanetScene planetScene;
    [SerializeField] private GameObject ShipFire = null;

    //ファンファーレのAudio
    private AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        ShipFire.SetActive(false);

        if (planetScene == null) planetScene = PlanetScene.Instance;

        if (planetScene == null) Debug.LogError("PlanetScene.csが見つかりませんでした。PlanetScene.cs is not find");

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    //ゴールに触れたとき処理
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name== "Character")
        {
            //ゴールファンファーレ
//            audioSource.Play();
            //ゴール後処理(シーン遷移)
            Debug.Log("GOOOOOOOOOOOOOOAL!!!!!!!!!!!!!!!");
            AudioManager.Instance.StopBGM();
            ShipFire.SetActive(true);

            planetScene.GameClear();
        }
    }
}
