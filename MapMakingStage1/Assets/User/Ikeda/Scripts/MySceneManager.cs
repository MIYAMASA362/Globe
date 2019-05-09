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
    public class Galaxy
    {
        public string Path_PlanetSelect;
        public List<string> Path_Planets;
    }

    //--- Attribute -----------------------------------------------------------

    //--- public --------------------------------

    //--- private -------------------------------

    [Header("State")]
    [SerializeField,Tooltip("初期化時に最初のシーンを読み込む")]
    private bool bInitLoad = true;

    [Header("UI State")]
    [SerializeField, Tooltip("Fadeのアニメータ")] private Animator animator;

    [HideInInspector] public string Path_Manager;
    [HideInInspector] public string Path_Pause;
    [HideInInspector] public string Path_Opening;
    [HideInInspector] public string Path_Title;
    [HideInInspector] public string Path_Option;
    [HideInInspector] public string Path_DataCheck;
    [HideInInspector] public string Path_GameStart;
    [HideInInspector] public string Path_GalaxySelect;
    [HideInInspector] public List<Galaxy> Galaxies;

    public static string NextLoadScene;
    public static bool IsPlayGame = false;              //ゲームをプレイできるか
    private static bool IsFade_Use = false;             //FadeIn/Outを利用
    private static bool IsLoad_Use = false;             //Loadを利用

    //--- operation -----------------------------
    public static int nMaxGalaxyNum { get; private set; }
    public static int nMaxPlanetNum { get; private set; }

    public static bool IsPausing { get; private set; }  //Pause中:true
    public static bool IsOption  { get; private set; }  //Option中:true
    public static bool IsFadeing { get; private set; }  //Fade中:true

    //--- MonoBehavior --------------------------------------------------------

    private void Awake()
    {
        //Awake に SceneManagerの関数などを使うとバグにつながる。
        //現象：Sceneが二重にロードされる

        nMaxGalaxyNum = Galaxies.Count;
        nMaxPlanetNum = Galaxies[0].Path_Planets.Count;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //数値の初期化
        Init_Attribute();

        //初期画面
        if(bInitLoad) SceneManager.LoadScene(Path_Opening);   
    }

    private void Update()
    {
        //数値の更新
        Update_Attribute();
    }

    private void LateUpdate()
    {
        if (IsPausing)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
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
        nMaxPlanetNum = Galaxies[DataManager.Instance.playerData.SelectGalaxy].Path_Planets.Count;
    }

    //--- Pause画面の表示　bEnable[表示:true/非表示:false] ---
    public static void Pause(bool bEnable)
    {
        //Fade中
        if (IsFadeing) return;

        //入力があるか
        if (!Input.GetButtonDown(InputManager.Menu)) return;

        //Pause状態とbEnableが逆であるのか
        if (IsPausing == bEnable) return;

        if (bEnable)
            SceneManager.LoadScene(Instance.Path_Pause,LoadSceneMode.Additive);
        else
            SceneManager.UnloadSceneAsync(Instance.Path_Pause);
    }

    //--- 現在の惑星 ----------------------------
    public static string Get_NowPlanet()
    {
        return Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].Path_Planets[DataManager.Instance.playerData.SelectPlanet];
    }

    //--- 現在の銀河 ----------------------------
    public static string Get_NowGalaxy()
    {
        return Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].Path_PlanetSelect;
    }

    //--- 次の銀河のPath 次がなければTitleへ ---
    public static string Get_NextGalaxy()
    {
        DataManager.Instance.playerData.SelectGalaxy++;

        if (DataManager.Instance.playerData.SelectGalaxy > nMaxGalaxyNum-1)
        {
            DataManager.Instance.playerData.SelectGalaxy = 0;
            return Instance.Path_Title;
        }
        return Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].Path_PlanetSelect;
    }

    //--- 次の惑星へのPath なければPlanetSelectへ ---
    public static string Get_NextPlanet()
    {
        DataManager.Instance.playerData.SelectPlanet++;

        if (DataManager.Instance.playerData.SelectPlanet > nMaxPlanetNum-1)
        {
            DataManager.Instance.playerData.SelectPlanet = 0;
            return Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].Path_PlanetSelect;
        }
        return Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].Path_Planets[DataManager.Instance.playerData.SelectGalaxy];
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

}