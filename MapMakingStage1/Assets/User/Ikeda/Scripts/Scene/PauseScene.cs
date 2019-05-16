using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseScene : SceneBase
{
    [SerializeField, Tooltip("ステージ名")]
    private TextMeshProUGUI tm_StateName;

    [SerializeField]
    private RectTransform[] ContentUI;
    private RectTransform[] KeepContentUI;

    [SerializeField]
    private RectTransform SelectingUI;

    [SerializeField]
    private int SelectNum = 0;

    private int MaxNum = 5;
    private float PopX = 30f;

    private bool bInput = false;

	// Use this for initialization
	public override void Start ()
    {
        tm_StateName.text = SceneManager.GetActiveScene().name;
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

    }
	
	// Update is called once per frame
	public override void Update ()
    {
        if (tm_StateName.text != MySceneManager.PlanetName)
            tm_StateName.text = MySceneManager.PlanetName;

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
                    
                    break;
                case 1:
                    MySceneManager.FadeInLoad(MySceneManager.Get_NowPlanet(),true);
                    break;
                case 2:
                    StageSelectScene.Load_Star_PlanetSelect();
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect,false);
                    break;
                case 3:
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect,false);
                    break;
                case 4:
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title, false);
                    break;
                default:
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title, false);
                    break;
            }
        }

        
    }
}
