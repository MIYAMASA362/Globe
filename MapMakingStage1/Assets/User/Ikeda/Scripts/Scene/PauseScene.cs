using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PauseScene : SceneBase {

    private bool ReSetPlanet = false;
    private bool ReSetGalaxy = false;
    private bool ReternTitle = false;

	// Use this for initialization
	public override void Start ()
    {
        ReSetPlanet = false;
        ReSetGalaxy = false;
        ReternTitle = false;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        if (ReSetPlanet) MySceneManager.Load_Planet();
        if (ReSetGalaxy) MySceneManager.Load_Galaxy();
        if (ReternTitle) SceneManager.LoadScene(MySceneManager.TitleScene);

	}

}
