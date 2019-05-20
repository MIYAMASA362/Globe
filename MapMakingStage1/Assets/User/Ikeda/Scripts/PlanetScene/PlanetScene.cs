using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DataType;

[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(TimeRank))]
[RequireComponent(typeof(CrystalHandle))]
[RequireComponent(typeof(StarPieceHandle))]
public class PlanetScene :SceneBase
{
    public enum STATE
    {
        LOAD,
        OPENING,
        MAINGAME,
        ENDING,
        RESULT
    }

    //--- Attribute ---------------------------------------------------------------------

    //Component
    private Timer timer;
    private TimeRank timeRank;
    private CrystalHandle crystalHandle;
    private StarPieceHandle starPieceHandle;

    //--- Animator ------------------------------
    [SerializeField] private Animator animator;

    private bool IsGameClear;
    public STATE state;

    //--- Data ----------------------------------
    [Space(8)]
    [SerializeField] public string DataFile = "";
    [SerializeField] private PlanetData planetData;

    //--- MonoBehaviour -----------------------------------------------------------------

    public override void Start ()
    {
        state = STATE.LOAD;
        Invoke("Loaded",4f);

        //--- Component ---
        timer = this.GetComponent<Timer>();
        timeRank = this.GetComponent<TimeRank>();
        crystalHandle = this.GetComponent<CrystalHandle>();
        starPieceHandle = this.GetComponent<StarPieceHandle>();

        //-- データ初期化 ---
        InitData();

        //--- Init status ---
        base.Start();
        IsGameClear = false;

    }
    
    public override void Update ()
    {
        if (state != STATE.MAINGAME) return;
        base.Update();

        //--- Timer ---
        timer.UpdateTimer();
        timeRank.Update_RankUI();
	}

    //--- Method ------------------------------------------------------------------------

    //--- ロードを完了させる --------------------
    public void Loaded()
    {
        MySceneManager.Instance.CompleteLoaded();
        //        animator.SetTrigger("AroundTrigger");
        animator.gameObject.SetActive(false);
        state = STATE.OPENING;
    }

    //--- オープニング終了 ----------------------
    public void EndOpening()
    {
        timer.StartTimer();
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
        IsGameClear = true;
        timer.StopTimer();

        UnInitData();   //データセーブ
        

        MySceneManager.FadeInLoad(MySceneManager.Load_PlanetSelect(), true);    //Scene遷移
    }

    //--- DataManager ---------------------------

    private void InitData()
    {
        DataManager.Instance.Save_PlayerData();                     //PlayerDataのセーブ
        LoadData();
    }

    public void LoadData()
    {
        DataFile = this.gameObject.scene.name;
        planetData = new PlanetData(DataFile);

        if (DataHandle.FileFind(planetData.FileName()))
            DataHandle.Load(ref planetData, planetData.FileName()); //データがあれば読み込み
        else
            DataHandle.Save(ref planetData, planetData.FileName()); //データがなければ書き込み
    }

    public void SaveData()
    {
        planetData = new PlanetData(DataFile);
        DataHandle.Save(ref planetData,planetData.FileName());
    }

    private void UnInitData()
    {
        DataManager.Instance.Save_PlayerData();                     //PlayerDataのセーブ

        PlanetData oldData = planetData;

        planetData = new PlanetData(DataFile);
        if(!oldData.IsClear)
            planetData.IsClear = IsGameClear;                           //ステージをクリアしたか
        if(!oldData.IsGet_StarCrystal)
            planetData.IsGet_StarCrystal = starPieceHandle.IsCompleted();   //StarCrystalが完成している
        if(!oldData.IsGet_Crystal)
            planetData.IsGet_Crystal = crystalHandle.IsGetting();       //Crystalを取得している

        DataHandle.Save(ref planetData, planetData.FileName());     //PlanetDataの設定
    }

}
