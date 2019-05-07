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

    [Space(8)]
    [SerializeField] private GameObject UI;

    [SerializeField] private GameObject[] UIIndex;
    [Space(4)]
    [SerializeField] private Color EnableColor = Color.white;
    [SerializeField] private Color DisableColor = Color.black;

    private int nGetPiece;

    // Use this for initialization
    void Start ()
    {
        //StarPieceにHandle設定
        foreach (StarPiece starPiece in Pieces)
        {
            if (starPiece == null) continue;
            starPiece.GetComponent<StarPiece>().handle = this.GetComponent<StarPieceHandle>();
        }

        //UIの変更
        UIIndex = new GameObject[UI.transform.childCount];
        for (int i = 0; i < UIIndex.Length; i++)
        {
            UIIndex[i] = UI.transform.GetChild(i).gameObject;
            UIIndex[i].GetComponent<Image>().color = DisableColor;
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
        if (nGetPiece > UIIndex.Length) return false;

        UIIndex[nGetPiece].GetComponent<Image>().color = EnableColor;
        return true;
    }

    //登録されたオブジェクト群との判定
    bool PiecesJudgment(GameObject @object)
    {
        foreach (StarPiece starPiece in Pieces)
        {
            GameObject Piece = starPiece.gameObject;
            if (Piece != @object) continue;
            nGetPiece++;
            return true;
        }
        return false;
    }

}
