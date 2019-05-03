using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
public class PlanetScene :SceneBase
{
    [System.Serializable]
    public class Crystal
    {
        [SerializeField,Tooltip("クリスタルオブジェクト")]
        public GameObject gameObject = null;
        [SerializeField]   //既に入手しているか
        public bool bGet = false;
        [HideInInspector]
        public GameObject ui = null;
    }

    public enum STATE
    {
        LOAD,
        OPENING,
        MAINGAME,
        ENDING
    }

    //--- Attribute ---------------------------------------------------------------------

    [SerializeField] private Animator animator;

    //--- Timer State ---------------------------
    #region Timer
    [SerializeField]
    private Timer timer;

    #endregion

    //--- Ranking Time --------------------------
    #region Ranging
    [Space(8),Header("Rank Time (Seconds)")]
    [SerializeField] private float GoaldTime  = 60f * 1f;
    [SerializeField] private float SilverTime = 60f * 2f;
    [SerializeField] private float BronzeTime = 60f * 3f;

    [Space(8),Header("Rank UI")]
    [SerializeField] private Image StarUI_1;
    [SerializeField] private Image StarUI_2;
    [SerializeField] private Image StarUI_3;
    #endregion

    //--- Crystal State -------------------------
    #region Crystal
    [Space(8), Header("Crystal State")]
    [SerializeField,Tooltip("既に取得しているクリスタルの透明度"),Range(0f,1f)] private float alpha;
    [SerializeField] private Crystal[] Crystals;

    [Space(4), Header("Crystal UI")]
    [SerializeField, Tooltip("クリスタルUI間の距離")] private float distance = 65f;
    [SerializeField,Tooltip("取得されているCrystalUIの色")] private Color EnableColor = Color.black;

    [Space(4)]
    [SerializeField, Tooltip("クリスタル表示Parent")] private GameObject CrystalUI;
    [SerializeField, Tooltip("クリスタルの表示に使う画像")] private GameObject CrystalUI_Image;

    #endregion

    //--- DataManager -------------------------
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
        timer.StartTimer();

        base.Start();

        //--- Init status ------------------------------------------------

        bGameClear = false;

        planetData = new DataManager.PlanetData();
        planetData.rank = new bool[] { false, false, false };

        //Crystalを入手しているか
        planetData.bGet = new bool[Crystals.Length];
        for (int i = 0; i < Crystals.Length; i++)
            planetData.bGet[i] = Crystals[i].bGet;

        //PlayerDataのセーブ
        DataManager.Instance.Save(ref DataManager.Instance.playerData,DataManager.PLAYER_FILE);

        //PlanetDataのロードorセーブ
        if (DataManager.Instance.FileFind(Get_FineName()))
            DataManager.Instance.Load(ref planetData, Get_FineName());
        else
            DataManager.Instance.Save(ref planetData,Get_FineName());

        //----------------------------------------------------------------

        //CrystalのUIなどを設定
        for(int i=0; i<Crystals.Length; i++)
        {
            Crystal crystal = Crystals[i];
            if (crystal.gameObject == null) continue;

            //UIを作成
            crystal.ui = Instantiate(CrystalUI_Image,CrystalUI.transform,false);
            crystal.ui.transform.position += crystal.ui.transform.right * (i * distance);

            crystal.ui.SetActive(true);

            //既に取得済み
            if (!planetData.bGet[i]) continue;

            crystal.bGet = planetData.bGet[i];

            //薄くする
            Color color = crystal.gameObject.GetComponent<Renderer>().material.color;
            crystal.gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, alpha);

