using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : CrystalBase
{
    //--- Attribute -----------------------------------------------------------

    private CrystalHandle handle = null;
    private bool IsEnable = true;
    private bool IsProduce = false;

    private GameObject ProduceTargetObj;
    private float time;

    private Vector3 defaultScale;

    //--- MonoBehaviour -------------------------------------------------------

	// Use this for initialization
	void Start ()
    {
        
    }

    public override void Update()
    {
        if (!IsProduce) { base.Update(); return; }
        MoveProduce();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsEnable) return;
        if (!handle) { Debug.Log("Not CrystalHandle!!"); return;}
        if (!other.gameObject.CompareTag("Player")) return;

        handle.HitCrystal(this.gameObject);
        ProduceTargetObj = handle.GetCrystalTarget();
        defaultScale = transform.localScale;
        IsEnable = false;
        IsProduce = true;
    }

    //--- Method --------------------------------------------------------------

    public void SetHandler(CrystalHandle handle)
    {
        this.handle = handle;
    }

    public void MoveProduce()
    {
        time += Time.deltaTime/2f;
        this.gameObject.transform.position = Vector3.Lerp(transform.position, ProduceTargetObj.transform.position, time);
        this.gameObject.transform.localScale = Vector3.Lerp(transform.localScale,ProduceTargetObj.transform.lossyScale, time);
        this.gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, ProduceTargetObj.transform.rotation, time);

        if (time >= 0.3f) EndProduce();
    }

    public void EndProduce()
    {
        this.gameObject.SetActive(false);
        handle.UICrystalEnter();
    }

}
