using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Timer))]
public class PlanetScene :SceneBase {

    //--- Timer State ---------------------------
    [SerializeField]
    private Timer timer;

    private bool bGameClear = false;

    //--- MonoBehaviour -----------------------------------

	// Use this for initialization
	public override void Start ()
    {
        timer = this.GetComponent<Timer>();
        timer.StartTimer();
        bGameClear = false;
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        if (bGameClear)
        {
            TimeBonus();
            MySceneManager.FadeInLoad(MySceneManager.Get_NextPlanet());
        }
	}

    //--- Method ------------------------------------------
    
    //--- Game ----------------------------------
    public void GameClear()
    {
        bGameClear = true;
        timer.StopTimer();
    }

    //--- Time ----------------------------------

    private void TimeBonus()
    {
        float time = timer.GetTime();

        //一分未満
        if (IsRange(time, 0, 60))
        {
            Debug.Log("一分未満だ！");
            return;
        }
        //二分未満
        else if (IsRange(time, 60, 120))
        {
            Debug.Log("二分未満だ！");
            return;
        }
    }

    //--- function ------------------------------

    //min 以上 max　未満かの判定
    private bool IsRange(float value,float min,float max)
    {
        return (min <= value && value < max);
    }
}
