using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class MySceneManager : Singleton<MySceneManager>
{
    //--- Class ---------------------------------------------------------------

    [System.Serializable]
    public class Planet
    {
        [HideInInspector]public string name = "";
        public string Path = "";
    }

    [System.Serializable]
    public class Galaxy
    {
        [HideInInspector] public string name = "";
        public int UnLockCrystalNum = 0;
        public List<Planet> Planets = new List<Planet>();
    }

    //--- Attribute -----------------------------------------------------------

    //--- public --------------------------------

    //--- private -------------------------------

    [Header("State")]
    [SerializeField,Tooltip("初期化時に最初のシーンを読み込む")]
    public bool bInitLoad = true;

    [Header("UI State")]
    [SerializeField, Tooltip("Fadeのアニメータ")] private Animator animator;

    [SerializeField] public string Path_Manager;
    [SerializeField] public string Path_Pause ;
    [SerializeField] public string Path_Opening;
    [SerializeField] public string Path_Title;
    [SerializeField] public string Path_Option;
    [SerializeField] public string Path_BackGround;
    [SerializeField] public string Path_DataCheck;
    [SerializeField] public string Path_GameStart;
    [SerializeField] public string Path_GalaxySelect;
    [SerializeField] public List<Galaxy> Galaxies;
    [SerializeField] public string Path_End;

    public static string NextLoadScene;

    public static bool IsPlayGame = false;              //ゲームをプレイできるか
    private static bool IsFade_Use = false;             //FadeIn/Outを利用
    private static bool IsLoad_Use = false;             //Loadを利用
    private static bool IsLoad_Pause = false;           //Pauseを読み込む

    //--- operation -----------------------------

    public static bool IsPausing { get; private set; }  //Pause中:true
    public static bool IsOption  { get; private set; }  //Option中:true
    public static bool IsFadeing { get; private set; }  //Fade中:true
    public static bool IsPause_BackLoad { get; set; }   //背後にPauseがある

    private static bool isRestart = false;

    public static void OnRestart()
    {
        isRestart = true;
    }

    public static bool IsRestart()
    {
        bool cur = isRestart;
        isRestart = false;
        return cur;
    }

    //--- MonoBehavior --------------------------------------------------------

    private void Awake()
    {
        //Awake に SceneManagerの関数などを使うとバグにつながる。
        //現象：Sceneが二重にロードされる
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //数値の初期化
        Init_Attribute();
        //初期画面
        if (bInitLoad) FadeInLoad(Path_Title, true);  
    }

    private void Update()
    {
        //数値の更新
        Update_Attribute();
    }

    private void LateUpdate()
    {
        
    }

    //--- Method --------------------------------------------------------------

    //--- Attributeの初期化 ---------------------
    private void Init_Attribute()
    {
        IsPausing = false;
        IsOption = false;
        IsFadeing = false;
        IsFade_Use = false;
        IsLoad_Use = false;
        IsPlayGame = false;
    }

    //--- Attributeの更新処理 -------------------
    private void Update_Attribute()
    {
        IsPausing = SceneManager.GetSceneByPath(Path_Pause).isLoaded;
        IsOption  = SceneManager.GetSceneByPath(Path_Option).isLoaded;
    }

    //--- Pause画面の表示　bEnable[表示:true/非表示:false] ---
    public static void Pause(bool bEnable)
    {
        //Fade中
        if (IsFadeing) return;

        //入力があるか
        if (!Input.GetButtonDown(InputManager.Menu)) return;

        if (IsOption) return;
        
        //Pause状態とbEnableが逆であるのか
        if (IsPausing == bEnable) return;

        if (bEnable)
            Instance.LoadPause();
        else
        {
            SceneManager.UnloadSceneAsync(Instance.Path_Pause);
            Instance.LoadBack_Pause();
        }
    }

    //--- 現在の惑星 ----------------------------
    public static string Get_NowPlanet()
    {
        return Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].Planets[DataManager.Instance.playerData.SelectPlanet].Path;
    }

    //--- 現在の銀河 ----------------------------
    public static string Get_NowGalaxy()
    {
        return Instance.Path_GalaxySelect;
    }

    public static string Get_NextGalaxyPlanet()
    {
        if (DataManager.Instance.playerData.GetStarCrystalNum >= Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].UnLockCrystalNum)
            return Get_NowPlanet();
        return Instance.Path_GalaxySelect;
    }

    //--- Planetの選択に戻る
    public static string Load_PlanetSelect()
    {
        StageSelectScene.Load_Star_PlanetSelect();
        return Instance.Path_GalaxySelect;
    }
    
    //--- 次のPlanetに移動 ---
    public static string Load_Next_Planet()
    {
        DataManager.Instance.playerData.SelectPlanet++;

        //最終ステージまでやった
        if(DataManager.Instance.playerData.SelectPlanet < Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].Planets.Count)
            return Get_NowPlanet();

        //次の銀河へ
        DataManager.Instance.playerData.SelectPlanet = 0;
        DataManager.Instance.playerData.SelectGalaxy++;
        if (DataManager.Instance.playerData.SelectGalaxy >= Instance.Galaxies.Count)
        {
            DataManager.Instance.playerData.SelectGalaxy = 0;
            return Instance.Path_End;
        }
        return Get_NextGalaxyPlanet();
    }

    //--- エンディング読み込み ------------------
    public static void Ending()
    {
        FadeInLoad(Instance.Path_End, true);
    }

    //--- 終了処理 ------------------------------
    public static void Game_Exit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
    #endif
    }

    //--- Pause ---------------------------------

    //--- バックで読み込みします ------
    public void LoadBack_Pause()
    {
        StartCoroutine("IE_LoadPause");
    }

    //--- バックで読み込んでいたのを有効化します ---
    public void LoadPause()
    {
        IsLoad_Pause = true;
        IsPause_BackLoad = true;
    }

    public void LoadOption()
    {
        if (IsPause_BackLoad)
            SceneManager.UnloadSceneAsync(Path_Pause);
        SceneManager.LoadScene(Path_Option,LoadSceneMode.Additive);
    }

    //--- Pause画面を登録 -----------------------
    IEnumerator IE_LoadPause()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(Instance.Path_Pause, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
        IsLoad_Pause = false;

        while (!IsLoad_Pause)
            yield return null;

        async.allowSceneActivation = true;
        IsLoad_Pause = false;

        yield return null;
    }

    //--- SceneLoad FadeInOut -------------------------------------------------

    //--- SceneLoad -----------------------------
    public static void Load(string NextScene)
    {
        NextLoadScene = NextScene;
        SceneManager.LoadScene(NextLoadScene);
    }

    //--- Animatorを使ってFadeIn IsLoad:ロード画面を使うか ---
    public static void FadeInLoad(string NextScene,bool IsLoad)
    {
        NextLoadScene = NextScene;
        IsFade_Use = true;
        IsLoad_Use = IsLoad;
        Instance.animator.SetBool("FadeFlag",true);
    }

    //--- Animator ------------------------------------------------------------

    //--- FadeOutが完了したらFadeOut ------------
    public void CompleteFadeOut()
    {
        SceneManager.LoadScene(NextLoadScene);
        Instance.animator.SetBool("FadeFlag", false);
        if (IsLoad_Use) return;
        Instance.animator.SetTrigger("LoadTrigger");
    }

    public void Start_Load()
    {
        Instance.animator.SetBool("FadeFlag", true);
//        IsLoad_Use = true;
    }

    //--- FadeInが完了した ----------------------
    public void CompleteFadeIn()
    {
        IsFade_Use = false;
    }

    //--- Load画面を終了 ------------------------
    public void CompleteLoaded()
    {
        Instance.animator.SetTrigger("LoadTrigger");
        IsLoad_Use = false;
    }

    //--- DataManager ---------------------------------------------------------

    //--- Fadeしている --------------------------
    public static bool Fading()
    {
        return IsFade_Use;
    }

    //--- Loadしている --------------------------
    public static bool IsLoading()
    {
        return IsLoad_Use;
    }

    public static string Get_GalaxyName()
    {
        return Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].name;
    }

    public static string Get_PlanetName()
    {
        return Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].Planets[DataManager.Instance.playerData.SelectPlanet].name;
    }
}