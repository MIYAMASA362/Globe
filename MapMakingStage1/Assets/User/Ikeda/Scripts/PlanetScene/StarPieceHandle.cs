using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarPieceHandle : MonoBehaviour
{
    //完成した
    [SerializeField] public bool IsComplete;

    [Space(8), Header("PieceObject")]
    [SerializeField] private  StarPiece[] Pieces = new StarPiece[5];

    [SerializeField] private GameObject[] UIPieces = new GameObject[5];
    [Space(4)]
    [SerializeField] private Material Enable_material;
    [SerializeField] private Material Disable_material;

    [Space(8)]
    [SerializeField]private int nGetPiece;

    // Use this for initialization
    void Start ()
    {
        //StarPieceにHandle設定
        foreach (StarPiece starPiece in Pieces)
        {
            if (starPiece == null) continue;
            starPiece.handle = this.GetComponent<StarPieceHandle>();
        }

        //UIを設定
        foreach(GameObject starPiece in UIPieces)
        {
            starPiece.GetComponent<Renderer>().material = Disable_material;
        }

        nGetPiece = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //登録されたオブジェクト群との確認とUI変更
    public bool HitStarPiece(GameObject HitObject)
    {
        if (!PiecesJudgment(HitObject)) return false;
        UIPieces[nGetPiece].GetComponent<Renderer>().material = Enable_material;
        nGetPiece++;
        return true;
    }

    //登録されたオブジェクト群との判定
    bool PiecesJudgment(GameObject HitObject)
    {
        for(int i= 0; i < Pieces.Length; i++)
        {
            if (HitObject != Pieces[i].gameObject) continue;
            return true;
        }
        return false;
    }

}
