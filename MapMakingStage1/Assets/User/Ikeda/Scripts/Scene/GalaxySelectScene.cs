using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GalaxySelectScene : SceneBase {

    //--- State ---------------------------------
    [SerializeField] private GameObject[] Galaxys;
    [SerializeField] private Transform CameraPivot = null;
    [SerializeField] private int nGalaxyNum = 0;

    private bool bInput = false;
    private Vector3 move;
    GameObject SelectObj = null;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        nGalaxyNum = 0;
        bInput = false;

        MySceneManager.nSelecter_Galaxy = nGalaxyNum;

        foreach (GameObject obj in Galaxys)
        {
            SetCanvas(obj,false);
        }
        SelectObj = Galaxys[nGalaxyNum];
        SetCanvas(SelectObj,true);
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        float selecter = Input.GetAxis(InputManager.X_Selecter);
        int old = nGalaxyNum;

        if (selecter == 0f) bInput = true;

        if (bInput)
        {
            if(selecter >= 0.5f)
            {
                nGalaxyNum++;
                bInput = false;
            }
            if(selecter <= -0.5f)
            {
                nGalaxyNum--;
                bInput = false;
            }
        }

        if (nGalaxyNum <= -1) nGalaxyNum = MySceneManager.nMaxGalaxyNum-1;

        nGalaxyNum = nGalaxyNum % MySceneManager.nMaxGalaxyNum;

        if(old != nGalaxyNum)
        {
            SelectObj = Galaxys[nGalaxyNum];
            SetCanvas(Galaxys[old],false);
            SetCanvas(SelectObj,true);
        }

        if (SelectObj.transform.position != CameraPivot.transform.position)
            CameraPivot.transform.position = Vector3.Lerp(CameraPivot.transform.position,SelectObj.transform.position,Time.deltaTime);

        if (Input.GetButtonDown(InputManager.Submit))LoadGalaxyScene();
        if (Input.GetButtonDown(InputManager.Cancel)) MySceneManager.FadeInLoad(MySceneManager.TitleScene);
        MySceneManager.nSelecter_Galaxy = nGalaxyNum;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Galaxys.Length - 1; i++)
        {
            Gizmos.DrawLine(Galaxys[i].transform.position, Galaxys[i + 1].transform.position);
        }
    }

    public void LoadGalaxyScene()
    {
        MySceneManager.FadeInLoad(MySceneManager.Get_NowGalaxy());
    }

    //PlanetのCanvasを変更させる
    void SetCanvas(GameObject obj, bool enable)
    {
        obj.transform.Find("Canvas").gameObject.SetActive(enable);
    }

}
