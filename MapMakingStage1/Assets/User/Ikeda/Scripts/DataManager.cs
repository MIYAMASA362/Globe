using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    //Playerのデータ
    [System.Serializable]
    public class PlayerData
    {
        public int CrystalNum = 0;  //クリスタル数

        public int SelectGalaxy = 0;
        public int SelectPlanet = 0;

        //デバッグ表示
        public void DebugLog()
        {
            Debug.Log("CrystalNum:" + CrystalNum);
            Debug.Log("SelectGalaxy:" + SelectGalaxy);
            Debug.Log("SelectPlanet:" + SelectPlanet);
        }
    }

    //Planetのデータ
    [System.Serializable]
    public class PlanetData
    {
        public bool clear = false;      //ゲームクリアの判定

        public int crystalNum = 0;       //取得済みのクリスタル数
        public bool[] bGet = {false};    //未取得のクリスタル判定

        public bool[] rank = {false,false,false};   //取得ランク

        public void DebugLog()
        {
            Debug.Log("Clear:" + clear);
            Debug.Log("CrystalNum:" + crystalNum);
            Debug.Log("bGet:" + bGet);
            Debug.Log("Rank:" + rank);
        }
    }

    //Galaxyのデータ
    [System.Serializable]
    public class GalaxyData
    {
        public int planetNum = 0;   //星数
        public int clearPlanet = 0; //クリア星数
    }

    //Configデータ
    [System.Serializable]
    public class ConfigData
    {

    }

    //--- Attribute ---------------------------------------

    //--- const ---------------------------------
    //データファイル
    static public readonly string DATA_FILE = "/Resources/local";

    static public readonly string PLAYER_FILE = "player.player";
    static public readonly string PLANET_FILE = "planet.text";
    static public readonly string GALAXY_FILE = "galaxy.text";
    static public readonly string CONFIG_FILE = "config.text";

    //--- public --------------------------------

    [Header("PlayerStatus")]
    [SerializeField]
    public PlayerData playerData =  new PlayerData();

    //--- MonoBehaviour -----------------------------------

    private void Awake()
    {
        DontDestroyOnLoad(this);
        playerData = new PlayerData();

        //保存先のディレクトリが存在するかの確認
        Create_LoaclDirectory();
    }

    // Use this for initialization
    void Start ()
    {

    }

    //--- Method ------------------------------------------

    //--- Data ----------------------------------

    //
    //  DataPath
    //
    private string DataPath(string FileName)
    { 
        return Application.dataPath + DATA_FILE + "/" + FileName;
    }

    //
    //  ファイル検索
    //
    public bool FileFind(string FileName)
    {
        FileInfo info = new FileInfo(DataPath(FileName));
        return info.Exists || info.Length != 0;
    }

    //
    //  Continue
    //
    public bool Continue()
    {
        //データがある
        if (Instance.FileFind(PLAYER_FILE))
        {
            Instance.Load(ref Instance.playerData,PLAYER_FILE);
            Debug.Log("Find");
            return true;
        }
        //データがない
        else
        {
            Debug.Log("Not Find");
            Instance.Save(ref Instance.playerData, PLAYER_FILE);
            return false;
        }
    }

    //
    //  Save
    //
    public bool Save<Type>(ref Type data,string TextFileName)
    {
        if(data == null)
        {
            Debug.LogWarning("DataがNullです"+"[Type:"+data.ToString()+"]");
            return false;
        }

        var path = DataPath(TextFileName);
        FileInfo info = new FileInfo(path);

        var json = JsonUtility.ToJson(data);
        var writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
        
        return true;
    }

    //
    //  Load
    //
    public bool Load<Type>(ref Type data,string TextFileName)
    {
        FileInfo info = new FileInfo(DataPath(TextFileName));
        if (!info.Exists || info.Length == 0) return false;

        var render = new StreamReader(info.OpenRead());
        var json = render.ReadToEnd();
        data = JsonUtility.FromJson<Type>(json);

        return true;
    }

    //
    //  データを初期化する
    //
    [ContextMenu("Reset")]
    public bool ReSet()
    {
        Delete_LocalDirectroy();
        playerData = new PlayerData();
        Save(ref playerData,PLAYER_FILE);

        return true;
    }

    //--- Directory -----------------------------

    //
    //  LoaclDirectoryの有無を確認して生成
    //
    private void Create_LoaclDirectory()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.dataPath + DATA_FILE);
        if (!directory.Exists) directory.Create();
    }

    //
    //  LoaclDirectory内を削除する(.playerは残す)
    //
    private void Delete_LocalDirectroy()
    {
        string path = Application.dataPath + DATA_FILE;

        DirectoryInfo directory = new DirectoryInfo(path);
        if (!directory.Exists) return;

        string PlayerFile = DataPath(PLAYER_FILE);

        foreach (FileInfo file in directory.GetFiles())
        {
            if (file.Extension != ".player" || file.Extension != ".player.meta")
            {
                file.Attributes = FileAttributes.Normal;
                file.Delete();
            }
        }
    }
}