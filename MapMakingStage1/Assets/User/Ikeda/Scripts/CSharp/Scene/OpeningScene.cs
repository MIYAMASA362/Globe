using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

[RequireComponent(typeof(VideoPlayer))]
public class OpeningScene : SceneBase {

    //--- Attribute ---------------------------------------

    [SerializeField, Tooltip("入力可能時に表示するオブジェクト")]
    private GameObject InputObj;

    [SerializeField,Tooltip("入力が可能になるフレーム")]
    private float Input_BeginFrame;

    [SerializeField]
    private VideoPlayer videoPlayer;


    //--- MonoBehavior ------------------------------------

    // Use this for initialization
    public override void Start () {
        base.Start();
        InputObj.SetActive(false);
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        if(videoPlayer.frame >= Input_BeginFrame)
        {
            InputObj.SetActive(true);

            if (Input.anyKeyDown)
                MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title,true);
        }
        else
        {
            InputObj.SetActive(false);
        }
	}

}
