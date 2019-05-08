using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetState : MonoBehaviour
{
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
}
