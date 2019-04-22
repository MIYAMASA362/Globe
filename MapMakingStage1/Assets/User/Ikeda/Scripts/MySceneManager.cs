using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using TMPro;
using UnityEngine.UI;

//ここにManagerを永遠にDontDestroyOnLoadへ退避させるステップ作った奴は殺す。末代まで殺す。
//んで、読み込んだSceneをちゃんと精査しろ！って骨に彫ってやる。
public class MySceneManager : Singleton<MySceneManager>
{
    [System.Serializable]
    public class Galaxy
    {
        [Tooltip("惑星選択")]
        public SceneAsset Asset_PlanetSelect;
        [Tooltip("惑星")]
        public SceneAsset[] Asset_Planets;
    }

    //--- Attribute ---------------------------------------

    //--- public --------------------------------

    //--- private -------------------------------

    [Header("UI State")]
    [SerializeField,Tooltip("Debug：現在の選択Scene名")]
    private TextMeshProUGUI tm_text;
    [SerializeField,Tooltip("Fadeのアニメータ")]
    private Animator animator;

    [Header("Scene State")]
    [SerializeField,Tooltip("マネージャー管理Scene")]
    private SceneAsset Asset_ManagerScene;
    [SerializeField, Tooltip("Pause画面")]
    private SceneAsset Asset_PauseScene;

    [Space(10), Header("Title")]
    [SerializeField, Tooltip("オープニング動画")]
    private SceneAsset Asset_OpeningScene;
    [SerializeField, Tooltip("タイトルスタート")]
    private SceneAsset Asset_StartScene;
    [SerializeField,Tooltip("タイトル")]
    private SceneAsset Asset_TitleScene;
    [SerializeField, Tooltip("オプション")]
    private SceneAsset Asset_OpsitionScene;

    [Space(10)]
    [SerializeField,Tooltip("銀河選択")]
    public SceneAsset Asset_GalexySelect;

    [SerializeField, Tooltip("銀河")]
    public Galaxy[] galaxies;

    public static string OpeningScene { get; private set; }
    public static string TitleScene { get; private set; }
    public static string PauseScene { get; private set; }
    public static string GalaxySelect { get; private set; }
    public static string OpsitionScene { get; private set; }

    public static int nMaxGalaxyNum { get; private set; }
    public static int nMaxPlanetNum { get; private set; }

    //nSelecter State
    public static int nSelecter_Galaxy = 0;
    public static int nSelecter_Planet = 0;

    public static bool bPausing { get; private set; } //Pause中:true
    public static bool bOption  { get; private set; } //Option中:true
    public static bool bFadeing { get; private set; } //Fade中:true

    public static string NextLoadScene;
    public static bool bFade_Use;                     //FadeIn/Outを利用

    //--- MonoBehavior ------------------------------------

    private void Awake()
    {
        //Awake に SceneManagerの関数などを使うとバグにつながる。
        //現象：Sceneが二重にロードされる

        nMaxGalaxyNum = galaxies.Length;
        nMaxPlanetNum = galaxies[nSelecter_Galaxy].Asset_Planets.Length;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        bPausing = false;
        bOption = false;
        bFadeing = false;
        bFade_Use = false;

        OpeningScene = AssetDatabase.GetAssetPath(Asset_OpeningScene);
        TitleScene = AssetDatabase.GetAssetPath(Asset_TitleScene);
        PauseScene = AssetDatabase.GetAssetPath(Asset_PauseScene);
        GalaxySelect = AssetDatabase.GetAssetPath(Asset_GalexySelect);
        OpsitionScene = AssetDatabase.GetAssetPath(Asset_OpsitionScene);

        
        SceneManager.LoadScene(OpeningScene);    //初期読み込み
    }

    private void Update()
    {
        bPausing = SceneManager.GetSceneByPath(PauseScene).isLoaded;
        bOption = SceneManager.GetSceneByPath(OpsitionScene).isLoaded;
        nMaxPlanetNum = galaxies[nSelecter_Galaxy].Asset_Planets.Length;
        
    }

    private void LateUpdate()
    {
        if (bPausing)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        tm_text.text = "GalaxyNum:" + nSelecter_Galaxy
                      +"\nPlanetNum:" + nSelecter_Planet;
    }   

    //--- Method ------------------------------------------

