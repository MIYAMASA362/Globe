using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GoalScript : MonoBehaviour
{
    private bool hit = false;
    private PlanetScene planetScene;

    //ファンファーレのAudio
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        if (planetScene == null) planetScene = PlanetScene.Instance;

        if (planetScene == null) Debug.LogError("PlanetScene.csが見つかりませんでした。PlanetScene.cs is not find");

        audioSource = gameObject.GetComponent<AudioSource>();

        
    }

    private void Update()
    {
        if(hit)
        {
            if (Input.GetButtonDown(InputManager.Set_EarthAxis))
            {
                //ゴールファンファーレ
                AudioManager.Instance.PlaySEOneShot(audioSource, AudioManager.Instance.SE_COMPLETESTAR);
                AudioManager.Instance.FadeOutBGM();
                planetScene.GameClear();
            }
        }
    }

    //ゴールに触れたとき処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hit = false;
        }
    }
}
