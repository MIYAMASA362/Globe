using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : TutorialBase {

    enum Step
    {
        None,
        Step1,
        Step2,
        Step3
    }
    Step step;

    public Billbord uiA;
    public Billbord uiB;
    public Billbord uiLBRB;

    // Use this for initialization
    void Start () {
        step = Step.None;

        uiA.gameObject.SetActive(false);
        uiB.gameObject.SetActive(false);
        uiLBRB.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		

        switch(step)
        {
            case Step.None:
                if (PlanetScene.Instance.state == PlanetScene.STATE.MAINGAME)
                {
                    uiA.gameObject.SetActive(true);
                    step = Step.Step1;
                }
                break;
            case Step.Step1:
                if(FlagManager.Instance.flagActive)
                {
                    uiA.gameObject.SetActive(false);
                    uiB.gameObject.SetActive(true);
                    step = Step.Step2;
                }
                break;
            case Step.Step2:
                if (FlagManager.Instance.flagActive)
                {
                    uiB.gameObject.SetActive(true);
                    if (Input.GetButtonDown(InputManager.Change_AscDes))
                    {
                        uiB.gameObject.SetActive(false);
                        uiLBRB.gameObject.SetActive(true);
                        step = Step.Step3;
                    }
                }
                else
                {
                    uiB.gameObject.SetActive(false);
                }

                break;
            case Step.Step3:
                if (FlagManager.Instance.flagActive)
                {
                    uiLBRB.gameObject.SetActive(true);

                    if (Input.GetButtonDown(InputManager.Left_AxisRotation) ||
                    Input.GetButtonDown(InputManager.Right_AxisRotation))
                    {
                        uiLBRB.gameObject.SetActive(false);
                        GameObject.Destroy(this.gameObject);
                    }
                }
                else
                {
                    uiLBRB.gameObject.SetActive(false);
                }
                break;
        }
	}
}
