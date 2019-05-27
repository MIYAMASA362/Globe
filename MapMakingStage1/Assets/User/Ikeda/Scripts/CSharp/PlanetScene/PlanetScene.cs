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
        crystalHandle = this.GetComponent<CrystalHandle>();
        starPieceHandle = this.GetComponent<StarPieceHandle>();
        planetOpening = this.GetComponent<PlanetOpening>();

        planetResult = this.GetComponent<PlanetResult>();

        //-- データ初期化 ---
        InitData();

        Invoke("Loaded",4f);

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
                starPieceHandle.Particle_Stop();
                planetResult.Begin();
                break;
            default:
                break;
        }
	}

    //--- Method ------------------------------------------------------------------------

    //--- ロードを完了させる --------------------
    private void Loaded()
    {
        MySceneManager.Instance.CompleteLoaded();
        planetOpening.Begin();
    }

    public void SetState(STATE state)
    {
        this.state = state;
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
        Debug.Log("LoadData:"+planetData.FilePath());


        if (DataHandle.FileFind(planetData.FilePath()))
            DataHandle.Load(ref planetData, planetData.FilePath()); //データがあれば読み込み
        else
            DataHandle.Save(ref planetData, planetData.FilePath()); //データがなければ書き込み
    }

    public void SaveData()
    {
        Debug.Log("SaveFile:" + planetData.FilePath());
        DataHandle.Save(ref planetData,planetData.FilePath());
    }

    private void UnInitData()
    {
        DataManager.Instance.PlayerData_Save();            //PlayerDataのセーブ 

        if(!planetData.IsClear)
            planetData.IsClear = IsGameClear;                           //ステージをクリアしたか

        if (!planetData.IsGet_StarCrystal)
        {
            planetData.IsGet_StarCrystal = starPieceHandle.IsCompleted();   //StarCrystalが完成している
            DataManager.Instance.playerData.GetStarCrystalNum++;
        }

        if (!planetData.IsGet_Crystal)
        {
            planetData.IsGet_Crystal = crystalHandle.IsGetting();       //Crystalを取得している
            DataManager.Instance.playerData.GetCrystalNum++;
        }

        PlanetData OldData = planetData;
        this.planetData = new PlanetData(DataFile);

        this.planetData.IsClear = OldData.IsClear;
        this.planetData.IsGet_Crystal = OldData.IsGet_Crystal;
        this.planetData.IsGet_StarCrystal = OldData.IsGet_StarCrystal;

        SaveData();
    }

    public void NextScene()
    {
        MySceneManager.FadeInLoad(MySceneManager.Load_Next_Planet(),true);
        //MySceneManager.FadeInLoad(MySceneManager.Load_PlanetSelect(), true);    //Scene遷移
    }

}
