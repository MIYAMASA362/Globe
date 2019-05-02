using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GalaxySelectScene : SceneBase {

    [System.Serializable]
    class Galaxy
    {
        [SerializeField,Tooltip("銀河")]
        public GameObject galaxy;
        [SerializeField, Tooltip("要求クリスタル数")]
        public int CrystalNum;
    }

    //--- State ---------------------------------
    [SerializeField] private Galaxy[] Galaxys;
    [SerializeField] private Transform CameraPivot = null;
    [SerializeField,Tooltip("Lock表示")] private GameObject LockUI;     //Lock表示
    [SerializeField, Tooltip("要求クリスタル表示")] private TMPro.TextMeshProUGUI CrystalMessage;
    [SerializeField] private int nGalaxyNum = 0;

    private bool bInput = false;
    private Vector3 move;
    GameObject SelectObj = null;

    //--- MonoBehaviour -----------------------------------

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        nGalaxyNum = 0;
        bInput = false;

        DataManager.Instance.playerData.SelectGalaxy = nGalaxyNum;
        DataManager.Instance.playerData.SelectPlanet = 0;

        //Active切り替え
        foreach (var obj in Galaxys)
        {
            SetCanvas(obj.galaxy,false);
        }
        SelectObj = Galaxys[nGalaxyNum].galaxy;
        SetCanvas(SelectObj,true);
        LockUI.SetActive(false);
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        //入力取得
        float selecter = Input.GetAxis(InputManager.X_Selecter);
        int old = nGalaxyNum;   //更新前後の確認

        if (selecter == 0f) bInput = true;

        //選択遷移
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

        //更新されていれば
        if(old != nGalaxyNum)
        {
            //表示を切り替える
            SelectObj = Galaxys[nGalaxyNum].galaxy;
            SetCanvas(Galaxys[old].galaxy,false);
            SetCanvas(SelectObj,true);
        }

        //選択中の星まで移動
        if (SelectObj.transform.position != CameraPivot.transform.position)
            CameraPivot.transform.position = Vector3.Lerp(CameraPivot.transform.position,SelectObj.transform.position,Time.deltaTime);

        //セレクタの更新
        DataManager.Instance.playerData.SelectGalaxy = nGalaxyNum;

        //遷移できるのか
        if (IsGalaxy_Submit())
        {
            LockUI.SetActive(false);

            //セレクト決定
            if (Input.GetButtonDown(InputManager.Submit)) LoadGalaxyScene();
        }
        else
        {
            CrystalMessage.text = "Need Crystal:"+Galaxys[nGalaxyNum].CrystalNum.ToString("00");
            LockUI.SetActive(true);
        }

        //戻る
        if (Input.GetButtonDown(InputManager.Cancel)) MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title,false);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Galaxys.Length - 1; i++)
        {
            Gizmos.DrawLine(Galaxys[i].galaxy.transform.position, Galaxys[i + 1].galaxy.transform.position);
        }
    }

    //--- Method ------------------------------------------

    public bool IsGalaxy_Submit()
    {
        //所持しているクリスタル数が多ければ true
        if (DataManager.Instance.playerData.CrystalNum >= Galaxys[nGalaxyNum].CrystalNum) return true;

        return false;
    }

    public void LoadGalaxyScene()
    {
        MySceneManager.FadeInLoad(MySceneManager.Get_NowGalaxy(),false);
    }

    //GalaxyのCanvasを変更させる
    void SetCanvas(GameObject obj, bool enable)
    {
        obj.transform.Find("Canvas").gameObject.SetActive(enable);
    }

}
