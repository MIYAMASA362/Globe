using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataCheckScene : SceneBase
{
    //--- Attribute ---------------------------------------

    //--- State ---------------------------------
    [SerializeField]
    private GameObject Selecter;
    [SerializeField]
    private RectTransform Yes;
    [SerializeField]
    private RectTransform No;

    private bool IsNewData = false;
    private bool bInput = false;

    //--- MonoBehaviour -----------------------------------
    public override void Start()
    {
        base.Start();

        Selecter.SetActive(false);

        IsNewData = false;
        bInput = false;

        Invoke("Set_Input",3f);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown(InputManager.Cancel)) MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title);

        if (!bInput) return;

        if (Input.GetAxis(InputManager.X_Selecter) >= 0.5f) IsNewData = false;
        if (Input.GetAxis(InputManager.X_Selecter) <= -0.5f) IsNewData = true;

        if (IsNewData)
            Selecter.transform.position = Yes.transform.position;
        else
            Selecter.transform.position = No.transform.position;

        if (Input.GetButtonDown(InputManager.Submit))
        {
            //新規で作るならデータを初期化
            if (IsNewData)
            {
                DataManager.Instance.ReSet();
                SceneManager.LoadScene(MySceneManager.Instance.Path_GameStart);
            }
            else
            {
                DataManager.Instance.Load(ref DataManager.Instance.playerData,DataManager.PLAYER_FILE);
                MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect);
            }
        }
    }

    //--- Method ------------------------------------------

    public void Set_Input()
    {
        bInput = true;
        Selecter.SetActive(true);
    }
}
