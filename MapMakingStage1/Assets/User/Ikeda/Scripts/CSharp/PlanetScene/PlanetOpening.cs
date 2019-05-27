using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlanetOpening : MonoBehaviour 
{
    [SerializeField]
    public PopUpScript popUpScript;

    //--- Attribute -----------------------------------------------------------
    [Header("StageName State")]
    [SerializeField]
    private GameObject StageLabel;
    [SerializeField, Tooltip("エリア名")]
    private TextMeshProUGUI tm_GalaxyName;
    [SerializeField,Tooltip("ステージ名")]
    private TextMeshProUGUI tm_StageName;

    //--- MonoBehaviour -------------------------------------------------------

    // Use this for initialization
    void Start ()
    {
        tm_GalaxyName.text = MySceneManager.Get_GalaxyName();
        tm_StageName.text = MySceneManager.Get_PlanetName();
    }

    //--- Method --------------------------------------------------------------

    public void Begin()
    {
        this.GetComponent<PlanetScene>().SetState(PlanetScene.STATE.OPENING);

        popUpScript.PopUp();
        Invoke("End", 4f);
    }

    public void End()
    {
        this.GetComponent<PlanetScene>().SetState(PlanetScene.STATE.MAINGAME);
        popUpScript.PopDown();
    }

}
