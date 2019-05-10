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
        public List<string> Path_Planets = new List<string>();
    }

    //--- Attribute -----------------------------------------------------------

    //--- public --------------------------------

    //--- private -------------------------------

    [Header("State")]
    [SerializeField,Tooltip("初期化時に最初のシーンを読み込む")]
    private bool bInitLoad = true;

    [Header("UI State")]
    [SerializeField, Tooltip("Fadeのアニメータ")] private Animator animator;

    public string Path_Manager;
    public string Path_Pause ;
    public string Path_Opening;
    public string Path_Title;
    public string Path_Option;
    public string Path_DataCheck;
    public string Path_GameStart;
    public string Path_GalaxySelect;
    public List<Galaxy> Galaxies;

    public static string NextLoadScene;
    public static bool IsPlayGame = false;              //ゲームをプレイできるか
    private static bool IsFade_Use = false;             //FadeIn/Outを利用
    private static bool IsLoad_Use = false;             //Loadを利用
    private static bool IsLoad_Pause = false;            //Pauseを読み込む

    //--- operation -----------------------------

    public static bool IsPausing { get; private set; }  //Pause中:true
    public static bool IsOption  { get; private set; }  //Option中:true
    public static bool IsFadeing { get; private set; }  //Fade中:true


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

    //--- Coroutine -----------------------------------------------------------

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
        return Instance.Galaxies[DataManager.Instance.playerData.SelectGalaxy].Path_Planets[DataManager.Instance.playerData.SelectPlanet];
    }

    //--- 現在の銀河 ----------------------------
    public static string Get_NowGalaxy()
    {
        return Instance.Path_GalaxySelect;
    }

    //--- Planetの選択に戻る
    public static string Load_PlanetSelect()
    {
        StageSelectScene.Load_Star_PlanetSelect();
        return Instance.Path_GalaxySelect;
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