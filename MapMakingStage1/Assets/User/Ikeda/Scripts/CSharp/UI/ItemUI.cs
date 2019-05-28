using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private GameObject Target = null;

    [SerializeField]
    private Vector3 PopUp_Pos;
    [SerializeField]
    private Vector3 Hide_Pos;
    [SerializeField]
    private float fTime = 0.25f;

    private bool IsEnable = false;
    private bool IsPopUp = false;

    private float Count = 0f;

    // Use this for initialization
    void Start ()
    {
        if (Target == null)
            Target = this.gameObject;
        Target.transform.localPosition = Hide_Pos;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsEnable) return;

        float time = Count / fTime;

        if(IsPopUp)
            Target.transform.localPosition = Vector3.Lerp(Hide_Pos, PopUp_Pos, time);
        else
            Target.transform.localPosition = Vector3.Lerp(PopUp_Pos,Hide_Pos, time);
       
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
