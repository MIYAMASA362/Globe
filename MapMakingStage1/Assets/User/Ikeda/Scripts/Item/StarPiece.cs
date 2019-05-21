using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPiece : CrystalBase
{
    //--- Attribute -----------------------------------------------------------

    private StarPieceHandle handle;
    private bool IsEnable = true;   //取得されているか
    private bool IsProduce = false; //演出フラグ

    private GameObject ProduceTargetObj;    //StarPieceUI
    private float time;                     //演出経過時間
    private int nPieceNum = 0;              //ピース番号

    private Vector3 defaultScale;

    //--- MonoBehavior --------------------------------------------------------

    public override void Update()
    {
        if (!IsProduce) { base.Update();  return; }
        MoveProduce();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsEnable) return;
        if (!handle) { Debug.Log("Not StarPieceHandle!!"); return; }
        if (!other.CompareTag("Player")) return;

        handle.HitStarPiece(this);
        this.transform.SetParent(Camera.main.transform);

        defaultScale = transform.localScale;
        ProduceTargetObj = handle.GetUIStarPiece(nPieceNum);
        IsProduce = true;
        IsEnable = false;
    }

    //--- Method --------------------------------------------------------------

    private void MoveProduce()
    {
        time += Time.deltaTime;
        this.gameObject.transform.position = Vector3.Lerp(transform.position, ProduceTargetObj.transform.position, time);
        this.gameObject.transform.localScale = Vector3.Lerp(transform.localScale, defaultScale * 0.1f, time);
        this.gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, ProduceTargetObj.transform.rotation, time);

        if (time >= 0.5f) EndProduce();
    }

    private void EndProduce()
    {
        this.gameObject.SetActive(false);
        handle.UIStarPieceEnter(nPieceNum);
    }

    public void SetHandler(StarPieceHandle handle)
    {
        this.handle = handle;
    }

    public void Set_PieceNum(int nGetPiece)
    {
        nPieceNum = nGetPiece;
    }
}
