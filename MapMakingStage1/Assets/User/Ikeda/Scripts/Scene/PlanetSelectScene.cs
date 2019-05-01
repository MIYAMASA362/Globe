using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSelectScene : SceneBase
{
    [System.Serializable]
    class Planet
    {
        [SerializeField, Tooltip("惑星")]
        public GameObject planet;
        [SerializeField, Tooltip("要求クリスタル数")]
        public int CrystalNum;
    }

    //--- State ---------------------------------
    [SerializeField] private Planet[] Planets;
    [SerializeField] private Transform CameraPivot = null;
    [SerializeField, Tooltip("Lock表示")] private GameObject LockUI;
    [SerializeField, Tooltip("要求クリスタル数")] private TMPro.TextMeshProUGUI CrystalMessage;
    [SerializeField] private int nPanetNum = 0;

    private bool bInput = false;
    private Vector3 move;
    private GameObject SelectObj = null;

    //--- MonoBehaviour -----------------------------------

    public override void Start()
    {
        base.Start();
        nPanetNum = 0;
        bInput = false;

        DataManager.Instance.playerData.SelectPlanet = nPanetNum;

        foreach(var obj in Planets)
        {
            SetCanvas(obj.planet,false);
        }

        SelectObj = Planets[nPanetNum].planet;
        SetCanvas(SelectObj,true);
        LockUI.SetActive(false);
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

        if (nPanetNum <= -1) nPanetNum = MySceneManager.nMaxPlanetNum -1;

        nPanetNum = nPanetNum % MySceneManager.nMaxPlanetNum;

        if(old != nPanetNum)
        {
            SelectObj = Planets[nPanetNum].planet;
            SetCanvas(Planets[nPanetNum].planet,false);
            SetCanvas(SelectObj,true);
        }

        if (SelectObj.transform.position != CameraPivot.transform.position)
            CameraPivot.transform.position = Vector3.Lerp(CameraPivot.transform.position,SelectObj.transform.position,Time.deltaTime);

        if (IsPlanet_Submit())
        {
            LockUI.SetActive(false);
            if (Input.GetButtonDown(InputManager.Submit)) LoadPlanetScene();
        }
        else
        {
            CrystalMessage.text = "Need Crystal:" + Planets[nPanetNum].CrystalNum.ToString("00");
            LockUI.SetActive(true);
        }

        if (Input.GetButtonDown(InputManager.Cancel)) MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect);
        DataManager.Instance.playerData.SelectPlanet = nPanetNum;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < Planets.Length -1; i++)
        {
            Gizmos.DrawLine(Planets[i].planet.transform.position, Planets[i + 1].planet.transform.position);
        }
    }

    //--- Method ------------------------------------------

    public bool IsPlanet_Submit()
    {
        if (DataManager.Instance.playerData.CrystalNum >= Planets[nPanetNum].CrystalNum) return true;

        return false;
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
