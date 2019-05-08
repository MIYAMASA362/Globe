using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

/*
 * どうやって世の中にお金を回すか
 * 労働の対価と幸せの対価
 * 
 * 何に失敗したのか、何をリカバリーしたのか
 * 
 */
public class StageSelectScene : SceneBase
{
    //--- Enum ----------------------------------------------------------------

    private enum STATE
    {
        GALAXYSELECT,
        PLANETSELECT
    }

    //--- Class ---------------------------------------------------------------

    [System.Serializable]
    public class Galaxy
    {
        [SerializeField, Tooltip("ステージ選択時のカメラ")]
        public GameObject planetCamera;
        [SerializeField,Tooltip("銀河")]
        public GalaxyState galaxyState;
    }

    //--- Attribute -----------------------------------------------------------

    [Header("StarCrystal State")]
    [SerializeField, Tooltip("星の宝石の総取得可能数")]
    private int nMaxStarCrystalNum;
    [SerializeField, Tooltip("星の宝石の取得数")]
    private int nGetStarCrystalNum;

    [Space(8)]
    [Header("Crystal State")]
    [SerializeField,Tooltip("隠し宝石の総取得可能数")]
    private int nMaxCrystalNum;
    [SerializeField,Tooltip("隠し宝石の取得数")]
    private int nGetCrystalNum;

    [Space(8)]
    [Header("Select State")]
    [SerializeField, Tooltip("エリア選択時のカメラ")]
    private GameObject galaxyCamera;
    [SerializeField, Tooltip("銀河を保持してるGameObject")]
    private GameObject GalaxysHolder;
    [SerializeField, Tooltip("銀河")]
    private Galaxy[] Galaxies = new Galaxy[4];

    [Space(4)]
    [SerializeField, Tooltip("エリアの選択可能最大数")]
    private int nMaxGalaxyNum;
    [SerializeField, Tooltip("選択中のエリア")]
    private int nGalaxySelectNum;
    [SerializeField, Tooltip("ステージの選択可能最大数")]
    private int nMaxPlanetNum;
    [SerializeField, Tooltip("選択中のステージ")]
    private int nPlanetSelectNum;

    //--- Internal State ------------------------
    [SerializeField] private STATE state = STATE.GALAXYSELECT;  //状態遷移
    [SerializeField] private bool IsInput = false;              //入力可能か
    [SerializeField] private float selecter = 0f;               //入力値
    [SerializeField] private float TargetAngle = 0f;            //回転値
    [SerializeField] private GameObject TargetRotObj = null;    //回転させるオブジェクト

    //--- const ---------------------------------
    private const float GalaxyRot = 90f;    //Galaxyの回転量
    private const float PlanetRot = 72f;    //Planetの回転量
    private const float RotSpeed = 8f;      //回転速度

    //--- MonoBehaviour -------------------------------------------------------

