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

    void Start()
    {
        crystal.SetHandler(this);

        UICrystal.GetComponent<Renderer>().material = Disable_material;
    }

    //クリスタルと当たり判定をした
    public bool HitCrystal(GameObject hit)
    {
        if (!CrystalJudgment(hit)) return false;
        UICrystal.GetComponent<Renderer>().material = Enable_material;
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
}
