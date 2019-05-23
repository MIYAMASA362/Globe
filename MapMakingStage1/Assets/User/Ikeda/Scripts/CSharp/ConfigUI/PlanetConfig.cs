using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetConfig : MonoBehaviour
{
    [SerializeField] private RectTransform ConfigUI;
    [SerializeField, Range(0.0f, 1.0f)]
    private float ScaleRatio = 0.2f;

    [Space(8)]
    [SerializeField,Range(0.0f,1.0f)]
    private float Disable_Alpha =0f;
    [SerializeField]
    private GameObject ConfigUI_Axis;
    [SerializeField]
    private GameObject ConfigUI_A;
    [SerializeField]
    private GameObject ConfigUI_B;

    private Vector3 DefaultScale;
    private bool IsPlayerScale = false;

	// Use this for initialization
	void Start ()
    {
        DefaultScale = ConfigUI.localScale;
        IsPlayerScale = false;
        Set_PlayerViewScale();
	}

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            Set_PlayerViewScale();
        else if (Input.GetKeyUp(KeyCode.O))
            Set_PlanetViewScale();
    }

    //--- Scaler ------------------------------------------

    [ContextMenu("Set_PlanetViewScale")]
    public void Set_PlanetViewScale()
    {
        if (!IsPlayerScale) return;
        ConfigUI.transform.localScale = DefaultScale;
        IsPlayerScale = false;
    }

    [ContextMenu("Set_PlayerViewScale")]
    public void Set_PlayerViewScale()
    {
        if (IsPlayerScale) return;
        ConfigUI.transform.localScale = DefaultScale * (1-ScaleRatio);
        IsPlayerScale = true;
    }
}
