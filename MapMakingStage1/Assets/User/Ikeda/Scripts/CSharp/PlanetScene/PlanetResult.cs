using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

[RequireComponent(typeof(CrystalHandle))]
[RequireComponent(typeof(StarPieceHandle))]
public class PlanetResult : MonoBehaviour
{
    //--- Attribute -----------------------------------------------------------

    //Component
    private CrystalHandle crystalHandle;
    private StarPieceHandle starPieceHandle;

    //UI
    [Header("UI")]
    [SerializeField,Tooltip("リザルト表示に使うUI")]
    private GameObject ResultUI;
    [SerializeField, Tooltip("リザルト時クリスタルUI")]
    private GameObject AchievCrystalUI;
    [SerializeField]
    private GameObject NotAchievCrystalUI;
    [SerializeField]
    private ItemUI ItemPopUp;

    [Space(8)]
    [SerializeField, Tooltip("リザルト時スタークリスタルUI")]
    private GameObject AchievStarCrystalUI;
    [SerializeField]
    private GameObject NotAchievStarCrystalUI;

    [Space(8)]
    [SerializeField, Tooltip("銀河アンロック表示")]
    private GameObject GalaxyUnLockUI;
    [SerializeField]
    private TextMeshProUGUI UnLockText;

    [Space(10)]
    [SerializeField]
    private Animator ResultEndAnimator;
    [SerializeField]
    private GameObject ShipCamera;
    [SerializeField]
    private GameObject ResultCamera;
    [SerializeField]
    private GameObject PlayerChara;
    [SerializeField]
    private GameObject StarsBackGround;
    [SerializeField]
    private GameObject AxisDevice;

    private float Input_Wait = 0;

    private bool IsEnable = false;
    private bool IsInput = false;

    private bool IsFadeIn = false;

    //--- MonoBehavior --------------------------------------------------------

    private void Start()
    {
        crystalHandle = GetComponent<CrystalHandle>();
        starPieceHandle = GetComponent<StarPieceHandle>();

        AchievCrystalUI.SetActive(false);
        NotAchievCrystalUI.SetActive(false);
        AchievStarCrystalUI.SetActive(false);
        NotAchievStarCrystalUI.SetActive(false);

        ShipCamera.SetActive(false);
        ResultCamera.SetActive(false);
        ResultUI.SetActive(false);
    }

    private void Update()
    {
        if (!IsEnable) return;

        StarsBackGround.transform.rotation = Quaternion.AngleAxis(-15f * Time.deltaTime,Vector3.right) * StarsBackGround.transform.rotation;

        Input_Wait += Time.deltaTime;
        
        if(Input_Wait >= 3.0f)
            if(Input.GetButtonUp(InputManager.Submit))
                End();
    }

    //--- Method --------------------------------------------------------------

    //Result演出の開始
    [ContextMenu("Begin")]
    public void Begin()
    {
        if (IsEnable) return;

        //ゲームでのUI表示切り
        crystalHandle.Disable_UI();
        starPieceHandle.Disable_UI();

        IsEnable = true;
        Input_Wait = 0;

        ShipCamera.SetActive(true);
        ResultEndAnimator.SetTrigger("TakeOff");

        ItemPopUp.PopDown();
    }

    public void Print()
    {
        if (!IsEnable) return;

        //取得か非取得で変更
        if (crystalHandle.IsGetting())
            AchievCrystalUI.SetActive(true);
        else
            NotAchievCrystalUI.SetActive(true);

        if (starPieceHandle.IsCompleted())
            AchievStarCrystalUI.SetActive(true);
        else
            NotAchievStarCrystalUI.SetActive(true);

        ShipCamera.SetActive(false);
        ResultCamera.SetActive(true);
        ResultUI.SetActive(true);

    }

    public void End()
    {
        if (!IsEnable) return;

        ResultUI.SetActive(false);
        IsEnable = false;

        ResultEndAnimator.SetTrigger("EndResult");
    }

    public void UnLoadPlayer()
    {
        PlayerChara.SetActive(false);
    }

    public void HideAxisDevice()
    {
        AxisDevice.SetActive(false);
    }
}
