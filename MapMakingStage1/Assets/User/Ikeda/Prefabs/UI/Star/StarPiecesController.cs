using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPiecesController : MonoBehaviour
{


    public int starPiecesCount = 5;
    public GameObject[] starPieces;

    public Material starMat;
    public Material starEmptyMat;

    void Update()
    {

        starPiecesCount = Mathf.Clamp(starPiecesCount, 0, 5);
        for (int m = 0;m < 6; m++)
        {
    
            if(m > starPiecesCount -1)
            {
                starPieces[m].GetComponent<MeshRenderer>().material = starMat;
            }
            else
            {
                starPieces[m].GetComponent<MeshRenderer>().material = starEmptyMat;
            }

        }
        
    }


}
