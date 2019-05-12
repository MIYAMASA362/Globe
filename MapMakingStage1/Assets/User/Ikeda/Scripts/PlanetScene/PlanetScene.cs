using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(TimeRank))]
[RequireComponent(typeof(CrystalHandle))]
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

    //--- Animator ------------------------------
    [SerializeField] private Animator animator;

    //--- DataManager ---------------------------
    [Space(15),Header("DataManager SaveData")]
    [SerializeField,Tooltip("保存されているデータ")]
    private DataManager.PlanetData planetData;

    private bool bGameClear;
    public STATE state;

    //--- MonoBehaviour -----------------------------------------------------------------

    public override void Start ()
    {
        state = STATE.LOAD;
        Invoke("Loaded",4f);

        timer = this.GetComponent<Timer>();

        timeRank = this.GetComponent<TimeRank>();
        crystalHandle = this.GetComponent<CrystalHandle>();

        base.Start();

        //--- Init status ------------------------------------------------

        bGameClear = false;

        //PlayerDataのセーブ
        DataManager.Instance.Save(ref DataManager.Instance.playerData, DataManager.PLAYER_FILE);

        Load_PlanetData(ref planetData);
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


    private void Load_PlanetData(ref DataManager.PlanetData data)
    {
        data = new DataManager.PlanetData();

        //--- DataManager -----------------------

        //Planetのデータがあるか
        if (DataManager.Instance.FileFind(Get_FineName()))
            //データをロード
            DataManager.Instance.Load(ref data, Get_FineName());

        //--- TimeRank --------------------------
        timeRank.ReSetData(ref data);

        //--- Crystal ---------------------------

        //---------------------------------------

        //セーブする
        DataManager.Instance.Save(ref data, Get_FineName());

    }

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
        bGameClear = true;

    //--- timer ---------
        timer.StopTimer();
        timeRank.Bonus(timer.GetTime());
    //-------------------

        //例えばクリスタルを増やしてみる
        Debug.LogWarning("クリスタルを増やしてる");
        DataManager.Instance.playerData.CrystalNum++;

        //初クリア
        if (!planetData.clear) planetData.clear = true;

        //星データの保存
        DataManager.Instance.Save(ref planetData, Get_FineName());

        //Playerデータの保存
        DataManager.Instance.Save(ref DataManager.Instance.playerData,DataManager.PLAYER_FILE);

        //Scene遷移
        MySceneManager.FadeInLoad(MySceneManager.Load_PlanetSelect(), true);
    }

    //--- DataManager ---------------------------

    //
    //  ファイルの名前を取得
    //
    private string Get_FineName()
    {
        return this.gameObject.scene.name + ".planet";
    }

    //
    //  データ読み込み
    //
    public void LoadData()
    {
        planetData = new DataManager.PlanetData();
        DataManager.Instance.Load(ref planetData,Get_FineName());
    }
}
