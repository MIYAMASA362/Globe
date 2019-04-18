using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GalaxySelectScene : SceneBase {

    private int nGalaxyNum = 0;

	// Use this for initialization
	public override void Start () {
        base.Start();
        nGalaxyNum = 0;
        MySceneManager.nSelecter_Galaxy = nGalaxyNum;
    }
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        if (Input.GetKeyDown(KeyCode.D))
        {
            nGalaxyNum++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            nGalaxyNum--;
        }

        if (nGalaxyNum <= -1) nGalaxyNum = MySceneManager.nMaxGalaxyNum;

        nGalaxyNum = nGalaxyNum % MySceneManager.nMaxGalaxyNum;

        if (Input.GetKeyDown(KeyCode.Return))LoadGalaxyScene();
        MySceneManager.nSelecter_Galaxy = nGalaxyNum;
    }

    public void LoadGalaxyScene()
    {
        MySceneManager.FadeInLoad(MySceneManager.Get_NowGalaxy());
    }
}
