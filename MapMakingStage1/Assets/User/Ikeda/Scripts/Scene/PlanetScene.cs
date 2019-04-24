using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
public class PlanetScene :SceneBase
{
    [System.Serializable]
    public class Crystal
    {
        [SerializeField,Tooltip("クリスタルオブジェクト")]
        public GameObject gameObject = null;
        [HideInInspector]   //既に入手しているか
        public bool bGet = false;
        [HideInInspector]
        public GameObject ui = null;
    }

    //--- Timer State ---------------------------
    [SerializeField]
    private Timer timer;

    //--- Ranking Time --------------------------
    [Space(8),Header("Rank Time (Seconds)")]
    [SerializeField]
    private float GoaldTime  = 60f * 1f;
    [SerializeField]
    private float SilverTime = 60f * 2f;
    [SerializeField]
    private float BronzeTime = 60f * 3f;

    [Space(8),Header("Rank UI")]
    [SerializeField]
    private UnityEngine.UI.Image StarUI_1;
    [SerializeField]
    private UnityEngine.UI.Image StarUI_2;
    [SerializeField]
    private UnityEngine.UI.Image StarUI_3;

    //--- Crystal State -------------------------
    [Space(8), Header("Crystal State")]
    [SerializeField,Tooltip("既に取得しているクリスタルの透明度"),Range(0f,1f)]
    private float alpha;
    [SerializeField]
    private Crystal[] Crystals;

    [Space(4), Header("Crystal UI")]
    [SerializeField, Tooltip("クリスタルUI間の距離")]
    private float distance =65f;
    [SerializeField]
    private Color EnableColor = Color.black;

    [Space(4)]
    [SerializeField, Tooltip("クリスタル表示Parent")]
    private GameObject CrystalUI;
    [SerializeField, Tooltip("クリスタルの表示に使う画像")]
    private GameObject CrystalUI_Image;

    private bool bGameClear = false;
    private GameObject[] CrystalUI_Index;

    //--- MonoBehaviour -----------------------------------

	// Use this for initialization
	public override void Start ()
    {
        timer = this.GetComponent<Timer>();
        timer.StartTimer();
        bGameClear = false;
        //データをセーブします。
        DataManager.Instance.SaveAll();
        base.Start();

        //カウンタ
        int nCount = 0;

        //Crystalの表示の設計
        foreach(Crystal crystal in Crystals)
        {
            if (crystal.gameObject == null) continue;

            Debug.Log("生成するよ");

            //UIを設計
            crystal.ui = Instantiate(CrystalUI_Image, CrystalUI.transform, false);
            //UIをずらし
            crystal.ui.transform.position += crystal.ui.transform.right * (nCount * distance);

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
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        RankUI();

        if (bGameClear)
        {
            TimeBonus();
            DataManager.Instance.SaveAll();
            MySceneManager.FadeInLoad(MySceneManager.Get_NextPlanet());
        }
	}

    //--- Method ------------------------------------------
    
    //--- Game ----------------------------------
    public void GameClear()
    {
        bGameClear = true;
        timer.StopTimer();

        //例えばクリスタルを増やしてみる
        Debug.LogWarning("クリスタルを増やしてる");
        DataManager.Instance.nCrystalNum++;
    }

    public void RankUI()
    {
        float time = timer.GetTime();

        if (StarUI_1.fillAmount > 0)
            StarUI_1.fillAmount = 1 - time / GoaldTime;
        else if (StarUI_2.fillAmount > 0)
            StarUI_2.fillAmount = 1 - (time - GoaldTime) / SilverTime;
        else if (StarUI_3.fillAmount > 0)
            StarUI_3.fillAmount = 1 - (time - GoaldTime - SilverTime) / BronzeTime;

    }

    //HitしたCrystalを加算
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
                    DataManager.Instance.nCrystalNum++;
                    obj.gameObject.SetActive(false);
                    obj.ui.GetComponent<Image>().color = EnableColor;
                }
            }
        }
    }

    //--- Time ----------------------------------

    private void TimeBonus()
    {
        float time = timer.GetTime();

        if (IsRange(time, 0, GoaldTime))
        {
            Debug.Log("ゴールドレコードだ！");
            return;
        }
        //二分未満
        else if (IsRange(time, GoaldTime, SilverTime))
        {
            Debug.Log("シルバーレコードだ！");
            return;
        }
        else if (IsRange(time, SilverTime, BronzeTime))
        {
            Debug.Log("ブロンズレコードだ！");
            return;
        }
        else
        {
            Debug.Log("時間かかりすぎた。");
            return;
        }
    }

    //--- function ------------------------------

    //min 以上 max　未満かの判定
    private bool IsRange(float value,float min,float max)
    {
        return (min <= value && value < max);
    }
}
