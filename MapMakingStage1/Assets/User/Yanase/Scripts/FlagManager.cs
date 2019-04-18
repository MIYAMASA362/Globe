using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : Singleton<FlagManager> {

    [Header("軸回転させる旗"), SerializeField]
    private GameObject flag = null;
    [Tag] public string findTag;
    FloatGround[] floatObjects;

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
    public bool onFloat = false;

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

        GameObject[] findObject = GameObject.FindGameObjectsWithTag(findTag);

        floatObjects = new FloatGround[findObject.Length];
        for (int i = 0; i < findObject.Length; i++) 
        {
            floatObjects[i] = findObject[i].GetComponent<FloatGround>();
        }
    }
	
	void Update () {
        if(RotationManager.Instance.rotationSpeed == 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach(var floatobj in floatObjects)
                {
                    if (floatobj.onGround) return;
                }

                onFloat = !onFloat;
            } 
        }
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
