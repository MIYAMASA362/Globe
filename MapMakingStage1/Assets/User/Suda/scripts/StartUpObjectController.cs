using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpObjectController : MonoBehaviour {

    //起動元のオブジェクト
    [SerializeField] public SwitchObjectController SwitchObject;
	
	// Update 
	void Update () {
        //起動元のオブジェクトが起動
        if (SwitchObject.button)
        {
            Action();
        }
	}

    //起動先特有の動きをさせる関数
    public virtual void Action()
    {

    }
}
