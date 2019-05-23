using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : CrystalBase
{
    //--- Attribute -----------------------------------------------------------

    private CrystalHandle handle = null;

    private Transform followTarget;    //StarPieceUI

    [Header("Get")]
    public float getHeight = 1.0f;
    public float getTime = 1.0f;

    [Header("Follow")]
    public float followSpeed = 1.0f;
    private int nPieceNum = 0;          //ピース番号

    //--- MonoBehaviour -------------------------------------------------------

	// Use this for initialization
	void Start ()
    {
        
    }

    public override void Update()
    {
        base.Update();

        switch (state)
        {
            case State.Idle:
                break;
            case State.Get:

                UpdeteGet();
                break;
            case State.Follow:

                UpdateFollow();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != State.Idle) return;
        if (!handle) { Debug.Log("Not CrystalHandle!!"); return;}
        if (!other.gameObject.CompareTag("Player")) return;

        SetGet(other.transform);
    }

    //--- Method --------------------------------------------------------------

    public void SetHandler(CrystalHandle handle)
    {
        this.handle = handle;
    }

    private void SetGet(Transform target)
    {
        handle.HitCrystal(this.gameObject);
        this.transform.SetParent(target);
        transform.up = -target.up;
        transform.localPosition = new Vector3(0.0f, getHeight, 0.0f);
        transform.localScale *= 0.6f;
        state = State.Get;
    }

    private void SetFollow()
    {
        transform.SetParent(Camera.main.transform);
        followTarget = handle.GetCrystalTarget();
        state = State.Follow;
    }

    private void UpdeteGet()
    {
        base.Update();
        getTime -= Time.deltaTime;
        if (getTime <= 0)
        {
            SetFollow();
        }
    }

    //--- Method --------------------------------------------------------------

    //UI位置までの移動演出
    private void UpdateFollow()
    {
        float delta = Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, followTarget.position, delta * followSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, followTarget.lossyScale, delta * followSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, followTarget.rotation, delta * followSpeed);

        float dist = (transform.position - followTarget.position).magnitude;
        if (dist < 0.05f) EndProduce();
    }

    //演出終了
    public void EndProduce()
    {
        this.gameObject.SetActive(false);
        handle.UICrystalEnter();
    }

}
