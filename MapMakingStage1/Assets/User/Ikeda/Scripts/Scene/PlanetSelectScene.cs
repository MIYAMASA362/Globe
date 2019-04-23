using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSelectScene : SceneBase
{
    [SerializeField] private GameObject[] Planets;
    [SerializeField] private Transform CameraPivot = null;
    [SerializeField]private int nPanetNum = 0;

    private bool bInput = false;
    private Vector3 move;
    private GameObject SelectObj = null;

    public override void Start()
    {
        base.Start();
        nPanetNum = 0;
        bInput = false;

        MySceneManager.nSelecter_Planet = nPanetNum;

        foreach(GameObject obj in Planets)
        {
            SetCanvas(obj,false);
        }

        SelectObj = Planets[nPanetNum];
        SetCanvas(SelectObj,true);
    }

    public override void Update()
    {
        base.Update();

        float selecter = Input.GetAxis(InputManager.X_Selecter);
        int old = nPanetNum;

        if (selecter == 0f) bInput = true;

        if (bInput)
        {
            if(selecter >= 0.5f)
            {
                nPanetNum++;
                bInput = false;
            }
            if(selecter <= -0.5f)
            {
                nPanetNum--;
                bInput = false;
            }
        }

        if (nPanetNum <= -1) nPanetNum = MySceneManager.nMaxPlanetNum;

        nPanetNum = nPanetNum % MySceneManager.nMaxPlanetNum;

        if(old != nPanetNum)
        {
            SelectObj = Planets[nPanetNum];
            SetCanvas(Planets[nPanetNum],false);
            SetCanvas(SelectObj,true);
        }

        if (SelectObj.transform.position != CameraPivot.transform.position)
            CameraPivot.transform.position = Vector3.Lerp(CameraPivot.transform.position,SelectObj.transform.position,Time.deltaTime);

        if (Input.GetButtonDown(InputManager.Submit)) LoadPlanetScene();
        if (Input.GetButtonDown(InputManager.Cancel)) MySceneManager.FadeInLoad(MySceneManager.GalaxySelect);
        MySceneManager.nSelecter_Planet = nPanetNum;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < Planets.Length -1; i++)
        {
            Gizmos.DrawLine(Planets[i].transform.position, Planets[i + 1].transform.position);
        }
    }

    public void LoadPlanetScene()
    {
        MySceneManager.FadeInLoad(MySceneManager.Get_NowPlanet());
    }

    //PlanetのCanvasを変更させる
    void SetCanvas(GameObject obj, bool enable)
    {
        obj.transform.Find("Canvas").gameObject.SetActive(enable);
    }
}
