using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

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

    //--- Inspecter State -----------------------
    [SerializeField]
    private Animator EnterAnimator;
    [Header("UI State")]
    [SerializeField,Tooltip("銀河の名前")]
    private TextMeshProUGUI GalaxyNameText;
    [SerializeField,Tooltip("惑星の名前")]
    private TextMeshProUGUI PlanetNameText;

    [Space(4)]
    [SerializeField, Tooltip("銀河の難易度表示")]
    private TextMeshProUGUI GalaxyLevelText;
    [SerializeField, Tooltip("惑星の難易度表示")]
    private TextMeshProUGUI PlanetLevelText;

    [Space(4)]
    [Header("ItemUI")]
    [SerializeField, Tooltip("未取得アイテムのUI表示")]
    private GameObject ItemUI;
    [SerializeField,Tooltip("星の宝石数を表示するテキスト")]
    private TextMeshProUGUI ItemUI_Text_StarCrystal;
    [SerializeField, Tooltip("隠し宝石数を表示するテキスト")]
    private TextMeshProUGUI ItemUI_Text_Crystal;
    [Space(4)]
    [SerializeField, Tooltip("銀河選択時の説明テキスト")]
    private GameObject ItemUI_Obj_GalaxyText;
    [SerializeField, Tooltip("惑星選択時の説明テキスト")]
    private GameObject ItemUI_Obj_PlanetText;
    [Space(4)]
    [SerializeField, Tooltip("星の宝石のカウント")]
    private TextMeshProUGUI StarCrystal_CountText;
    [SerializeField, Tooltip("隠し宝石のカウント")]
    private TextMeshProUGUI Crystal_CountText;
    [Space(4)]
    [SerializeField,Tooltip("コンプリート表示")]
    private GameObject CompleteUI;

    [Space(8)]
    [Header("LockUI")]
    [SerializeField, Tooltip("未解除エリアのUI表示")]
    private GameObject LockUI;
    [SerializeField]
    private TextMeshProUGUI LockUI_Text;

    [Space(4)]
    [SerializeField, Tooltip("エリア選択時のUIオブジェクト")]
    private GameObject GalaxySelectUI;
    [SerializeField, Tooltip("ステージ選択時のUIオブジェクト")]
    private GameObject PlanetSelectUI;

    [Space(4)]
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
    private static bool IsLoad_Start_PlanetSelect = false;
    [SerializeField] private STATE state;                   //状態遷移
    [SerializeField] private bool IsInput = false;          //入力可能か
    [SerializeField] private float IsInput_PauseTime = 0f;      //IsInputをfalse状態にする時間
    private bool IsInputLeft = false;           //左入力
    private bool IsInputRight = false;          //右入力
    private float TargetAngle = 0f;             //回転値
    private GameObject TargetRotObj = null;     //回転させるオブジェクト

    //--- const ---------------------------------
    private const float GALAXY_ROTARION_ANGLE = 90f;    //Galaxyの回転量
    private const float PLANET_ROTATION_ANGLE = 72f;    //Planetの回転量
    private const float ROTATION_SPEED = 8f;      //回転速度
    private const float ISINPUT_POUSETIME = 0.5f;

    //--- MonoBehaviour -------------------------------------------------------

	// Use this for initialization
	public override void Start ()
    {
        TargetRotObj = GalaxysHolder;
        StartCoroutine("Init_Coroutine");
        if (MySceneManager.Instance != null)
            if (!MySceneManager.Instance.bInitLoad) MySceneManager.Instance.Start_Load();
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        //Load中
        if (MySceneManager.IsLoading() || MySceneManager.IsFadeing) return;

        base.Update();

        IsInputLeft = Input.GetButtonDown(InputManager.Left_AxisRotation);
        IsInputRight = Input.GetButtonDown(InputManager.Right_AxisRotation);

        //入力取得
        IsInput_PauseTime -= Time.deltaTime;
        if (IsInput_PauseTime < 0f) IsInput_PauseTime = 0f;
        if (IsInput_PauseTime == 0f) IsInput = true;

        //state分岐
        switch (state)
        {
            case STATE.GALAXYSELECT:
                //更新処理
                GalaxySelect_Update();
                break;
            case STATE.PLANETSELECT:
                //更新処理
                PlanetSelect_Update();
                break;
            default:
                break;
        }

        TargetRotation();
    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i < Galaxies.Length - 1; i++)
        {
            Gizmos.DrawLine(Galaxies[i].galaxyState.gameObject.transform.position, Galaxies[i + 1].galaxyState.gameObject.transform.position);
        }
    }

    //--- Coroutine -----------------------------------------------------------

    //--- Startでの読み込み ---------------------
    IEnumerator Init_Coroutine()
    {
        base.Start();

        //---　初期化(初期状態に必要なパラメータなどにアクセスして調整する) ---

        yield return new WaitForSeconds(1f);

        //GalaxyStateを設定し、情報取得
        foreach (Galaxy galaxy in Galaxies)
        {
            if (galaxy == null) continue;
            //PlanetCameraを無効化
            galaxy.planetCamera.SetActive(false);

            //エリアの集計を設定
            galaxy.galaxyState.selectScene = this.GetComponent<StageSelectScene>();
            galaxy.galaxyState.InitState();
        }

        yield return new WaitForSeconds(1f);

        //読み込み
        DataManager.Instance.PlayerData_Load();
        
        nGalaxySelectNum = DataManager.Instance.playerData.SelectGalaxy;
        nPlanetSelectNum = DataManager.Instance.playerData.SelectPlanet;

        //最大数の設定
        nMaxGalaxyNum = MySceneManager.Instance.Galaxies.Count;
        nMaxPlanetNum = MySceneManager.Instance.Galaxies[nGalaxySelectNum].Planets.Count;

        //UIを初期化
        GalaxySelectUI.SetActive(false);
        PlanetSelectUI.SetActive(false);

        //GalaxyCameraを設定
        state = STATE.GALAXYSELECT;
        Change_SelectUI(state);

        yield return new WaitForSeconds(1f);

        //--- 初期状態設定 ----------------------------
        
        //回転させる対象を設定
        TargetRotObj = GalaxysHolder;

        //テキストの設定
        SetGalaxyLevelText(nGalaxySelectNum + 1);
        GalaxyNameText.text = MySceneManager.Instance.Galaxies[nGalaxySelectNum].name;

        StarCrystal_CountText.text = DataManager.Instance.playerData.GetStarCrystalNum.ToString("00");
        Crystal_CountText.text = DataManager.Instance.playerData.GetCrystalNum.ToString("00");

        TargetAngle = GALAXY_ROTARION_ANGLE * nGalaxySelectNum;

        //Cameraの切り替え
        Set_GalaxyCamera();

        //Lockの切り替え
        Change_Lock_UnLock(true);

        if (IsLoad_Start_PlanetSelect)
            LoadInit_PlanetSelect();

        IsInput_PauseTime = 0.5f;

        MySceneManager.Instance.CompleteLoaded();

        AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_TITLE);

        yield break;
    }

    //--- Method --------------------------------------------------------------

    //--- 更新 --------------------------------------------

    void TargetRotation()
    {
        Quaternion q1 = TargetRotObj.transform.rotation;
        Quaternion q2 = Quaternion.Euler(0f, TargetAngle, 0f);
        TargetRotObj.transform.rotation = Quaternion.Lerp(q1, q2, Time.deltaTime * ROTATION_SPEED);
    }

    //--- エリア選択 ----------------------------
    public void GalaxySelect_Update()
    {
        //選択更新
        if (!IsInput) return;

        //前フレーム変更格納
        int nOld = nGalaxySelectNum;
        if (IsInputLeft)
        {
            nGalaxySelectNum++;
            IsInput = false;
        }

        if(IsInputRight)
        {
            nGalaxySelectNum--;
            IsInput = false;
        }

        //選択が更新されている
        if (nOld != nGalaxySelectNum)
        {
            base.PlayAudio_Select();
            //マイナス値ならば
            if (nGalaxySelectNum <= -1) nGalaxySelectNum = nMaxGalaxyNum - 1;
            if(nMaxGalaxyNum != 0)
                nGalaxySelectNum = nGalaxySelectNum % nMaxGalaxyNum;

            //保存データ更新
            Save_SelectData();

            //回転対象を設定
            Set_RotTargetState(GalaxysHolder,GALAXY_ROTARION_ANGLE * nGalaxySelectNum);

            //UI更新
            Change_SelectUI(state);
            Change_Lock_UnLock(true);

            //入力Pause
            IsInput_PauseTime = ISINPUT_POUSETIME;
        }

        //決定キーを確認
        if (Galaxy_Submit())
        {
            base.PlayAudio_Success();
            nPlanetSelectNum = 0;

            //カメラ切り替え
            Set_PlanetCamera();
            //最大数を更新
            nMaxPlanetNum = MySceneManager.Instance.Galaxies[nGalaxySelectNum].Planets.Count;
            //遷移を更新
            state = STATE.PLANETSELECT;

            //回転対象を設定
            Set_RotTargetState(Galaxies[nGalaxySelectNum].galaxyState.PlanetParent, PLANET_ROTATION_ANGLE * nPlanetSelectNum);

            //UI変更
            Change_SelectUI(state);
            Change_Lock_UnLock(false);
            Change_ItemUI(true);
        }

        if (Input.GetButtonDown(InputManager.Cancel))
        {
            base.PlayAudio_Return();
            MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title, false);
        }    
    }

    //--- 惑星選択 ------------------------------
    public void PlanetSelect_Update()
    {
        //選択更新
        if (!IsInput) return;
        //前フレーム変更格納
        int nOld = nPlanetSelectNum;
        
        if (IsInputLeft)
        {
            nPlanetSelectNum++;
            IsInput = false;
        }

        if (IsInputRight)
        {
            nPlanetSelectNum--;
            IsInput = false;
        }

        //更新されている
        if (nOld != nPlanetSelectNum)
        {
            base.PlayAudio_Select();
            //マイナス値ならば
            if (nPlanetSelectNum <= -1) nPlanetSelectNum = nMaxPlanetNum - 1;
            nPlanetSelectNum = nPlanetSelectNum % nMaxPlanetNum;

            //保存データ更新
            Save_SelectData();

            //回転対象を設定
            Set_RotTargetState(Galaxies[nGalaxySelectNum].galaxyState.PlanetParent, PLANET_ROTATION_ANGLE * nPlanetSelectNum);

            //UI更新
            Change_SelectUI(state);
            Change_ItemUI(true);

            //入力Pause
            IsInput_PauseTime = ISINPUT_POUSETIME;
        }

        //決定キーを確認
        if (Planet_Submit())
        {
            base.PlayAudio_Success();
            LoadPlanetScene();
        }

        //戻る
        if (Input.GetButtonDown(InputManager.Cancel))
        {
            base.PlayAudio_Return();
            state = STATE.GALAXYSELECT;
            Set_GalaxyCamera();
            //UI変更
            Change_SelectUI(state);
            Change_Lock_UnLock(true);
        }
    }

    //--- 回転の対象設定 ------------------------
    void Set_RotTargetState(GameObject target,float Angle)
    {
        TargetRotObj = target;
        TargetAngle = Angle;
    }

    //--- 選択UIの更新　-------------------------
    void Change_SelectUI(STATE state)
    {
        switch (state)
        {
            case STATE.GALAXYSELECT:
                //UIConfigなどを更新
                GalaxySelectUI.SetActive(true);
                PlanetSelectUI.SetActive(false);
                break;
            case STATE.PLANETSELECT:
                //UIConfigなどを更新
                GalaxySelectUI.SetActive(false);
                PlanetSelectUI.SetActive(true);
                break;
            default:
                break;
        }

        //銀河名更新
        GalaxyNameText.text = MySceneManager.Instance.Galaxies[nGalaxySelectNum].name;
        //惑星名更新
        PlanetNameText.text = MySceneManager.Instance.Galaxies[nGalaxySelectNum].Planets[nPlanetSelectNum].name;

        //銀河LEVEL名変更
        SetGalaxyLevelText((nGalaxySelectNum + 1));
        //惑星LEVEL名変更
        PlanetLevelText.text = (nGalaxySelectNum + 1)+ "-" + (nPlanetSelectNum+1);
    }

    void SetGalaxyLevelText(int nGalaxyNum)
    {
        GalaxyLevelText.text = "エリア " + nGalaxyNum;
    }

    //LockとUnLockを変更　IsActiveで表示の有効化、無効化
    void Change_Lock_UnLock(bool IsActive)
    {
        //Activeであるならば
        if(LockUI.activeSelf) LockUI.SetActive(false);
        if(ItemUI.activeSelf) ItemUI.SetActive(false);

        if (!IsActive) return;

        //Lockされた
        bool IsLock = Galaxies[nGalaxySelectNum].galaxyState.CheckLock(DataManager.Instance.playerData.GetCrystalNum);
        //銀河の情報
        GalaxyState galaxyState = Galaxies[nGalaxySelectNum].galaxyState;
        if (IsLock)
        {
            //ロックされているのか表示
            LockUI.SetActive(true);
            LockUI_Text.text = "あと" + galaxyState.Crtstal_Diffrence(DataManager.Instance.playerData.GetCrystalNum)+"こ";
            CompleteUI.SetActive(false);
        }
        else
            Change_ItemUI(IsActive);
    }

    //ItemUIの表示 IsActiveで表示の有効、無効変換
    void Change_ItemUI(bool IsActive)
    {
        //Activeであるならば
        if(ItemUI.activeSelf) ItemUI.SetActive(false);
        if(ItemUI_Obj_GalaxyText.activeSelf) ItemUI_Obj_GalaxyText.SetActive(false);
        if(ItemUI_Obj_PlanetText.activeSelf) ItemUI_Obj_PlanetText.SetActive(false);

        if (!IsActive) return;

        GalaxyState galaxyState = Galaxies[nGalaxySelectNum].galaxyState;
        switch (state)
        {
            case STATE.GALAXYSELECT:
                
                //ステージの内容表示
                ItemUI.SetActive(true);
                ItemUI_Obj_GalaxyText.SetActive(true);
                ItemUI_Text_Crystal.text = "あと" + galaxyState.Crystal_RemainingNum() + "こ";
                ItemUI_Text_StarCrystal.text = "あと" + galaxyState.StarCrystal_ReaminingNum() + "こ";
                CompleteUI.SetActive(galaxyState.IsComplete());
                break;
            case STATE.PLANETSELECT:
                PlanetState planetState = galaxyState.Planets[nPlanetSelectNum];
                ItemUI.SetActive(true);
                ItemUI_Obj_PlanetText.SetActive(true);
                ItemUI_Text_Crystal.text = "あと" + planetState.Crystal_ReaminingNum() + "こ";
                ItemUI_Text_StarCrystal.text = "あと" + planetState.StarCrystal_ReaminingNum() + "こ";

                CompleteUI.SetActive(planetState.IsComplete());
                break;
            default:
                break;
        }
    }

    void Set_GalaxyCamera()
    {
        galaxyCamera.SetActive(true);
        Galaxies[nGalaxySelectNum].planetCamera.SetActive(false);
    }

    void Set_PlanetCamera()
    {
        galaxyCamera.SetActive(false);
        Galaxies[nGalaxySelectNum].planetCamera.SetActive(true);
    }

    //--- 入力系 ------------------------------------------

    //--- 決定 ----------------------------------

    //銀河を決定し、遷移が有効
    private bool Galaxy_Submit()
    {
        //決定キーが押された
        if (!Input.GetButtonDown(InputManager.Submit)) return false;
        //銀河のロックが解除されるか
        if (Galaxies[nGalaxySelectNum].galaxyState.CheckLock(DataManager.Instance.playerData.GetCrystalNum)) return false;
        IsInput_PauseTime = ISINPUT_POUSETIME;
        return true;
    }

    //惑星を決定した
    private bool Planet_Submit()
    {
        //決定キーが押された
        if (!Input.GetButtonDown(InputManager.Submit)) return false;
        IsInput_PauseTime = ISINPUT_POUSETIME;
        EnterAnimator.SetTrigger("EnterTrigger");
        
        return true;
    }

    //--- Planetのセレクト画面から開始する ------
    public static void Load_Star_PlanetSelect()
    {
        //次にStageSelectSceneに来たらLoadInit_Planetを開始させる
        IsLoad_Start_PlanetSelect = true;
    }

    private void LoadInit_PlanetSelect()
    {
        //セレクトを更新
        nGalaxySelectNum = DataManager.Instance.playerData.SelectGalaxy;
        nPlanetSelectNum = DataManager.Instance.playerData.SelectPlanet;

        state = STATE.PLANETSELECT;

        GalaxysHolder.transform.rotation = Quaternion.AngleAxis(GALAXY_ROTARION_ANGLE * nGalaxySelectNum,Vector3.up);

        Set_RotTargetState(Galaxies[nGalaxySelectNum].galaxyState.PlanetParent,PLANET_ROTATION_ANGLE * nPlanetSelectNum);
        Change_Lock_UnLock(true);
        Change_ItemUI(true);
        Change_SelectUI(state);
        Set_PlanetCamera();
        IsLoad_Start_PlanetSelect = false;
    }

    //--- DataManager -------------------------------------

    private void Save_SelectData()
    {
        DataManager.Instance.PlayerData_SetSelect(nGalaxySelectNum, nPlanetSelectNum);
    }

    //--- SceneManager ------------------------------------

    public void LoadPlanetScene()
    {
        //データを保存
        Save_SelectData();
        //シーン遷移
        MySceneManager.FadeInLoad(MySceneManager.Get_NowPlanet(),true);

        AudioManager.Instance.FadeOutBGM();
    }

    //GalaxyのCanvasを変更させる
    void SetCanvas(GameObject obj, bool enable)
    {
        obj.transform.Find("Canvas").gameObject.SetActive(enable);
    }
}
