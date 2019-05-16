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
        ENDING
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

    //--- Data ----
    [Space(8)]
    [SerializeField] public string DataFile = "";
    [SerializeField] private PlanetData planetData;

    //--- MonoBehaviour -----------------------------------------------------------------

    public override void Start ()
    {
        state = STATE.LOAD;
        Invoke("Loaded",4f);

        timer = this.GetComponent<Timer>();

        timeRank = this.GetComponent<TimeRank>();
        crystalHandle = this.GetComponent<CrystalHandle>();
        starPieceHandle = this.GetComponent<StarPieceHandle>();

        //データ初期化
        InitData();

        base.Start();

        //--- Init status ------------------------------------------------

        IsGameClear = false;
    }

    
    public override void Update ()
    {
        if (state != STATE.MAINGAME) return;
        base.Update();
        timer.UpdateTimer();
        timeRank.Update_RankUI();

        if (Input.GetKeyDown(KeyCode.Space))
            GameClear();
	}

    //--- Method ------------------------------------------------------------------------

    //--- ロードを完了させる --------------------
    public void Loaded()
    {
        MySceneManager.Instance.CompleteLoaded();
        animator.SetTrigger("AroundTrigger");
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
        planetData = new PlanetData(DataFile);

        if (DataHandle.FileFind(planetData.FileName()))
            DataHandle.Load(ref planetData, planetData.FileName()); //データがあれば読み込み
        else
            DataHandle.Save(ref planetData, planetData.FileName()); //データがなければ書き込み
    }

    private void UnInitData()
    {
        DataManager.Instance.Save_PlayerData();                     //PlayerDataのセーブ

        planetData.IsClear = IsGameClear;                           //ステージをクリアしたか
        planetData.IsGet_StarCrystal = starPieceHandle.IsCompleted();   //StarCrystalが完成している
        planetData.IsGet_Crystal = crystalHandle.IsGetting();       //Crystalを取得している

        DataHandle.Save(ref planetData, planetData.FileName());     //PlanetDataの設定
    }

}
