using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GoalScript : MonoBehaviour {

    [SerializeField] private PlanetScene planetScene = null;

    //ファンファーレのAudio
    private AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        if(planetScene == null)planetScene = GameObject.Find("EventSystem").GetComponent<PlanetScene>();

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
            AudioManager.Instance.StopBGM();

            planetScene.GameClear();
        }
    }
}
