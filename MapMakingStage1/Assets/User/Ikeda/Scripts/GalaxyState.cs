using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GalaxyState : MonoBehaviour
{
    //--- Attribute -----------------------------------------------------------
    [Header("Galaxy Name")]
    [SerializeField]
    public string GalaxyName = "NONE";
    
    [Space(8)]
    [Header("StarCrystal State")]
    [SerializeField, Tooltip("星の宝石の総取得可能数")]
    public int nMaxStarCrystalNum = 0;
    [SerializeField, Tooltip("星の宝石の取得数")]
    public int nGetStarCrystalNum = 0;

    [Space(8)]
    [Header("Crystal State")]
    [SerializeField, Tooltip("隠し宝石の総取得可能数")]
    public int nMaxCrystalNum = 0;
    [SerializeField, Tooltip("隠し宝石の取得数")]
    public int nGetCrystalNum = 0;

    [Space(8)]
    [SerializeField, Tooltip("ロック解除要求隠し宝石数")]
    private int nUnLockCrystalNum = 0;

    [Space(8)]
    [SerializeField, Tooltip("ステージ親")]
    public GameObject PlanetParent = null;
    [SerializeField,Tooltip("ステージ惑星")]
    public PlanetState[] Planets = new PlanetState[5];

    //--- Internal State ------------------------
    [HideInInspector]
    public StageSelectScene selectScene;

    //--- MonoBehaviour -------------------------------------------------------

    private void Awake()
    {
       if(PlanetParent != null)
       {
            for(int i =0; i < PlanetParent.transform.childCount; i++)
            {
                GameObject child =  PlanetParent.transform.GetChild(i).gameObject;
                Planets[i] = child.transform.GetComponent<PlanetState>();
            }
       }
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //--- Method --------------------------------------------------------------

    public void InitState()
    {
        //星の宝石
        nMaxStarCrystalNum = 0;
        nGetStarCrystalNum = 0;

        //隠し宝石
        nMaxCrystalNum = 0;
        nGetCrystalNum = 0;

        //ステージの情報を集計
        foreach (PlanetState planetState in Planets)
        {
            if (planetState == null) continue;

            nMaxStarCrystalNum += planetState.nMaxStarCrystalNum;
            nGetStarCrystalNum += planetState.nGetStarCrystalNum;

            nMaxCrystalNum += planetState.nMaxCrystalNum;
            nGetCrystalNum += planetState.nGetCrystalNum;
        }
    }

    //Lockできるか
    public bool CheckLock(int nCrystalNum)
    {
        return nUnLockCrystalNum > nCrystalNum;
    }

    //隠し宝石差分個数
    public int Crtstal_Diffrence(int nCystalNum)
    {
        return nUnLockCrystalNum - nCystalNum;
    }

    //残りの星の宝石数
    public int StarCrystal_ReaminingNum()
    {
        return nMaxStarCrystalNum - nGetStarCrystalNum;
    }

    //残りの隠し宝石数
    public int Crystal_RemainingNum()
    {
        return nMaxCrystalNum - nGetCrystalNum;
    }
}
