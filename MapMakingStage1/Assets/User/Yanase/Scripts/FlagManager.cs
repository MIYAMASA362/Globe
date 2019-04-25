using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : Singleton<FlagManager> {

    [Header("軸回転させる旗"), SerializeField]
    private GameObject flag = null;
    [Header("軸を刺すときのエフェクト")]
    public LineEffectSwitcher lineEffectSwitcher = null;
    private Vector3 linePosition = Vector3.zero;

    [Header("浮遊グラウンド設定")]
    [Tag] public string findTag = "FloatGround";
    public FloatType.Type curFloatType;
    private FloatGround[] floatObjects;

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

        FindFloatGround();
    }

    void Update()
    {
        if (Input.GetButtonDown(InputManager.Change_AscDes))
        {
            if (CheckFloatOnGround()) ChangeFloatOnGround();
        }
    }

    public void SetFlag(Vector3 axisPos, FloatType.Type type)
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
        linePosition = axisPos;

        Transform planetTransform = RotationManager.Instance.planetTransform;

        flag.transform.position = planetTransform.transform.position;
        flag.transform.up = axisPos - planetTransform.transform.position;
        lineEffectSwitcher.SetEffect(linePosition, Color.green);
        flag.SetActive(true);
        curFloatType = type;

        RotationManager.Instance.ArrowObject.transform.up = flag.transform.up;
    }

    public bool DestoyFlag()
    {
        if (!flag)
        {
            Debug.Log("flag is nothing!!");
            return false;
        }

        if (!CheckFloatOnGround()) return false;

        lineEffectSwitcher.SetEffect(linePosition, Color.red);
        flag.SetActive(false);

        return true;
    }

    void FindFloatGround()
    {
        GameObject[] findObject = GameObject.FindGameObjectsWithTag(findTag);

        floatObjects = new FloatGround[findObject.Length];
        for (int i = 0; i < findObject.Length; i++)
        {
            floatObjects[i] = findObject[i].GetComponent<FloatGround>();
        }
    }

    bool CheckFloatOnGround()
    {
        if (RotationManager.Instance.rotationSpeed != 0.0f) return false;

        foreach (var floatObj in floatObjects)
        {
            if (curFloatType == floatObj.type)
            {
                if (floatObj.onGround) return false;
            }
        }

        return true;
    }

    void ChangeFloatOnGround()
    {
        foreach (var floatObj in floatObjects)
        {
            if(curFloatType == floatObj.type)
            {
                floatObj.isFloat = !floatObj.isFloat;
            }
        }
    }
}
