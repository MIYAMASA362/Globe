using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using XInputDotNetPure;

public class OptionScene : SceneBase
{
    //--- Attribute -----------------------------------------------------------

    [SerializeField]
    private RectTransform SelecterObj;

    [SerializeField]
    private RectTransform[] SelectContent = new RectTransform[5];

    private int SelectNum = 0;
    private int MaxSelect = 0;
    private bool IsInput = false;
    private bool IsSetting_Bind= false;     //変更するコンテンツに向け操作をバインド
    private bool IsChanged = false;

    [Space(10)]
    [Header("BGM")]
    [SerializeField] private Slider BGM_Slider;
    [SerializeField] private TextMeshProUGUI BGM_ParameterText;
    [SerializeField] private float BGM_Volume = 0f;

    [Space(10)]
    [Header("SE")]
    [SerializeField] private Slider SE_Slider;
    [SerializeField] private TextMeshProUGUI SE_ParameterText;
    [SerializeField] private float SE_Volume = 0f;

    [Space(10)]
    [Header("CameraReverseVertical")]
    [SerializeField] private TextMeshProUGUI CR_V_ParameterText;
    [SerializeField] private bool IsCR_V = false;

    [Space(10)]
    [Header("CameraReverseHorizontal")]
    [SerializeField] private TextMeshProUGUI CR_H_ParameterText;
    [SerializeField] private bool IsCR_H = false;

    [Space(10)]
    [Header("ControllerVibration")]
    [SerializeField] private TextMeshProUGUI CVibrationEnable_ParameterText;
    [SerializeField] private bool IsVibration = false;
    [SerializeField] private TextMeshProUGUI CVibrationValue_ParameterText;
    [SerializeField] private Slider CVibration_Slider;
    [SerializeField] private float fVibration = 0f;

    [Space(10)]
    [Header("SuportUI")]
    [SerializeField] TextMeshProUGUI SuportUI_ParameterText;
    [SerializeField] bool IsSuport = false;

    [Space(10)]
    [Header("Register")]
    [SerializeField] private GameObject RegisterMessage;

    [Space(10)]
    [Header("State")]
    [SerializeField] private float ChangeValue = 0.0f;
    [SerializeField] private float ChangeValueAccel = 0.1f;
    [SerializeField] private float ChangeValueMax = 0.5f;
    [SerializeField] private GameObject Config_Value;
    [SerializeField] private GameObject Config_Enable;

    //--- MonoBehaviour -------------------------------------------------------

	// Use this for initialization
	public override void Start ()
    {
        if(!MySceneManager.IsPause_BackLoad)
            SceneManager.LoadScene(MySceneManager.Instance.Path_BackGround,LoadSceneMode.Additive);
        else
            MySceneManager.Instance.LoadBack_Pause();

        LoadState_CommonData();
        Init_Setting();

        MaxSelect = SelectContent.Length;

        SelectNum = 0;
        SelecterObj.localPosition = SelectContent[SelectNum].localPosition;

        if(!MySceneManager.IsPause_BackLoad)
            MySceneManager.Instance.CompleteLoaded();
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        //コンテンツ選択
        Content_Selecter();

        //コンテンツ設定変更
        Content_Setting();
    }

    //--- Method --------------------------------------------------------------
    void Content_Selecter()
    {
        int OldSelect = SelectNum;

        float selecter = Input.GetAxis(InputManager.Y_Selecter);

        if (selecter == 0)
            IsInput = true;

        if (IsInput)
        {
            if (selecter >= 0.5f)
                SelectNum--;
            else if (selecter <= -0.5f)
                SelectNum++;
        }

        if (OldSelect != SelectNum)
        {
            IsInput = false;
            base.PlayAudio_Select();
            if (!IsVibration && SelectNum == 5)
                if (OldSelect < SelectNum)
                    SelectNum++;
                else
                    SelectNum--;

            if (SelectNum <= -1) SelectNum = MaxSelect - 1;
            SelectNum = SelectNum % MaxSelect;

            SelecterObj.localPosition = SelectContent[SelectNum].transform.localPosition;
        }
    }