    //Pause画面の表示　bEnable[表示:true/非表示:false]
    public static void Pause(bool bEnable)
    {
        //Fade中
        if (bFadeing) return;

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
     
    public static string Get_NowPlanet()
    {
        return AssetDatabase.GetAssetPath(Instance.galaxies[nSelecter_Galaxy].Asset_Planets[nSelecter_Planet]);
    }

    public static string Get_NowGalaxy()
    {
        return AssetDatabase.GetAssetPath(Instance.galaxies[nSelecter_Galaxy].Asset_PlanetSelect);
    }

    //次の銀河のPath 次がなければTitleへ
    public static string Get_NextGalaxy()
    {
        nSelecter_Galaxy++;

        if (nSelecter_Galaxy >= nMaxGalaxyNum)
            return TitleScene;

        return AssetDatabase.GetAssetPath(Instance.galaxies[nSelecter_Galaxy].Asset_PlanetSelect);
    }

    //次の惑星へのPath なければ
    public static string Get_NextPlanet()
    {
        nSelecter_Planet++;

        if (nSelecter_Planet >= nMaxPlanetNum)
            return AssetDatabase.GetAssetPath(Instance.galaxies[nSelecter_Galaxy].Asset_PlanetSelect);

        return AssetDatabase.GetAssetPath(Instance.galaxies[nSelecter_Galaxy].Asset_Planets[nSelecter_Planet]);
    }

    //SceneLoad
    public static void Load(string NextScene)
    {
        NextLoadScene = NextScene;
        SceneManager.LoadScene(NextLoadScene);
    }

    //Animatorを使ってFadeOut
    public static void FadeInLoad(string NextScene)
    {
        NextLoadScene = NextScene;
        Instance.animator.SetBool("FadeFlag",true);
    }

    //FadeOutが完了したらFadeIn
    public void FadeComplete()
    {
        SceneManager.LoadScene(NextLoadScene);
        Instance.animator.SetBool("FadeFlag", false);
    }

    //終了処理
    public static void Game_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }

    #region コメント箱
    //nSelecter Stateに沿った動き
    //public static void Load_Planet()
    //{
    //    string LoadScene = AssetDatabase.GetAssetPath(Instance.galaxies[nSelecter_Galaxy].Asset_Planets[nSelecter_Planet]);

    //    if (bFade_Use)
    //        FadeInLoad(LoadScene);
    //    else
    //        SceneManager.LoadScene(LoadScene);
    //}

    //nSelecter Stateに沿った動き
    //public static void Load_Galaxy()
    //{
    //    string LoadScene = AssetDatabase.GetAssetPath(Instance.galaxies[nSelecter_Galaxy].Asset_Galaxy);

    //    if (bFade_Use)
    //        FadeInLoad(LoadScene);
    //    else
    //        SceneManager.LoadScene(LoadScene);
    //}

    //次の星へ
    //public static void Next_LoadPlanet()
    //{
    //    nSelecter_Planet++;

    //    //それ以上ない
    //    if (nSelecter_Planet >= nMaxPlanetNum)
    //    {
    //        Load_Galaxy();
    //        return;
    //    }

    //    Load_Planet();
    //}



    //次の銀河へ
    //public static void Next_LoadGalaxy()
    //{
    //    nSelecter_Galaxy++;

    //    //それ以上ない
    //    if(nSelecter_Galaxy >= nMaxGalaxyNum)
    //    {
    //        SceneManager.LoadScene(TitleScene);
    //        return;
    //    }

    //    Load_Galaxy();
    //}

    #endregion

    //--- Editor ------------------------------------------

    [CanEditMultipleObjects]
    [CustomEditor(typeof(MySceneManager))]
    class ThisEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(4);
            if (GUILayout.Button("Apply To Build Settings")) BuildSetting();
        }

        public void BuildSetting()
        {
            var mySceneManager = target as MySceneManager;

            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

            //Manager Register
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_ManagerScene),true));

            //Title Register
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_TitleScene), true));

            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_PauseScene), true));

            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_GalexySelect), true));

            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_OpsitionScene), true));

            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mySceneManager.Asset_OpeningScene), true));

            foreach (var galaxy in mySceneManager.galaxies)
            {
                string galaxyPath = AssetDatabase.GetAssetPath(galaxy.Asset_PlanetSelect);
                if (!string.IsNullOrEmpty(galaxyPath))
                {
                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(galaxyPath, true));

                    foreach (var planet in galaxy.Asset_Planets)
                    {
                        string planetPath = AssetDatabase.GetAssetPath(planet);
                        if(!string.IsNullOrEmpty(planetPath))
                            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(planetPath, true));
                    }
                }
            }

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

            Debug.Log("Success!");
        }
    }

}