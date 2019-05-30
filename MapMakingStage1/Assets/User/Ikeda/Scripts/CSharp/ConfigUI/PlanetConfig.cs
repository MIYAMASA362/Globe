using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetConfig : MonoBehaviour
{
    //--- Attribute -------------------------------------------------
    [SerializeField]
    Canvas target =null;

    bool IsEnable = true;

    //--- MonoBehaviour ---------------------------------------------

    private void Start()
    {
        if (target == null)
            target = this.GetComponent<Canvas>();
    }

    private void Update()
    {
        if (!IsEnable) return;
        
        if (PlanetScene.Instance.state == PlanetScene.STATE.MAINGAME && !MySceneManager.IsOption && !MySceneManager.IsPausing)
            target.enabled = true;
        else
            target.enabled = false;
    }

    //--- Method ----------------------------------------------------

    public void SetEnable(bool enable)
    {
        target.enabled = enable;
        IsEnable = enable;
    }
}
