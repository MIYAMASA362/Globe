using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class PauseScene : SceneBase
{
    [SerializeField, Tooltip("ステージ名")]
    private TextMeshProUGUI tm_StateName;

    private bool ReSetPlanet = false;
    private bool ReSetGalaxy = false;
    private bool ReternTitle = false;

	// Use this for initialization
	public override void Start ()
    {
        ReSetPlanet = false;
        ReSetGalaxy = false;
        ReternTitle = false;

        tm_StateName.text = SceneManager.GetActiveScene().name;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        if (ReSetPlanet) MySceneManager.FadeInLoad(MySceneManager.Get_NowPlanet());
        if (ReSetGalaxy) MySceneManager.FadeInLoad(MySceneManager.Get_NowPlanet());
        if (ReternTitle) MySceneManager.FadeInLoad(MySceneManager.TitleScene);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            MySceneManager.FadeInLoad(MySceneManager.TitleScene);
            SceneManager.UnloadSceneAsync(MySceneManager.PauseScene);
        }
    }

    private void LateUpdate()
    {
        
    }

}
