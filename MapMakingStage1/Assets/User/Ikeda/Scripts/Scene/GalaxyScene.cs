using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GalaxyScene : SceneBase
{
    [SerializeField, Tooltip("銀河番号")]
    private int GalaxyNum = 0;

    private int nPlanetNum = 0;

	// Use this for initialization
	public override void Start ()
    {
        MySceneManager.nSelecter_Galaxy = GalaxyNum;
        base.Start();
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.D))
        {
            nPlanetNum++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            nPlanetNum--;
        }

        if (nPlanetNum <= -1) nPlanetNum = MySceneManager.nMaxPlanetNum -1;

        nPlanetNum = nPlanetNum % MySceneManager.nMaxPlanetNum;
    }

    //PlanetSceneへのアクセス
    public void LoadPlanetScene()
    { 
        SceneManager.LoadScene(AssetDatabase.GetAssetPath(MySceneManager.Instance.galaxies[GalaxyNum].Asset_Planets[nPlanetNum]));
        MySceneManager.nSelecter_Planet = nPlanetNum;
    }
}
