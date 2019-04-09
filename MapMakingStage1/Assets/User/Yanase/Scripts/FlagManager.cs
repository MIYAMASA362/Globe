using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : Singleton<FlagManager> {

    [Header("軸回転させる旗"), SerializeField]
    private GameObject flag = null;

    // 仮
    public Transform axis = null;

    public bool flagActive
    {
        get
        {
            if (flag)
            {
                return flag.activeInHierarchy;
            }
            else
            {
                Debug.Log("Flag is nothing");
                return false;
            }
        }
    }

    public Transform flagTransform
    {
        get
        {
            if (flag)
            {
                return flag.transform;
            }
            else
            {
                Debug.Log("Flag is nothing");
                return null;
            }
        }
    }

    void Start () {
        if (flag) flag.SetActive(false);
    }
	
	void Update () {
        
	}

    public void SetFlag(Vector3 axisPos)
    {
        if(!flag)
        {
            Debug.Log("flag is nothing!!");
            return;
        }

        if(flagActive)
        {
            Debug.Log("flag is active!!");
            return;
        }

        Transform planetTransform = RotationManager.Instance.planetTransform;

        flag.transform.position = planetTransform.transform.position;
        flag.transform.up = axisPos - planetTransform.transform.position;
        flag.SetActive(true);
    }

    public void DestoyFlag()
    {
        if (!flag)
        {
            Debug.Log("flag is nothing!!");
            return;
        }
        flag.SetActive(false);
    }
}
