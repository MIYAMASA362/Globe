using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObjectController : MonoBehaviour {

    //起動フラグ
    public bool button;

	// initialization
	void Start ()
    {
        button = false;
	}

    //フラグ起動関数
    public void OnButton()
    {
        button = true;
    }
}
