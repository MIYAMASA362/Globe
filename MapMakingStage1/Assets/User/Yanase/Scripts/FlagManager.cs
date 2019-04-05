using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : Singleton<FlagManager> {

    [Header("軸と一緒に回転するオブジェクト")]
    public Transform rotationTransform = null;
    [Header("回転するスピード")]
    public float speed;
    [Header("軸回転させる旗"), SerializeField]
    private GameObject flag = null;

    // 仮
    public Transform planet = null;
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

    void Start () {
        if (flag) flag.SetActive(false);
    }
	
	void Update () {
        if (flag)
        {
            if (flagActive)
            {
                Quaternion quaternion;
                // ターゲット軸の回転するクオータニオン作成
                if (Input.GetKey(KeyCode.Z))
                {
                    quaternion = Quaternion.AngleAxis(-speed * Time.deltaTime, flag.transform.up);
                    // 回転値を合成
                    flag.transform.rotation = quaternion * flag.transform.rotation;
                    if (rotationTransform) rotationTransform.rotation = quaternion * rotationTransform.transform.rotation;
                }
                if (Input.GetKey(KeyCode.X))
                {
                    quaternion = Quaternion.AngleAxis(speed * Time.deltaTime, flag.transform.up);
                    // 回転値を合成
                    flag.transform.rotation = quaternion * flag.transform.rotation;
                    if (rotationTransform) rotationTransform.rotation = quaternion * rotationTransform.transform.rotation;
                }
            }
            

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (flagActive)
                {
                    DestoyFlag();
                }
                else
                {
                    SetFlag(axis.position);
                }
            }
        }
	}

    void SetFlag(Vector3 axisPos)
    {
        if(!flag)
        {
            Debug.Log("Flag is nothing");
            return;
        }
        flag.transform.position = planet.transform.position;
        flag.transform.up = axisPos - planet.transform.position;
        flag.SetActive(true);
    }

    void DestoyFlag()
    {
        if (!flag)
        {
            Debug.Log("Flag is nothing");
            return;
        }
        flag.SetActive(false);
    }
}
