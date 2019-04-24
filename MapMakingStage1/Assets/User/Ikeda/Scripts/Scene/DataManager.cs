using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    //--- Attribute ---------------------------------------
    
    //--- Status --------------------------------
    public int  nCrystalNum = 0; //所持している宝石数

    public int  nGalaxy_IsFinalSelect = 0;   //最後に遊んだ銀河
    public int  nPlanet_IsFinalSelect = 0;   //最後に遊んだ惑星

    public int  IsSave = 0;              //セーブデータがあるか(0:false,1:true)

    private string CrystalNumKey = "CrystalNum";
    private string GalaxyNumKey = "GalaxySelect";
    private string PlanetNumKey = "PlanetNum";
    private string IsSaveKey = "IsSave";

    //--- MonoBehaviour -----------------------------------

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start ()
    {
        nCrystalNum = 0;
        nGalaxy_IsFinalSelect = 0;
        nPlanet_IsFinalSelect = 0;
        IsSave = 0;
    }
	
    //--- Method ------------------------------------------

    public void ResetState()
    {
        nCrystalNum = 0;
        nGalaxy_IsFinalSelect = 0;
        nPlanet_IsFinalSelect = 0;
        IsSave = 0;
    }

    //全データ書き込み
    public void SaveAll()
    {
        IsSave = 1;
        PlayerPrefs.SetInt(CrystalNumKey,nCrystalNum);
        PlayerPrefs.SetInt(GalaxyNumKey,nGalaxy_IsFinalSelect);
        PlayerPrefs.SetInt(PlanetNumKey,nPlanet_IsFinalSelect);
        PlayerPrefs.SetInt(IsSaveKey,IsSave);

        PlayerPrefs.Save();

        Debug.Log("Saved!!:"+PlayerPrefs.GetInt(IsSave.ToString()));
    }

    //全データ読み込み
    public void LoadAll()
    {
        nCrystalNum             = PlayerPrefs.GetInt(CrystalNumKey,0);
        nGalaxy_IsFinalSelect   = PlayerPrefs.GetInt(GalaxyNumKey,0);
        nPlanet_IsFinalSelect   = PlayerPrefs.GetInt(PlanetNumKey,0);
        IsSave                  = PlayerPrefs.GetInt(IsSaveKey,0);

        Debug.Log("Load");
    }

    //全データ削除
    [ContextMenu("DeleteAll")]
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    [ContextMenu("Debug_Save")]
    public void Debug_Save()
    {
        PlayerPrefs.SetInt(CrystalNumKey, nCrystalNum);
        PlayerPrefs.SetInt(GalaxyNumKey, nGalaxy_IsFinalSelect);
        PlayerPrefs.SetInt(PlanetNumKey, nPlanet_IsFinalSelect);
        PlayerPrefs.SetInt(IsSaveKey, IsSave);

        PlayerPrefs.Save();
    }
}
