using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DataType;

[RequireComponent(typeof(CrystalHandle))]
[RequireComponent(typeof(StarPieceHandle))]
[RequireComponent(typeof(PlanetResult))]
[RequireComponent(typeof(PlanetOpening))]
public class PlanetScene :SceneBase
{
    public enum STATE
    {
        LOAD,
        OPENING,
        MAINGAME,
        RESULT
    }

    //--- Attribute ---------------------------------------------------------------------

    //Component
    private CrystalHandle crystalHandle;
    private StarPieceHandle starPieceHandle;

    //Planet系
    private PlanetOpening planetOpening;
    private PlanetResult planetResult;

    //--- Animator ------------------------------
    private bool IsGameClear;
    public STATE state;

    //--- Data ----------------------------------
    [Space(8)]
    [SerializeField] public string DataFile = "";
    [SerializeField] private PlanetData planetData;

    //--- MonoBehaviour -----------------------------------------------------------------

    public override void Start ()
    {
        base.Start();

        state = STATE.MAINGAME;
        Invoke("Loaded",4f);

        //--- Component ---
        crystalHandle = this.GetComponent<CrystalHandle>();
        starPieceHandle = this.GetComponent<StarPieceHandle>();
        planetOpening = this.GetComponent<PlanetOpening>();

        planetResult = GetComponent<PlanetResult>();

        //-- データ初期化 ---
        InitData();

        //--- Init status ---
        IsGameClear = false;
    }
    
    public override void Update ()
    {
        switch (state)
        {
            case STATE.LOAD:

                break;
            case STATE.OPENING:

                break;
            case STATE.MAINGAME:
                base.Update();

                break;
            case STATE.RESULT:
                planetResult.Begin();
                break;
            default:
                break;
        }
	}

    //--- Method ------------------------------------------------------------------------

    //--- ロードを完了させる --------------------
    public void Loaded()
    {
        MySceneManager.Instance.CompleteLoaded();
        planetOpening.PopUp_StageLabel();
        EndOpening();
    }

    //--- オープニング終了 ----------------------
    public void EndOpening()
    {
        state = STATE.MAINGAME;
    }

    //--- Game ----------------------------------

    //
    //  ゲームクリア
    //  GameGoalからの呼び出し
    //
    [ContextMenu("GameClear")]
    public void GameClear()
    {
        if (IsGameClear) return;

        IsGameClear = true;

        UnInitData();   //データセーブ

        state = STATE.RESULT;
    }

    //--- DataManager ---------------------------

    private void InitData()
    {
        DataManager.Instance.PlayerData_Save();                    //PlayerDataのセーブ
        LoadData();
    }

    public void LoadData()
    {
        DataFile = this.gameObject.scene.name;
        planetData = new PlanetData(DataFile);

        if (DataHandle.FileFind(planetData.FilePath()))
            DataHandle.Load(ref planetData, planetData.FilePath()); //データがあれば読み込み
        else
            DataHandle.Save(ref planetData, planetData.FilePath()); //データがなければ書き込み
    }

    public void SaveData()
    {
        planetData = new PlanetData(DataFile);
        DataHandle.Save(ref planetData,planetData.FilePath());
    }

    private void UnInitData()
    {
        DataManager.Instance.PlayerData_Save();            //PlayerDataのセーブ

        PlanetData oldData = planetData;

        planetData = new PlanetData(DataFile);

        if(!oldData.IsClear)
            planetData.IsClear = IsGameClear;                           //ステージをクリアしたか
        if(!oldData.IsGet_StarCrystal)
            planetData.IsGet_StarCrystal = starPieceHandle.IsCompleted();   //StarCrystalが完成している
        if(!oldData.IsGet_Crystal)
            planetData.IsGet_Crystal = crystalHandle.IsGetting();       //Crystalを取得している

        DataHandle.Save(ref planetData, planetData.FilePath());     //PlanetDataの設定
    }

    public void NextScene()
    {
        MySceneManager.FadeInLoad(MySceneManager.Load_PlanetSelect(), true);    //Scene遷移
    }

}
