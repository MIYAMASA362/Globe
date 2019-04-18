using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlanetScene :SceneBase {

    private bool GameClear = false;

	// Use this for initialization
	public override void Start ()
    {
        GameClear = false;
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        if (GameClear)
        {
            MySceneManager.FadeInLoad(MySceneManager.Get_NextPlanet());
        }
	}
}
