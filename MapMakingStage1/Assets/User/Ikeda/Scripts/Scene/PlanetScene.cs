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

    //--- Attribute ---------------------------------------------------------------------

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

    //--- Hide State -------------------------
    //[SerializeField,Tooltip("保存されているデータ")]
    private DataManager.PlanetData planetData;

    private bool bGameClear;
    private bool[] RankEnable = { false, false,false };

    //--- MonoBehaviour -----------------------------------------------------------------

    public override void Start ()
    {
        timer = this.GetComponent<Timer>();
        timer.StartTimer();

        bGameClear = false;

        planetData = new DataManager.PlanetData();
        planetData = Load_PlayData(Get_FineName());   //星のデータ取得
        for (int i = 0; i < RankEnable.Length; i++)
        {
            RankEnable[i] = planetData.rank[i];     //タイムランクの取得
        }

        Init_Crystals(ref planetData);          //クリスタルの初期化

        base.Start();
	}

    public override void Update ()
    {
        base.Update();

        Update_RankUI();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameClear();
        }
	}

    //--- Method ------------------------------------------------------------------------

    //
    //  クリスタルを設定する
    //
    private void Init_Crystals(ref DataManager.PlanetData planetData)
    {
        //カウンタ
        int nCount = 0;

        //Crystalの表示の設計
        foreach (Crystal crystal in Crystals)
        {
            if (crystal.gameObject == null) continue;

            //UIを設計
            crystal.ui = Instantiate(CrystalUI_Image, CrystalUI.transform, false);
            //UIをずらし
            crystal.ui.transform.position += crystal.ui.transform.right * (nCount * distance);

            //保存したデータから取得
            if (Crystals.Length == planetData.bGet.Length)
            {
                crystal.bGet = planetData.bGet[nCount];
            }

            //既に取得している
            if (crystal.bGet)
            {
                //薄くする
                Color color = crystal.gameObject.GetComponent<Renderer>().material.color;
                crystal.gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, alpha);

                Color UIColor = crystal.ui.GetComponent<Image>().color;
                crystal.ui.GetComponent<Image>().color = EnableColor;
            }

            //Active化
            crystal.ui.SetActive(true);

            nCount++;
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
        TimeBonus();

        //例えばクリスタルを増やしてみる
        Debug.LogWarning("クリスタルを増やしてる");
        DataManager.Instance.playerData.CrystalNum++;

        //データ保存
        Save_PlayData();

        //Scene遷移
        MySceneManager.FadeInLoad(MySceneManager.Get_NextPlanet());
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

    //
    //  HitしたCrystalを加算
    //
    public void HitCrystal(GameObject hitCrystal)
    {
        foreach(var obj in Crystals)
        {
            //hitしたCrystal
            if(obj.gameObject == hitCrystal)
            {
                //初めて取得する
                if (!obj.bGet)
                {
                    //加算する
                    Debug.LogWarning("クリスタル加算している");
                    DataManager.Instance.playerData.CrystalNum++;
                    planetData.crystalNum++;    //クリスタルの取得済みを更新

                    obj.bGet = true;            //取得した事を伝える
                    obj.ui.GetComponent<Image>().color = EnableColor;   //UIカラー変更
                }

                obj.gameObject.SetActive(false);    //Active変更
            }
        }
    }

    //--- Time ----------------------------------

    //
    //タイムのボーナス
    //
    private void TimeBonus()
    {
        float time = timer.GetTime();

        //ゴールド
        if (IsRange(time, 0, GoaldTime))
        {
            //初めてゴールドレコード
            if (!planetData.bGet[0])
            {
                planetData.bGet[0] = true;
            }

            Debug.Log("ゴールドレコードだ！");
            return;
        }
        //シルバー
        if (IsRange(time, 0, SilverTime))
        {
            //初めてシルバーレコード
            if (!planetData.bGet[1])
            {
                planetData.bGet[1] = true;
            }

            Debug.Log("シルバーレコードだ！");
            return;
        }
        //ブロンズ
        if (IsRange(time, 0, BronzeTime))
        {
            //初めてブロンズレコード
            if (!planetData.bGet[2])
            {
                planetData.bGet[2] = true;
            }

            Debug.Log("ブロンズレコードだ！");
        }
        //以外
        if(time >= BronzeTime)
        {
            Debug.Log("時間かかりすぎた。");
            return;
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
    //  プレイヤーや星のデータを更新
    //
    private DataManager.PlanetData Load_PlayData(string FileName)
    {
        //データをセーブします
        DataManager.Instance.Save(ref DataManager.Instance.playerData, DataManager.PLAYER_FILE);

        //データをロードします
        DataManager.PlanetData Data = new DataManager.PlanetData();
        if (DataManager.Instance.FileFind(FileName))
        {
            DataManager.Instance.Load(ref Data, FileName);
        }

        return Data;
    }

    //
    //  プレイヤーや星のデータを更新
    //
    private void Save_PlayData()
    {
        //初クリア
        if (!planetData.clear) planetData.clear = true;

        //クリスタルの取得判定を保存
        int nCount = 0;
        planetData.bGet = new bool[Crystals.Length];
        foreach (Crystal crystal in Crystals)
        {
            planetData.bGet[nCount] = crystal.bGet;
            nCount++;
        }

        //タイムランクの保存
        for(int i =0; i < RankEnable.Length; i++)
        {
            planetData.rank[i] = RankEnable[i];
        }

        //星のデータ保存
        DataManager.Instance.Save(ref planetData, Get_FineName());
        //プレイヤーのデータ保存
        DataManager.Instance.Save(ref DataManager.Instance.playerData, DataManager.PLAYER_FILE);
    }

    //
    //  データ書き込み
    //
    [ContextMenu("SaveData")]
    private void SaveData()
    {
        planetData = new DataManager.PlanetData();
        planetData.bGet = new bool[Crystals.Length];

        int nCount = 0;
        foreach (Crystal crystal in Crystals)
        {
            planetData.bGet[nCount] = crystal.bGet;
            nCount++;
        }

        DataManager.Instance.Save(ref planetData, Get_FineName());
    }

    //
    //  データ読み込み
    //
    [ContextMenu("LoadData")]
    private void LoadData()
    {
        DataManager.Instance.Load(ref planetData,Get_FineName());
    }

    //
    //  データリセット
    //
    [ContextMenu("ResetData")]
    public void ResetData()
    {
        planetData = new DataManager.PlanetData();

        int nCount = 0;
        for(int i = 0; i<Crystals.Length; i++)
        {
            if (Crystals[i].gameObject != null) nCount++;
        }
        planetData.bGet = new bool[nCount];

        nCount = 0;
        foreach(Crystal crystal in Crystals)
        {
            if(crystal.gameObject != null)
            {
                planetData.bGet[nCount] = crystal.bGet;
                nCount++;
            }
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