	// Use this for initialization
	public override void Start ()
    {
        base.Start();

        //星の宝石の設定
        nMaxStarCrystalNum = 0;
        nGetStarCrystalNum = 0;

        //隠し宝石の設定
        nMaxCrystalNum = 0;
        nGetCrystalNum = 0;

        //GalaxyStateを設定し、情報取得
        foreach (Galaxy galaxy in Galaxies)
        {
            if (galaxy == null) continue;
            //PlanetCameraを無効化
            galaxy.planetCamera.SetActive(false);

            //エリアの集計を設定
            galaxy.galaxyState.selectScene = this.GetComponent<StageSelectScene>();
            galaxy.galaxyState.InitState();

            //隠し宝石
            nMaxCrystalNum += galaxy.galaxyState.nMaxCrystalNum;
            nGetCrystalNum += galaxy.galaxyState.nGetCrystalNum;

            //星の宝石
            nMaxStarCrystalNum += galaxy.galaxyState.nMaxStarCrystalNum;
            nGetStarCrystalNum += galaxy.galaxyState.nGetStarCrystalNum;
        }

        //選択中の内容
        nGalaxySelectNum = 0;
        nPlanetSelectNum = 0;

        //最大数の設定
        nMaxGalaxyNum = MySceneManager.Instance.Galaxies.Count;
        nMaxPlanetNum = MySceneManager.Instance.Galaxies[nPlanetSelectNum].Path_Planets.Count;

        //GalaxyCameraを設定
        if (state == STATE.GALAXYSELECT) galaxyCamera.SetActive(true);

        //保存データ更新
        SelectDataUpdate();

        //回転させる対象を設定
        TargetRotObj = GalaxysHolder;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        //入力取得
        selecter = Input.GetAxis(InputManager.X_Selecter);
        if (selecter == 0f) IsInput = true;

        //state分岐
        switch (state)
        {
            case STATE.GALAXYSELECT:
                GalaxySelect_Update();
                if(galaxyCamera.activeSelf)
                    galaxyCamera.SetActive(true);
                break;
            case STATE.PLANETSELECT:
                PlanetSelect_Update();
                if (Galaxies[nGalaxySelectNum].planetCamera.activeSelf)
                    Galaxies[nGalaxySelectNum].planetCamera.SetActive(true);
                break;
            default:
                break;
        }

        Quaternion q1 = TargetRotObj.transform.rotation;
        Quaternion q2 = Quaternion.Euler(0f, TargetAngle, 0f);
        TargetRotObj.transform.rotation = Quaternion.Lerp(q1, q2, Time.deltaTime * RotSpeed);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Galaxies.Length - 1; i++)
        {
            Gizmos.DrawLine(Galaxies[i].galaxyState.gameObject.transform.position, Galaxies[i + 1].galaxyState.gameObject.transform.position);
        }
    }

    //--- Method --------------------------------------------------------------

    //--- 更新 --------------------------------------------

    //--- エリア選択 ----------------------------
    public void GalaxySelect_Update()
    {
        //前フレーム変更格納
        int nOld = nGalaxySelectNum;

        //選択更新
        if (IsInput)
        {
            if(selecter >= 0.5f)
            {
                nGalaxySelectNum++;
                IsInput = false;
            }

            if(selecter <= -0.5f)
            {
                nGalaxySelectNum--;
                IsInput = false;
            }
        }

        //更新されている
        if (nOld != nGalaxySelectNum)
        {
            //マイナス値ならば
            if (nGalaxySelectNum <= -1) nGalaxySelectNum = nMaxGalaxyNum - 1;
            nGalaxySelectNum = nGalaxySelectNum % nMaxGalaxyNum;

            //保存データ更新
            SelectDataUpdate();

            //回転対象を設定
            Set_RotTargetState(GalaxysHolder,GalaxyRot * nGalaxySelectNum);
        }

        //決定キーを確認
        if (Input.GetButtonDown(InputManager.Submit))
        {
            //カメラ切り替え
            Galaxies[nGalaxySelectNum].planetCamera.SetActive(true);
            //最大数を更新
            nMaxPlanetNum = MySceneManager.Instance.Galaxies[nGalaxySelectNum].Path_Planets.Count;
            //遷移を更新
            state = STATE.PLANETSELECT;
        }

        if(Input.GetButtonDown(InputManager.Cancel))
            MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title, false);
    }

    //--- 惑星選択 ------------------------------
    public void PlanetSelect_Update()
    {
        //前フレーム変更格納
        int nOld = nPlanetSelectNum;

        //選択更新
        if (IsInput)
        {
            if (selecter >= 0.5f)
            {
                nPlanetSelectNum++;
                IsInput = false;
            }

            if (selecter <= -0.5f)
            {
                nPlanetSelectNum--;
                IsInput = false;
            }
        }

        //更新されている
        if (nOld != nPlanetSelectNum)
        {
            //マイナス値ならば
            if (nPlanetSelectNum <= -1) nPlanetSelectNum = nMaxPlanetNum - 1;
            nPlanetSelectNum = nPlanetSelectNum % nMaxPlanetNum;

            //保存データ更新
            SelectDataUpdate();

            //回転対象を設定
            Set_RotTargetState(Galaxies[nGalaxySelectNum].galaxyState.PlanetParent, PlanetRot * nPlanetSelectNum);
        }

        //決定キーを確認
        if (Planet_Submit())
        {
            
        }

        if (Input.GetButtonDown(InputManager.Cancel))
        {
            state = STATE.GALAXYSELECT;
            Galaxies[nGalaxySelectNum].planetCamera.SetActive(false);
        }
    }

    //--- 回転の対象設定 --------------------------
    void Set_RotTargetState(GameObject target,float Angle)
    {
        TargetRotObj = target;
        TargetAngle = Angle;
    }

    //--- 入力系 ------------------------------------------

    //--- 決定 ----------------------------------

    //銀河を決定し、遷移が有効
    private bool Galaxy_Submit()
    {
        //決定キーが押された
        if (!Input.GetButtonDown(InputManager.Submit)) return false;
        //銀河のロックが解除されるか
        if (!Galaxies[nGalaxySelectNum].galaxyState.CheckLock(nGetCrystalNum)) return false;
        return true;
    }

    //惑星を決定した
    private bool Planet_Submit()
    {
        //決定キーが押された
        if (!Input.GetButtonDown(InputManager.Submit)) return false;
        return true;
    }

    //--- DataManager -------------------------------------

    //--- データ更新 ----------------------------
    private void SelectDataUpdate()
    {
        DataManager.Instance.playerData.SelectGalaxy = nGalaxySelectNum;
        DataManager.Instance.playerData.SelectPlanet = nPlanetSelectNum;
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
