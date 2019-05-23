using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
public class TimeRank : MonoBehaviour
{
    private Timer timer;

    [Header("Rank Time (Seconds)")]
    [SerializeField] private float GoaldTime  = 60f * 1f;
    [SerializeField] private float SilverTime = 60f * 2f;
    [SerializeField] private float BronzeTime = 60f * 3f;

    [Space(8), Header("Rank UI")]
    [SerializeField] private Image StarUI_1;
    [SerializeField] private Image StarUI_2;
    [SerializeField] private Image StarUI_3;

    [SerializeField] private bool IsGoaldRank;
    [SerializeField] private bool IsSilverRank;
    [SerializeField] private bool IsBronzeRank;

    // Use this for initialization
    void Start ()
    {
        timer = this.GetComponent<Timer>();
        timer.StartTimer();

        IsGoaldRank  = false;
        IsSilverRank = false;
        IsBronzeRank = false;
	}

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

    public void ReSetData(ref DataType.PlanetData planetData)
    {
        /*
        planetData.rank = 
            new bool[] {
                IsGoaldRank,
                IsSilverRank,
                IsBronzeRank
            };
        */
    }

    public void Bonus(float time)
    {
        //以外
        if (time > BronzeTime)
        {
            Debug.Log("時間かかりすぎた。");
            return;
        }

        //ゴールド
        if (IsRange(time, 0, GoaldTime))
        {
            IsGoaldRank = true;
            Debug.Log("ゴールドレコードだ！");
        }
        //シルバー
        if (IsRange(time, 0, SilverTime))
        {
            IsSilverRank = true;
            Debug.Log("シルバーレコードだ！");
        }
        //ブロンズ
        if (IsRange(time, 0, BronzeTime))
        {
            IsBronzeRank = true;
            Debug.Log("ブロンズレコードだ！");
        }
    }

    private bool IsRange(float value, float min, float max)
    {
        return (min <= value && value <= max);
    }
}