    void Content_Setting()
    {
        switch (SelectNum)
        {
            //BGMボリューム
            case 0:
                BGM_Setting();
                break;
            //SEボリューム
            case 1:
                SE_Setting();
                break;
            //カメラ上下反転
            case 2:
                CameraReverseVertical_Setting();
                break;
            //カメラ左右反転
            case 3:
                CameraReverseHorizontal_Setting();
                break;
            //振動有効・無効
            case 4:
                ControllerVibrationEnable_Setting();
                break;
            //振動値
            case 5:
                ControllerVibration_Setting();
                break;
            //SuportUI
            case 6:
                SuportUI_Setting();
                break;
            //デフォルト
            case 7:
                ConfigUI(false,false);
                if (!Input.GetButtonDown(InputManager.Submit)) return;
                base.PlayAudio_Success();
                Default_Setting();
                break;
            //もどる
            case 8:
                ConfigUI(false,false);
                if (!Input.GetButtonDown(InputManager.Submit)) return;
                base.PlayAudio_Success();
                DataManager.Instance.CommonData_Save();
                if (MySceneManager.IsPause_BackLoad)
                {
                    SceneManager.UnloadSceneAsync(MySceneManager.Instance.Path_Option);
                    MySceneManager.Instance.LoadPause();
                }
                else
                    MySceneManager.FadeInLoad(MySceneManager.Instance.Path_Title, true);
                break;
            default:
                break;
        }
    }

    //コンフィグ表示変更
    void ConfigUI(bool IsEnable, bool IsValueConfig)
    {
        Config_Value.SetActive(false);
        Config_Enable.SetActive(false);

        if (!IsEnable) return;

        if (IsValueConfig)
            Config_Value.SetActive(true);
        else
            Config_Enable.SetActive(true);
    }

    //初期化設定
    void Init_Setting()
    {
        BGM_Setting();
        SE_Setting();
        CameraReverseVertical_Setting();
        CameraReverseHorizontal_Setting();
        ControllerVibrationEnable_Setting();
        ControllerVibration_Setting();
    }

    float AddInputValue()
    {
        float Axis = Input.GetAxis(InputManager.X_Selecter);
        if (Axis >= 0.5f)
        {
            if (ChangeValue < 0) ChangeValue = 0f;
            ChangeValue += ChangeValueAccel;
        }
        else if (Axis <= -0.5f)
        {
            if (ChangeValue > 0) ChangeValue = 0f;
            ChangeValue -= ChangeValueAccel;
        }
        else
        {
            ChangeValue = 0f;
        }

        ChangeValue = Mathf.Clamp(ChangeValue, -ChangeValueMax, ChangeValueMax);
        return ChangeValue;
    }

    //BGM
    void BGM_Setting()
    {
        ConfigUI(true,true);

        BGM_Volume += AddInputValue();

        BGM_Clamp();
        BGM_SetSlider();
        BGM_SetText();

        DataManager.Instance.commonData.BGM_Volume = BGM_Volume;
    }

    void BGM_Clamp()
    {
        if (BGM_Volume > BGM_Slider.maxValue)
            BGM_Volume = BGM_Slider.maxValue;
        else if (BGM_Volume < BGM_Slider.minValue)
            BGM_Volume = BGM_Slider.minValue;
    }

    public void BGM_SetSlider()
    {
        BGM_Slider.value = BGM_Volume;
    }

    void BGM_SetText()
    {
        BGM_ParameterText.text =  (int)BGM_Volume + "%";
    }

    //--- SE --------------------------------------------------------
    void SE_Setting()
    {
        ConfigUI(true,true);

        float old = Mathf.Floor(SE_Volume);
        SE_Volume += AddInputValue();

        SE_Clamp();
        SE_SetSlider();
        SE_SetText();
        float Axis = Input.GetAxis(InputManager.X_Selecter);
        if (old != Mathf.Floor(SE_Volume))
            IsChanged = true; 

        if(IsChanged && Axis == 0f)
        {
            base.PlayAudio_Select();
            IsChanged = false;
        }


        DataManager.Instance.commonData.SE_Volume = SE_Volume;
    }

    void SE_Clamp()
    {
        if (SE_Volume > SE_Slider.maxValue)
            SE_Volume = SE_Slider.maxValue;
        else if (SE_Volume < SE_Slider.minValue)
            SE_Volume = SE_Slider.minValue;
    }

    public void SE_SetSlider()
    {
        SE_Slider.value = SE_Volume;
    }

    void SE_SetText()
    {
        SE_ParameterText.text = (int)SE_Volume + "%";
    }

