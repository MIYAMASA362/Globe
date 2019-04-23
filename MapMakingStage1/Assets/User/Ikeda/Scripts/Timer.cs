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

        TimerText.text = "Time:"+(int)time;
	}

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
