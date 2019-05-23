using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetOpening : MonoBehaviour 
{
    //--- Attribute -----------------------------------------------------------
    [Header("StageName State")]
    [SerializeField]
    private GameObject StageLabel;
    [SerializeField,Tooltip("ステージ名")]
    private TextMeshProUGUI tm_StageName;
    [SerializeField,Tooltip("PopUpに要する時間")]
    private float PopUp_Time = 0.25f;
    [SerializeField,Tooltip("PopUpする距離")]
    private float PopUp_MaxDistance = 800f;
    [SerializeField,Tooltip("右方向にPopUpする")]
    private bool PopUp_Right = false;

    //--- Internal --------------------------------------------------
    private bool IsEnable = false;
    private bool IsPopUp  = false;
    private float PopUpCount = 0f;
    private Vector3 HidePos;

    //--- MonoBehaviour -------------------------------------------------------

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start ()
    {
        Set_StageLabel();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Update_StageLabel();
	}

    //--- Method --------------------------------------------------------------
    
    //--- StageLabel ------------------------------------------------
    private void Set_StageLabel()
    {
        tm_StageName.text = MySceneManager.Get_PlanetName();
        int mag = PopUp_Right ? 1 : -1;
        HidePos = StageLabel.transform.position + (StageLabel.transform.right * PopUp_MaxDistance * mag);
        StageLabel.transform.position = HidePos;
    }

    private void Update_StageLabel()
    {
        if (!IsEnable) return;

        float time = PopUpCount / PopUp_Time;
        int mag = PopUp_Right ? -1 : 1;
        Vector3 target = IsPopUp ? HidePos + (StageLabel.transform.right * PopUp_MaxDistance * mag) : HidePos;

        StageLabel.transform.position = Vector3.Lerp(StageLabel.transform.position, target, time);
        if (time > 1.0) { IsEnable = false; return; }
        PopUpCount += Time.deltaTime;
    }

    [ContextMenu("PopUp")]
    public void PopUp_StageLabel()
    {
        PopUpCount = 0f;
        IsEnable = true;
        IsPopUp = true;
    }

    [ContextMenu("PopDown")]
    public void PopDown_StageLabel()
    {
        PopUpCount = 0f;
        IsEnable = true;
        IsPopUp = false;
    }

    
}
