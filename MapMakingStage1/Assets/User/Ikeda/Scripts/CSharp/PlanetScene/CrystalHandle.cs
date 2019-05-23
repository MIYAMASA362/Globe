using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalHandle : MonoBehaviour
{
    [SerializeField] private Crystal crystal;
    [SerializeField] private GameObject UICrystal;

    [Space(8)]
    [SerializeField] private Material Enable_material;
    [SerializeField] private Material Disable_material;

    [SerializeField,Tooltip("取得しているか")] private bool IsGet = false;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        crystal.SetHandler(this);

        UICrystal.GetComponent<Renderer>().material = Disable_material;
    }

    //クリスタルと当たり判定をした
    public bool HitCrystal(GameObject hit)
    {
        if (!CrystalJudgment(hit)) return false;

        AudioManager manager = AudioManager.Instance;
        manager.PlaySEOneShot(audioSource, manager.SE_GETDIAMOND1);

        IsGet = true;
        return true;
    }

    bool CrystalJudgment(GameObject HitObject)
    {
        return HitObject == crystal.gameObject;
    }

    public bool IsGetting()
    {
        return IsGet;
    }

    public void Enable_UI()
    {
        UICrystal.SetActive(true);
    }

    public void Disable_UI()
    {
        UICrystal.SetActive(false);
    }

    public void UICrystalEnter()
    {
        UICrystal.GetComponent<Renderer>().material = Enable_material;
        AudioManager manager = AudioManager.Instance;
        manager.PlaySEOneShot(audioSource, manager.SE_GETDIAMOND2);
    }

    public Transform GetCrystalTarget()
    {
        return this.UICrystal.transform;
    }
}
