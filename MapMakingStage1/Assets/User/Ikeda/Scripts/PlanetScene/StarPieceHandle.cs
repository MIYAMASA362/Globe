using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarPieceHandle : MonoBehaviour
{
    //完成した
    [SerializeField] private bool IsComplete;

    [Space(8), Header("PieceObject")]
    [SerializeField] private  StarPiece[] Pieces = new StarPiece[5];

    [SerializeField] private Animator UIStarPieceAnimator;
    [SerializeField] private ParticleSystem CompletedParticle;
    [SerializeField] private GameObject[] UIPieces = new GameObject[5];
    
    [Space(4)]
    [SerializeField] private Material Enable_material;
    [SerializeField] private Material Disable_material;

    [Space(8)]
    [SerializeField] private int nGetPiece;

    AudioSource audioSource;

    float animTime = 0.0f;

    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //StarPieceにHandle設定
        foreach (StarPiece starPiece in Pieces)
        {
            if (starPiece == null) continue;
            starPiece.SetHandler(this);
        }

        //UIを設定
        foreach(GameObject starPiece in UIPieces)
        {
            starPiece.GetComponent<Renderer>().material = Disable_material;
        }

        nGetPiece = 0;
        IsComplete = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (UIStarPieceAnimator.GetBool("get"))
        {
            animTime += Time.deltaTime;
            if (animTime > 1.0f)
            {
                UIStarPieceAnimator.SetBool("get", false);
            }
        }
    }

    //登録されたオブジェクト群との確認とUI変更
    public bool HitStarPiece(StarPiece HitObject)
    {
        if (!PiecesJudgment(HitObject.gameObject)) return false;

        HitObject.SetPieceNum(nGetPiece);
        nGetPiece++;

        AudioManager manager = AudioManager.Instance;
        manager.PlaySEOneShot(audioSource, manager.SE_GETSTAR);
            
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

    public bool IsCompleted()
    {
        return IsComplete;
    }

    public void Disable_UI()
    {
        UIStarPieceAnimator.gameObject.SetActive(false);
    }

    //UIのオブジェクト返し
    public Transform GetUIStarPiece(int nPieceNum)
    {
        return UIPieces[nPieceNum].transform;
    }

    //入手したStarPieceがたどり着いた
    public void UIStarPieceEnter(int nPieceNum)
    {
        if (nPieceNum + 1 >= UIPieces.Length)
        {
            IsComplete = true;
            CompletedParticle.Play();
            AudioManager manager = AudioManager.Instance;
            manager.PlaySEOneShot(audioSource, manager.SE_COMPLETESTAR);
        }

        UIPieces[nPieceNum].GetComponent<Renderer>().material = Enable_material;
        UIStarPieceAnimator.SetBool("get", true);
        animTime = 0;
    }
}


