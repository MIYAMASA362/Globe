using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetState : MonoBehaviour
{
    [Header("Planet Name")]
    [SerializeField]
    public string PlanetName = "NONE";

    [Header("StarCrystal State")]
    [SerializeField, Tooltip("星の宝石の総取得可能数")]
    public int nMaxStarCrystalNum;
    [SerializeField, Tooltip("星の宝石の取得数")]
    public int nGetStarCrystalNum;

    [Space(8)]
    [Header("Crystal State")]
    [SerializeField, Tooltip("隠し宝石の総取得可能数")]
    public int nMaxCrystalNum;
    [SerializeField, Tooltip("隠し宝石の取得数")]
    public int nGetCrystalNum;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
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
