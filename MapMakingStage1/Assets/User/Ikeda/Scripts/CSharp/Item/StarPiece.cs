using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPiece : CrystalBase
{
    //--- Attribute -----------------------------------------------------------

    private StarPieceHandle handle;
    private Transform followTarget;    //StarPieceUI

    [Header("Get")]
    public float getHeight = 1.0f;
    public float getTime = 1.0f;

    [Header("Follow")]
    public float followSpeed = 1.0f;
    private int nPieceNum = 0;          //ピース番号

    

    //--- MonoBehavior --------------------------------------------------------
    private void Start()
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
        if (!handle) { Debug.Log("Not StarPieceHandle!!"); return; }
        if (!other.CompareTag("Player")) return;

        SetGet(other.transform);
    }

    private void SetGet(Transform target)
    {
        handle.HitStarPiece(this);
        this.transform.SetParent(target);
        transform.localPosition = new Vector3(0.0f, getHeight, 0.0f);
        RotSpeed += 3.0f;
        state = State.Get;
    }

    private void SetFollow()
    {
        transform.SetParent(Camera.main.transform);
        followTarget = handle.GetUIStarPiece(nPieceNum);
        state = State.Follow;
    }

    private void UpdeteGet()
    {
        base.Update();
        getTime -= Time.deltaTime;
        if(getTime <= 0)
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
    private void EndProduce()
    {
        this.gameObject.SetActive(false);
        handle.UIStarPieceEnter(nPieceNum);
    }

    public void SetHandler(StarPieceHandle handle)
    {
        this.handle = handle;
    }

    public void SetPieceNum(int nGetPiece)
    {
        nPieceNum = nGetPiece;
    }
}
