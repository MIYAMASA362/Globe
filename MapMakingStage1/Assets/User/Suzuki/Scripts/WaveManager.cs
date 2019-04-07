using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public Material OceanMat;
    private Vector4 Speed;
    // フラグマネージャー取得
//    FlagManager flagManager = FlagManager.Instance;

    // Use this for initialization
    void Start () {
        Speed.z = -3.5f;
	}
	
	// Update is called once per frame
	void Update () {
        RotationManager rotationManager = RotationManager.Instance;
        FlagManager flagManager = FlagManager.Instance;
        Debug.Log(rotationManager.GetSpeed());

        if (flagManager.flagActive && rotationManager.GetSpeed() != 0)
       {
            if (rotationManager.GetSpeed() > 0)
            {
                Speed.z = -3.5f;
            }
            else if (rotationManager.GetSpeed() < 0)
            {
                Speed.z = 3.5f;
            }
        }
        else
        {
            Speed = new Vector4(0, 0, 0, 0);
        }
        OceanMat.SetVector("_FlowSpeed", Speed);
    }
}