    //--- CameraReverseVertical -------------------------------------
    void CameraReverseVertical_Setting()
    {
        ConfigUI(true,false);

        if (Input.GetButtonDown(InputManager.Submit))
        {
            base.PlayAudio_Success();
            IsCR_V = !IsCR_V;
        }
        CameraReverseVertical_SetText();

        DataManager.Instance.commonData.IsCameraReverseVertical = IsCR_V;
    }

    void CameraReverseVertical_SetText()
    {
        CR_V_ParameterText.text = IsCR_V ? "オン" : "オフ";
    }

    //--- CameraReverseHorizontal -----------------------------------
    void CameraReverseHorizontal_Setting()
    {
        if (Input.GetButtonDown(InputManager.Submit))
        {
            base.PlayAudio_Success();
            IsCR_H = !IsCR_H;
        }
        CameraReverseHorizontal_SetText();

        DataManager.Instance.commonData.IsCameraReverseHorizontal = IsCR_H;
    }

    void CameraReverseHorizontal_SetText()
    {
        CR_H_ParameterText.text = IsCR_H ? "オン" : "オフ";
    }

    //--- ControllerVibrationEnable ---------------------------------
    void ControllerVibrationEnable_Setting()
    {
        ConfigUI(true,false);

        if (Input.GetButtonDown(InputManager.Submit))
        {
            base.PlayAudio_Success();
            IsVibration = !IsVibration;
        }
        ControllerVibrationEnable_SetText();

        DataManager.Instance.commonData.IsVibration = IsVibration;

        if (!IsVibration)
        {
            DataManager.Instance.commonData.fVibration = 0f;
            GamePad.SetVibration(PlayerIndex.One, 0, 0);
            return;
        }
    }

    void ControllerVibrationEnable_SetText()
    {
        CVibrationEnable_ParameterText.text = IsVibration ? "オン" : "オフ";
    }

    //--- ControllerVibration ---------------------------------------
    void ControllerVibration_Setting()
    {
        if (!IsVibration) { return; }

        ConfigUI(true,true);
       
        float value = AddInputValue();
        fVibration += value;

        ControllerVibration_Clamp();
        ControllerVibration_SetSlider();
        ControllerVibration_SetText();

        DataManager.Instance.commonData.fVibration = fVibration;

        if (Mathf.Abs(value) > 0)
        {
            float vib = RotationManager.XBoxVibration;
            float vibPer = fVibration / 100;
            GamePad.SetVibration(PlayerIndex.One, vib * vibPer, vib * vibPer);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0, 0);
        }
    }

    void ControllerVibration_Clamp()
    {
        if (fVibration > CVibration_Slider.maxValue)
            fVibration = CVibration_Slider.maxValue;
        else if (fVibration < CVibration_Slider.minValue)
            fVibration = CVibration_Slider.minValue;
    }

    public void ControllerVibration_SetSlider()
    {
        CVibration_Slider.value = fVibration;
    }

    void ControllerVibration_SetText()
    {
        CVibrationValue_ParameterText.text = (int)fVibration + "%";
    }

    //--- SuportUI --------------------------------------------------
    void SuportUI_Setting()
    {
        if (Input.GetButtonDown(InputManager.Submit))
        {
            base.PlayAudio_Success();
            IsSuport = !IsSuport;
        }

        SuportUI_SetText();

        DataManager.Instance.commonData.IsSuport = IsSuport;
    }

    void SuportUI_SetText()
    {
        SuportUI_ParameterText.text = IsSuport ? "オン" : "オフ";
    }

    //--- Default ---------------------------------------------------
    void Default_Setting()
    {
        //データ読み込み
        DataManager.Instance.CommonData_ReSet();
        LoadState_CommonData();

        BGM_SetSlider();
        SE_SetSlider();
        ControllerVibration_SetSlider();

        BGM_SetText();
        SE_SetText();
        CameraReverseVertical_SetText();
        CameraReverseHorizontal_SetText();
        ControllerVibrationEnable_SetText();
        ControllerVibration_SetText();
        SuportUI_SetText();
    }

    //--- DataManager -----------------------------------------------

    void LoadState_CommonData()
    {
        DataType.CommonData data = DataManager.Instance.commonData;
        BGM_Volume = data.BGM_Volume;
        SE_Volume = data.SE_Volume;
        IsCR_V = data.IsCameraReverseVertical;
        IsCR_H = data.IsCameraReverseHorizontal;
        IsVibration = data.IsVibration;
        fVibration = data.fVibration;
        IsSuport = data.IsSuport;
    }
}
