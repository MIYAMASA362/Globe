using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class TitleScene : SceneBase {

	// Use this for initialization
    public override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(MySceneManager.GalaxySelect);
        }
	}
}
