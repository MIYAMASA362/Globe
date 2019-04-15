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
    //--- Attribute ---------------------------------------

    //--- private -------------------------------

    //共通化されいてるべきScene
    private static readonly string Title_Scene = SceneIndex.Ikeda_PlanetScene;
    private static readonly string Pause_Scene = SceneIndex.Pause;

    //--- Status ----------------------
    private static bool bPausing = false;       //Pause中:true

    //--- MonoBehavior ------------------------------------

    private void Awake()
    {
        //Awake に SceneManagerの関数などを使うとバグにつながる。
        //現象：Sceneが二重にロードされる

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SceneManager.LoadScene(Title_Scene);    //初期読み込み
    }

    private void Update()
    {
        bPausing = SceneManager.GetSceneByPath(Pause_Scene).isLoaded;
    }

    //No Coding
    private void LateUpdate()
    {

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
                SceneManager.LoadScene(Pause_Scene, LoadSceneMode.Additive);
            else
                SceneManager.UnloadSceneAsync(Pause_Scene);
        }
    }

    public static void Pause()
    {
        //Pause切り替え
        Pause(!bPausing);
    }

    //SceneManagerが良くわかない人が使う LoadScene
    //  path:   SceneIndex.ooo 読み込みたいシーン
    //  bAdd:   シーンを上に重ねるか[重ねる:true]
    //  bAsync: 非同期で読み込み[非同期:true]
    public static void LoadScene(string path, bool bAdd, bool bAsync)
    {
        if (bAdd)
        {
            if (bAsync) SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
            else SceneManager.LoadScene(path, LoadSceneMode.Additive);
        }
        else
        {
            if (bAsync) SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
            else SceneManager.LoadScene(path, LoadSceneMode.Single);
        }
    }

    public static void LoadScene(SceneIndex.ENUM_SCENE eNUM, bool bAdd, bool bAsync)
    {
        LoadScene(SceneIndex.Path_Index[(int)eNUM], bAdd,bAsync);
    }

    //SceneManagerが良く分からない人が使う UnLoadScene
    public static void UnLoadScene(string path)
    {
        SceneManager.UnloadSceneAsync(path);
    }

    public static void UnLoadScene(SceneIndex.ENUM_SCENE eNUM)
    {
        UnLoadScene(SceneIndex.Path_Index[(int)eNUM]);
    }

}