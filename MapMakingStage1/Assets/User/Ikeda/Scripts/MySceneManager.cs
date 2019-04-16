using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

//ここにManagerを永遠にDontDestroyOnLoadへ退避させるステップ作った奴は殺す。末代まで殺す。
//んで、読み込んだSceneをちゃんと精査しろ！って骨に彫ってやる。
public class MySceneManager : Singleton<MySceneManager>
{
    [System.Serializable]
    public class Galaxy
    {
        [Tooltip("銀河")]
        public SceneAsset Asset_Galaxy;
        [Tooltip("惑星")]
        public SceneAsset[] Asset_Planets;
    }

    //--- Attribute ---------------------------------------

    //--- public --------------------------------

    //--- private -------------------------------

    [SerializeField,Tooltip("マネージャー管理Scene")]
    private SceneAsset Asset_ManagerScene;
    [SerializeField,Tooltip("タイトル")]
    private SceneAsset Asset_TitleScene;
    [SerializeField,Tooltip("Pause画面")]
    private SceneAsset Asset_PauseScene;
    [SerializeField,Tooltip("銀河選択")]
    public SceneAsset Asset_GalexySelect;

    [SerializeField, Tooltip("銀河")]
    public Galaxy[] galaxies;

    public static string TitleScene
    {
        get;
        private set;
    }
    public static string PauseScene
    {
        get;
        private set;
    }
    public static string GalaxySelect
    {
        get;
        private set;
    }

    public static bool bPausing
    {
        get;
        private set;
    }   //Pause中:true
    public static int nMaxGalaxyNum
    {
        get;
        private set;
    }
    public static int nMaxPlanetNum
    {
        get;
        private set;
    }

    public static int nSelecter_Galaxy = 0;
    public static int nSelecter_Planet = 0;

    //--- MonoBehavior ------------------------------------

    private void Awake()
    {
        //Awake に SceneManagerの関数などを使うとバグにつながる。
        //現象：Sceneが二重にロードされる

        nMaxGalaxyNum = galaxies.Length;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        bPausing = false;

        TitleScene = AssetDatabase.GetAssetPath(Asset_TitleScene);
        PauseScene = AssetDatabase.GetAssetPath(Asset_PauseScene);
        GalaxySelect = AssetDatabase.GetAssetPath(Asset_GalexySelect);

        SceneManager.LoadScene(TitleScene);    //初期読み込み
    }

    private void Update()
    {
        bPausing = SceneManager.GetSceneByPath(PauseScene).isLoaded;
    }

    private void LateUpdate()
    {
        nMaxPlanetNum = MySceneManager.Instance.galaxies[nSelecter_Galaxy].Asset_Planets.Length;
        if (bPausing)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }   

    //--- Method ------------------------------------------

    //Pause画面の表示　bEnable[表示:true/非表示:false]
    public static void Pause(bool bEnable)
    {
        //入力があるか
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        //Pause状態とbEnableが逆であるのか
        if (bPausing != bEnable)
        {
            if (bEnable)
                SceneManager.LoadScene(PauseScene,LoadSceneMode.Additive);
            else
                SceneManager.UnloadSceneAsync(PauseScene);
        }
    }

    public static void Load_Planet()
    {
        SceneManager.LoadScene(AssetDatabase.GetAssetPath(Instance.galaxies[nSelecter_Galaxy].Asset_Planets[nSelecter_Planet]));
    }

    public static void Load_Galaxy()
    {
        SceneManager.LoadScene(AssetDatabase.GetAssetPath(Instance.galaxies[nSelecter_Galaxy].Asset_Galaxy));
    }

    public static void Next_LoadPlanet()
    {
        nSelecter_Planet++;

        //それ以上ない
        if (nSelecter_Planet >= nMaxPlanetNum)
        {
            Load_Galaxy();
            return;
        }

        Load_Planet();
    }

    public static void Next_LoadGalaxy()
    {
        nSelecter_Galaxy++;

        //それ以上ない
        if(nSelecter_Galaxy >= nMaxGalaxyNum)
        {
            SceneManager.LoadScene(TitleScene);
            return;
        }

        Load_Galaxy();
    }
}