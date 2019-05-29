using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2 : TutorialBase {

    enum Step
    {
        Step1,
        Step2,
        Step3,
        Step4
    }
    Step step = Step.Step1;

    public Billbord uiX;
    public Billbord uiA;
    public Billbord uiB;
    public TutorialTrigger trigger;

    public float timer = 10.0f;

	// Use this for initialization
	void Start () {
        uiX.gameObject.SetActive(false);
        uiA.gameObject.SetActive(false);
        uiB.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		switch(step)
        {

            case Step.Step1:
                if(PlanetScene.Instance.state == PlanetScene.STATE.MAINGAME && FlagManager.Instance.flagActive)
                {
                    uiX.gameObject.SetActive(true);
                    step = Step.Step2;
                }
                break;
            case Step.Step2:
                if (CameraManager.Instance.planetCamera.gameObject.activeInHierarchy)
                {
                    uiX.gameObject.SetActive(false);
                    uiB.gameObject.SetActive(true);
                    step = Step.Step3;
                }
                break;
            case Step.Step3:
                if (Input.GetButtonDown(InputManager.Change_AscDes))
                {
                    uiB.gameObject.SetActive(false);
                    step = Step.Step4;
                }
                break;
            case Step.Step4:
                if (trigger.isTrigger)
                {
                    timer -= Time.deltaTime;

                    if(timer <= 0.0f)
                    {
                        if (FlagManager.Instance.flagActive)
                        {
                            uiA.gameObject.SetActive(true);
                            
                        }
                        else
                        {
                            uiA.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    uiA.gameObject.SetActive(false);
                }

                break;

        }
	}
}
