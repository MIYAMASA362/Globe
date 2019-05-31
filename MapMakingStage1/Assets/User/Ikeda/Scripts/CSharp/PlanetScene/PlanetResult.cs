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
    private Material getMaterial;
    [SerializeField]
    private Material notMaterial;
    [SerializeField]
    private GameObject[] StarCrystalUI;


    //[Space(8)]
    //[SerializeField, Tooltip("銀河アンロック表示")]
    //private GameObject GalaxyUnLockUI;
    //[SerializeField]
    //private TextMeshProUGUI UnLockText;

    [Space(10)]
    [SerializeField]
    private Animator ResultEndAnimator;
    [SerializeField]
    private GameObject StarsBackGround;


    private float InputWait = 0;

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
        ResultUI.SetActive(false);
    }

    private void Update()
    {
        if (!IsEnable) return;

        StarsBackGround.transform.rotation = Quaternion.AngleAxis(-15f * Time.deltaTime, ResultEndAnimator.transform.right) * StarsBackGround.transform.rotation;

        InputWait += Time.deltaTime;

        if (Input.GetButtonUp(InputManager.Submit) && IsInput && InputWait > 0.5f)
        {
            End();
        }
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
        InputWait = 0;

        ResultEndAnimator.SetTrigger("Disolve");

        ItemPopUp.PopDown();
    }

    public void Print()
    {
        if (!IsEnable) return;

        ResultUI.SetActive(true);

        //取得か非取得で変更
        if (crystalHandle.IsGetting())
            AchievCrystalUI.SetActive(true);
        else
            NotAchievCrystalUI.SetActive(true);

        
        AchievStarCrystalUI.SetActive(true);

        int num = GetComponent<StarPieceHandle>().nGetPiece;
        foreach(GameObject star in StarCrystalUI)
        {
            if (num > 0)
            {
                star.GetComponent<MeshRenderer>().material = getMaterial;
                num--;
            }
            else
            {
                star.GetComponent<MeshRenderer>().material = notMaterial;
            }
        }

    }

    public void End()
    {
        if (!IsEnable) return;

        AudioManager.Instance.PlaySEOneShot(ResultEndAnimator.GetComponent<ResultEndEvent>().audioSource, AudioManager.Instance.SE_SUCCESS);
        IsEnable = false;
        IsInput = false;
        ResultUI.SetActive(false);
        ResultEndAnimator.SetTrigger("EndResult");
    }

    public void IsInputEnable()
    {
        IsInput = true;
    }
}
