using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : SceneBase
{

    //--- State -------------------------------------------
    [SerializeField]
    private RectTransform Selecter;
    [SerializeField]
    private GameObject NotContinueMessage;

    [Space(8)]
    [SerializeField]
    private GameObject SelectDisplay;
    [SerializeField]
    private GameObject[] select = new GameObject[4];

    [Space(4),SerializeField]
    private float WaitTime = 10f;

    //--- Internal ------------------------------
    private float time = 0f;
    private int SelectNum = 0;  //選択中のもの
    private int MaxNum = 0;
    private bool bUpdate = false;
    private bool bInput = false;
    private bool bWait = false;

    //--- MonoBehaviour -----------------------------------

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        MaxNum = select.Length;

        NotContinueMessage.SetActive(false);

        Selecter.localPosition = select[0].transform.localPosition + (Vector3.right * Selecter.localPosition.x);

        for (int i = 0; i < MaxNum; i++)
            select[i].transform.GetChild(0).gameObject.SetActive(false);

        select[0].transform.GetChild(0).gameObject.SetActive(true);

        time = 0f;
        bInput = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        time += Time.deltaTime;

        if (MySceneManager.bOption || bWait) return;

        int n = SelectNum;

        float selecter = Input.GetAxis(InputManager.Y_Selecter);

        if(selecter == 0)
            bInput = true;

        if(bInput)
        {
            if (selecter >= 0.5f)
            {
                SelectNum--;
                bInput = false;
            }
            else if (selecter <= -0.5f)
            {
                SelectNum++;
                bInput = false;
            }
        }

        if (SelectNum <= -1) SelectNum = MaxNum - 1;
        if (n != SelectNum)
        {
            SelectNum = SelectNum % MaxNum;

            for (int i = 0; i < MaxNum; i++)
            {
                select[i].transform.GetChild(0).gameObject.SetActive(SelectNum == i);
            }

            Selecter.localPosition = select[SelectNum].transform.localPosition + (Vector3.right * Selecter.localPosition.x);
        }

        if (Input.GetButtonDown(InputManager.Submit))
        {
            switch (SelectNum)
            {
                //Start
                case 0:
                        MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GameStart);
                    break;

                //Continue
                case 1:
                    Debug.Log("チェック");

                    if (DataManager.Instance.Continue())
                        MySceneManager.FadeInLoad(MySceneManager.Get_NowPlanet());
                    else
                    {
                        NotContinueMessage.SetActive(true);
                        bWait = true;
                        StartCoroutine("Continue_WaitInput");
                    }
                    
                    break;

                //Option
                case 2:
                    SceneManager.LoadScene(MySceneManager.Instance.Path_Option, LoadSceneMode.Additive);
                    break;

                //Exit
                case 3:
                    MySceneManager.Game_Exit();
                    break;

                default:
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_GalaxySelect);
                    break;
            }
        }

        //何か入力があれば
        if (Input.anyKey)
        {
            time = 0f;
        }
        else if(time >= WaitTime)
        {
            MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Opening);
        }
    }

    //--- Method ------------------------------------------

    private void bUpdate_Change()
    {
        bUpdate = true;
    }

    //--- IEnumerator -------------------------------------

    IEnumerator Continue_WaitInput()
    {
        yield return new WaitForSeconds(2.0f);
        while (!Input.anyKeyDown) { yield return 0; }
        NotContinueMessage.SetActive(false);
        bWait = false;
        yield return null;
    }

}
