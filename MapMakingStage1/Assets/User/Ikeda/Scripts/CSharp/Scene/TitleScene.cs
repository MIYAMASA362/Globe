using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : SceneBase
{
    //--- Attribute -----------------------------------------------------------

    //--- State -------------------------------------------
    [Space(8)]
    [SerializeField]
    private RectTransform Selecter;

    [Space(8)]
    [SerializeField]
    private GameObject SelectDisplay;
    [SerializeField]
    private GameObject[] select = new GameObject[4];

    //--- Internal ------------------------------
    private int SelectNum = 0;          //選択中のもの
    private int MaxSelectNum = 0;       //最大セレクタ数
    private bool bInput = false;        //入力可否
    private bool IsContinue = false;    //コンティニュー可否

    //--- MonoBehaviour -------------------------------------------------------

    public void Awake()
    {
        bInput = false;
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        MySceneManager.IsPause_BackLoad = false;
        //背景読み込み
        SceneManager.LoadScene(MySceneManager.Instance.Path_BackGround,LoadSceneMode.Additive);

        //コンティニュー
        IsContinue = DataManager.Instance.playerData.IsContinue;

        Init_Select();

        Invoke("Loaded",4f);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (MySceneManager.IsOption || MySceneManager.IsLoading()) return;

        Update_Select();

        Select_Submit();

        Update_IdleTime();
    }

    //--- Method --------------------------------------------------------------

    //オープニングに再帰する
    private void Update_IdleTime()
    {
        if(Input.GetButtonDown(InputManager.Cancel))
            MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Opening, false);
    }

    //セレクトの初期化
    private void Init_Select()
    {
        MaxSelectNum = select.Length;
        if (IsContinue) SelectNum = 1;

        Selecter.localPosition = select[SelectNum].transform.localPosition + (Vector3.right * Selecter.localPosition.x);

        for (int i = 0; i < MaxSelectNum; i++)
            select[i].transform.GetChild(0).gameObject.SetActive(false);

        select[SelectNum].transform.GetChild(0).gameObject.SetActive(true);
    }

    //セレクトの更新
    private void Update_Select()
    {
        float selecter = Input.GetAxis(InputManager.Y_Selecter);
        if (selecter == 0) bInput = true;
        if (!bInput) return;

        int old = SelectNum;

        //セレクタ更新
        if (selecter >= 0.5f) SelectNum--;
        else if (selecter <= -0.5f) SelectNum++;

        //入力があった
        if (old == SelectNum) return;
        bInput = false;
        base.PlayAudio_Select();
        //コンティニュー出来ない
        if (!IsContinue && SelectNum == 1)
        {
            if (old < SelectNum)
                SelectNum++;
            else
                SelectNum--;
        }

        if (SelectNum <= -1) SelectNum = MaxSelectNum - 1;
        SelectNum = SelectNum % MaxSelectNum;

        for (int i = 0; i < MaxSelectNum; i++)
            select[i].transform.GetChild(0).gameObject.SetActive(SelectNum == i);

        //セレクタ位置更新
        Selecter.localPosition = select[SelectNum].transform.localPosition + (Vector3.right * Selecter.localPosition.x);
    }

    //セレクトされた物が決定された
    private void Select_Submit()
    {
        if (!Input.GetButtonDown(InputManager.Submit)) return;
        base.PlayAudio_Success();

        switch (SelectNum)
        {
            //Start
            case 0:
                DataManager.Instance.Reset_GameData();
                MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect, true);
                break;

            //Continue
            case 1:
                if (IsContinue)
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect, true);
                break;

            //Option
            case 2:
                MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Option, true);
                break;

            //Exit
            case 3:
                MySceneManager.Game_Exit();
                break;

            default:
                MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title, true);
                break;
        }
    }

    //ロード完了
    public void Loaded()
    {
        MySceneManager.Instance.CompleteLoaded();

        AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_TITLE);
    }

}
