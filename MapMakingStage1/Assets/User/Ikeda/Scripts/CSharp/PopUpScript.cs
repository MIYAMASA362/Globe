using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScript : MonoBehaviour
{
    //--- Attribute -----------------------------------------------------------

    [SerializeField]
    private GameObject PopUpTarget;
    [SerializeField]
    private float fTime = 0.25f;
    [SerializeField]
    private float PopUp_Distance = 800f;
    [SerializeField]
    private bool IsRight = false;

    private bool IsEnable = false;
    private bool IsPopUp = false;

    private float Count = 0f;
    private Vector3 HidePos;

    //--- MonoBehaviour -------------------------------------------------------

	// Use this for initialization
	void Start ()
    {
        PopUpTarget.SetActive(true);
        int mag = IsRight ? 1 : -1;
        HidePos = PopUpTarget.transform.position + (PopUpTarget.transform.right * PopUp_Distance * mag);
        PopUpTarget.transform.position = HidePos;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsEnable) return;

        float time = Count / fTime;
        int mag = IsRight ? -1 : 1;
        Vector3 target = IsPopUp ? HidePos + (PopUpTarget.transform.right * PopUp_Distance * mag) : HidePos;

        PopUpTarget.transform.position = Vector3.Lerp(PopUpTarget.transform.position, target, time);
        if (time > 1.0) { IsEnable = false; return; }
        Count += Time.deltaTime;
    }

    [ContextMenu("PopUp")]
    public void PopUp()
    {
        Count = 0f;
        IsEnable = true;
        IsPopUp = true;
    }

    [ContextMenu("PopDown")]
    public void PopDown()
    {
        Count = 0f;
        IsEnable = true;
        IsPopUp = false;
    }
}