            Color UIColor = crystal.ui.GetComponent<Image>().color;
            crystal.ui.GetComponent<Image>().color = EnableColor;
        }

        //----------------------------------------------------------------
	}

    public override void Update ()
    {
        base.Update();

        Update_RankUI();

        if (Input.GetKeyDown(KeyCode.Space))
            GameClear();
	}

    //--- Method ------------------------------------------------------------------------

    //
    //  
    //
    public void Loaded()
    {
        MySceneManager.Instance.CompleteLoaded();
        animator.SetTrigger("AroundTrigger");
        state = STATE.OPENING;
    }

    //
    //  HitしたCrystalを加算
    //
    public void HitCrystal(GameObject hitCrystal)
    {
        foreach (var crystal in Crystals)
        {
            
            //hitしたCrystal
            if (crystal.gameObject != hitCrystal) continue;

            //Active変更
            crystal.gameObject.SetActive(false);

            //取得している
            if (crystal.bGet) continue;

            //加算する
            Debug.LogWarning("クリスタル加算している");
            DataManager.Instance.playerData.CrystalNum++;
            planetData.crystalNum++;    //クリスタルの取得済みを更新

            crystal.bGet = true;            //取得した事を伝える
            crystal.ui.GetComponent<Image>().color = EnableColor;   //UIカラー変更
        }
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
        TimeBonus(timer.GetTime());
    //-------------------

        //例えばクリスタルを増やしてみる
        Debug.LogWarning("クリスタルを増やしてる");
        DataManager.Instance.playerData.CrystalNum++;

        //初クリア
        if (!planetData.clear) planetData.clear = true;

        //クリスタルのデータを保存
        for(int i = 0; i < Crystals.Length; i++)
            planetData.bGet[i] = Crystals[i].bGet;

        //星データの保存
        DataManager.Instance.Save(ref planetData, Get_FineName());

        //Playerデータの保存
        DataManager.Instance.Save(ref DataManager.Instance.playerData,DataManager.PLAYER_FILE);

        //Scene遷移
        MySceneManager.FadeInLoad(MySceneManager.Get_NextPlanet(), false);
    }
    
    //
    //  ランクUI
    //
    public void Update_RankUI()
    {
        float time = timer.GetTime();

        if (StarUI_1.fillAmount > 0)
            StarUI_1.fillAmount = 1 - time / GoaldTime;
        else if (StarUI_2.fillAmount > 0)
            StarUI_2.fillAmount = 1 - (time - GoaldTime) / SilverTime;
        else if (StarUI_3.fillAmount > 0)
            StarUI_3.fillAmount = 1 - (time - GoaldTime - SilverTime) / BronzeTime;
    }


    //--- Time ----------------------------------

    //
    //タイムのボーナス
    //
    private void TimeBonus(float time)
    {
        //以外
        if(time > BronzeTime)
        {
            Debug.Log("時間かかりすぎた。");
            return;
        }

        //ゴールド
        if (IsRange(time, 0, GoaldTime))
        {
            //初めてゴールドレコード
            if (!planetData.rank[0])
                planetData.rank[0] = true;
            
            Debug.Log("ゴールドレコードだ！");
        }
        //シルバー
        if (IsRange(time, 0, SilverTime))
        {
            //初めてシルバーレコード
            if (!planetData.rank[1])
                planetData.rank[1] = true;

            Debug.Log("シルバーレコードだ！");
        }
        //ブロンズ
        if (IsRange(time, 0, BronzeTime))
        {
            //初めてブロンズレコード
            if (!planetData.rank[2])
                planetData.rank[2] = true;

            Debug.Log("ブロンズレコードだ！");
        }
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

    //
    //  データ保存
    //
    public void SaveData()
    {
        planetData = new DataManager.PlanetData();

        planetData.bGet = new bool[Crystals.Length];
        for(int i= 0; i< Crystals.Length; i++)
        {
            planetData.bGet[i] = Crystals[i].bGet;
        }

        DataManager.Instance.Save(ref planetData, Get_FineName());
    }

    //--- function ------------------------------

    //
    //  min 以上 max　以下かの判定
    //
    private bool IsRange(float value,float min,float max)
    {
        return (min <= value && value <= max);
    }
}
