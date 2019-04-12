using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlanetSelecter : MonoBehaviour {

    //--- Attribute ---------------------------------------

    //--- Public --------------------------------

    //--- Private -------------------------------

    //--- State -----------------------
    [SerializeField] GameObject[] Stage;
    [SerializeField] Transform CameraPivot = null;

    //--- State -----------------------
    int nLength;
    int nSelect = 0;

    Vector3 move;
    GameObject planet = null;

    //--- MonoBehaviour -----------------------------------

    // Use this for initialization
    void Start ()
    {
        nLength = Stage.Length;

        //Canvasの有効を無効に
        foreach (GameObject obj in Stage)
        {
            PlanetCanvas(obj,false);
        }

        //初期Planetを有効にする
        PlanetCanvas(Stage[nSelect],true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        nSelect = Selecter(nSelect, nLength);
        
        //位置が同じではない
        if(planet.transform.position != CameraPivot.transform.position)
        {
            CameraPivot.transform.position = Vector3.Lerp(CameraPivot.transform.position,planet.transform.position,Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {
        
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < Stage.Length-1; i++)
        {
            Gizmos.DrawLine(Stage[i].transform.position,Stage[i+1].transform.position);
        }
    }

    //--- Method ------------------------------------------

    int Selecter(int num,int length)
    {
        int old = num;

        //Right 右
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            num++;
            num = num % length;
        }

        //Left 左
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            num--;
            if (num <= -1) num = length - 1;
            num = num % length;
        }

        planet = Stage[num];

        //Is Changed value : 切り替えられている
        if(old != num)
        {
            PlanetCanvas(Stage[old],false);
            PlanetCanvas(planet, true);
        }

        return num;
    }

    //PlanetのCanvasを変更させる
    void PlanetCanvas(GameObject obj, bool enable)
    {
        obj.transform.Find("Canvas").gameObject.SetActive(enable);
    }

}
