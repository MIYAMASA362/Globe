using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {

    public float time { get; private set; }

    [SerializeField]
    private TextMeshProUGUI TimerText;

    [SerializeField]
    private float MaxTime = 60f* 10f;
    [SerializeField]
    private bool bCount = false;

    //--- MonoBhaviour ------------------------------------

    private void Awake()
    {
        time = 0f;
        bCount = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (bCount)
        {
            time += Time.deltaTime;
        }

        if (time >= MaxTime) time = MaxTime;

        int seconds = 0;
        int minutes = 0;

        if (time == 0)
        {
            seconds = 0;
            minutes = 0;
        }
        else
        {
            seconds = (int)time % 60;
            minutes = (int)time / 60;
        }

        TimerText.text = minutes.ToString("00")+":"+seconds.ToString("00");
	}

    //--- Method ------------------------------------------

    public void StartTimer()
    {
        time = 0f;
        bCount = true;
    }

    public void StopTimer()
    {
        bCount = false;
    }

    public float GetTime()
    {
        return time;
    }
}
