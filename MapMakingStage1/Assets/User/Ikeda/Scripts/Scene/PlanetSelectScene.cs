using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSelectScene : SceneBase
{
    private int nPanetNum = 0;

    public override void Start()
    {
        base.Start();
        nPanetNum = 0;
        MySceneManager.nSelecter_Planet = nPanetNum;
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.D))
        {
            nPanetNum++;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            nPanetNum--;
        }

        if (nPanetNum <= -1) nPanetNum = MySceneManager.nMaxPlanetNum;

        nPanetNum = nPanetNum % MySceneManager.nMaxPlanetNum;

        if (Input.GetKeyDown(KeyCode.Return)) LoadPlanetScene();
        MySceneManager.nSelecter_Planet = nPanetNum;

    }

    public void LoadPlanetScene()
    {
        MySceneManager.FadeInLoad(MySceneManager.Get_NowPlanet());
    }
}
