﻿using System.Collections;
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

    void Start()
    {
        crystal.handle = this.GetComponent<CrystalHandle>();

        UICrystal.GetComponent<Renderer>().material = Disable_material;
    }

    //クリスタルと当たり判定をした
    public bool HitCrystal(GameObject hit)
    {
        if (!CrystalJudgment(hit)) return false;
        UICrystal.GetComponent<Renderer>().material = Enable_material;

        return true;
    }

    bool CrystalJudgment(GameObject HitObject)
    {
        return HitObject == crystal.gameObject;
    }
}