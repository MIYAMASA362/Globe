using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlanetOpening : MonoBehaviour 
{
    [SerializeField]
    public PopUpScript popUpScript;
    [SerializeField]
    private ItemUI ItemPopUp;

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
        PlanetScene.Instance.SetOpening();
        popUpScript.PopUp();
    }

    public void End()
    {
        PlanetScene.Instance.EndOpening();
        popUpScript.PopDown();
        ItemPopUp.PopUp();
    }

}
