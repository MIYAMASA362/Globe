using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class PauseScene : SceneBase
{
    [SerializeField, Tooltip("ステージ名")]
    private TextMeshProUGUI tm_StateName;

    [SerializeField]
    private RectTransform[] ContentUI;

    [SerializeField]
    private RectTransform SelectingUI;

    [SerializeField]
    private int SelectNum = 0;
    private int MaxNum = 5;

    private bool bInput = false;

	// Use this for initialization
	public override void Start ()
    {
        tm_StateName.text = SceneManager.GetActiveScene().name;
        bInput = false;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        //Selectの変更
        int n = SelectNum;
        float selecter = Input.GetAxis(InputManager.Y_Selecter);

        if (selecter == 0) bInput = true;

        if (bInput)
        {
            if (selecter >= 0.5f)
            {
                SelectNum--;
                bInput = false;
            }
            if (selecter <= -0.5f)
            {
                SelectNum++;
                bInput = false;
            }
        }
        if (SelectNum <= -1) SelectNum = MaxNum - 1;
        if(n != SelectNum) SelectNum = SelectNum % MaxNum;

        SelectingUI.position = ContentUI[SelectNum].position;

        //決定されたとき
        if (Input.GetButtonDown(InputManager.Submit))
        {
            SceneManager.UnloadSceneAsync(MySceneManager.PauseScene);
            //遷移
            switch (SelectNum)
            {
                
                case 0:
                    
                    break;
                case 1:
                    MySceneManager.FadeInLoad(MySceneManager.Get_NowPlanet());
                    break;
                case 2:
                    MySceneManager.FadeInLoad(MySceneManager.Get_NowGalaxy());
                    break;
                case 3:
                    MySceneManager.FadeInLoad(MySceneManager.GalaxySelect);
                    break;
                case 4:
                    MySceneManager.FadeInLoad(MySceneManager.TitleScene);
                    break;
                default:
                    MySceneManager.FadeInLoad(MySceneManager.TitleScene);
                    break;
            }
        }

        
    }

    private void LateUpdate()
    {
        
    }


}
