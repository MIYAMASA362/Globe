using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseScene : SceneBase
{
    //--- Attribute -------------------------------------------------
    [SerializeField, Tooltip("ステージ名")]
    private TextMeshProUGUI tm_StateName;

    [SerializeField]
    private RectTransform[] ContentUI;
    private RectTransform[] KeepContentUI;

    [SerializeField]
    private RectTransform SelectingUI;

    [SerializeField]
    private int SelectNum = 0;

    [SerializeField]
    private TextMeshProUGUI StageNum;

    private int MaxNum = 5;
    private float PopX = 30f;

    private bool bInput = false;

    //--- MonoBehaviour ---------------------------------------------

	// Use this for initialization
	public override void Start ()
    {
        tm_StateName.text = MySceneManager.Get_PlanetName();
        SelectingUI.gameObject.SetActive(true);
        bInput = false;

        //初期値保存
        KeepContentUI = new RectTransform[ContentUI.Length];
        for (int i = 0; i < ContentUI.Length; i++)
        {
            KeepContentUI[i] = ContentUI[i];
        }

        SelectNum = 0;
        ContentUI[SelectNum].position += KeepContentUI[SelectNum].transform.right * PopX;
        SelectingUI.position = ContentUI[SelectNum].position;
        StageNum.text = (DataManager.Instance.playerData.SelectGalaxy + 1).ToString() + "-" + (DataManager.Instance.playerData.SelectPlanet + 1).ToString();
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        if (MySceneManager.IsOption) return;

        //Selectの変更
        int n = SelectNum;
        float selecter = Input.GetAxis(InputManager.Y_Selecter);

        if (selecter == 0) bInput = true;

        if (bInput)
        {
            if (selecter >= 0.5f || Input.GetKeyDown(KeyCode.W))
            {
                SelectNum--;
                bInput = false;
            }

            if (selecter <= -0.5f || Input.GetKeyDown(KeyCode.S))
            {
                SelectNum++;
                bInput = false;
            }
        }
        if (SelectNum <= -1) SelectNum = MaxNum - 1;
        if (n != SelectNum)
        {
            base.PlayAudio_Select();
            SelectNum = SelectNum % MaxNum;

            ContentUI[n].position -= KeepContentUI[n].transform.right * PopX;
            ContentUI[SelectNum].position += KeepContentUI[SelectNum].transform.right * PopX;

            SelectingUI.position = ContentUI[SelectNum].position;
        }

        //決定されたとき
        if (Input.GetButtonDown(InputManager.Submit) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.UnloadSceneAsync(MySceneManager.Instance.Path_Pause);
            //遷移
            switch (SelectNum)
            {
                case 0:
                    base.PlayAudio_Return();
                    MySceneManager.Instance.LoadBack_Pause();
                    break;
                case 1:
                    base.PlayAudio_Success();
                    AudioManager.Instance.StopBGM();
                    MySceneManager.OnRestart();
                    MySceneManager.FadeInLoad(MySceneManager.Get_NowPlanet(),true);
                    break;
                case 2:
                    base.PlayAudio_Success();
                    AudioManager.Instance.StopBGM();
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect,true);
                    break;
                case 3:
                    base.PlayAudio_Success();
                    MySceneManager.Instance.LoadOption();
                    break;
                case 4:
                    base.PlayAudio_Success();
                    AudioManager.Instance.StopBGM();
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title, true);
                    break;
                default:
                    base.PlayAudio_Success();
                    AudioManager.Instance.StopBGM();
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title, true);
                    break;
            }
        }
    }
}
