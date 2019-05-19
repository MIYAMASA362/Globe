using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class FlagManager : Singleton<FlagManager> {

    [Header("軸回転させる旗"), SerializeField]
    private GameObject flag = null;

    [Header("SE")]
    public float SE_FlagPlugInVolume = 1f;
    public float SE_FlagDeployVolume = 3f;
    public float SE_FlagUnPlugVolume = 1f;
    public float SE_FloatSwapVolume = 1f;
    private AudioSource flagAudio;

    [Header("軸を刺すときのエフェクト")]
    public LineEffectSwitcher lineEffectSwitcher = null;
    private Vector3 linePosition = Vector3.zero;

    [Header("浮遊グラウンド設定")]
    [Tag] public string findTag = "FloatGround";
    public FloatType.Type curFloatType;
    private FloatGround[] floatObjects;

    public bool flagActive = false;

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

    private Vector3 InitScale = Vector3.zero;
    public float flagScaleSpeed = 3.0f;

    void Start () {

        FindFloatGround();

        InitScale = flag.transform.localScale;
        flag.transform.localScale = Vector3.zero;

        flagAudio = flag.GetComponent<AudioSource>();
        if (!flagAudio) flagAudio = flag.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (flagActive)
        {
            flag.transform.localScale = Vector3.Lerp(flag.transform.localScale, InitScale, Time.deltaTime * flagScaleSpeed);

            if (Input.GetButtonDown(InputManager.Change_AscDes))
            {
                if (CheckFloatOnGround())
                {
                    if (flagAudio) AudioManager.Instance.PlaySEOneShot(flagAudio, AUDIO.SE_FLOATSWAP, SE_FloatSwapVolume);
                    ChangeFloatOnGround();
                }
            }
        }
        else
        {
            flag.transform.localScale = Vector3.Lerp(flag.transform.localScale,Vector3.zero, Time.deltaTime * flagScaleSpeed);
        }
    }

    public void SetFlag(Vector3 axisPos, FloatType.Type type)
    {
        if (!flag)
        {
            Debug.Log("flag is nothing!!");
            return;
        }

        if (flagActive)
        {
            Debug.Log("flag is active!!");
            return;
        }
        linePosition = axisPos;

        Transform planetTransform = RotationManager.Instance.planetTransform;

        flag.transform.position = planetTransform.transform.position;
        flag.transform.up = axisPos - planetTransform.transform.position;
        lineEffectSwitcher.SetEffect(linePosition, Color.green);
        curFloatType = type;
        flagActive = true;

        AudioManager audioManager = AudioManager.Instance;
        if (flagAudio)
        {
            audioManager.PlaySEOneShot(flagAudio, AUDIO.SE_FLAGPLUGIN, SE_FlagPlugInVolume);
            audioManager.PlaySEOneShot(flagAudio, AUDIO.SE_DEPLOY, SE_FlagDeployVolume);
        }
    }

    public bool DestoyFlag(Vector3 axisPos)
    {
        if (!flag)
        {
            Debug.Log("flag is nothing!!");
            return false;
        }

        if (!CheckFloatOnGround()) return false;

        lineEffectSwitcher.SetEffect(axisPos, Color.red);
        flagActive = false;

        if (flagAudio) AudioManager.Instance.PlaySEOneShot(flagAudio, AUDIO.SE_FLAGUNPLUG, SE_FlagUnPlugVolume);

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
        if (RotationManager.Instance.isRotation) return false;

        bool isCheck = true;

        foreach (var floatObj in floatObjects)
        {
            if (curFloatType == floatObj.type)
            {
                if (floatObj.onGround)
                {
                    isCheck = false;
                    break;
                }
            }
        }

        if (!isCheck)
        {
            foreach (var floatObj in floatObjects)
            {
                if (curFloatType == floatObj.type)
                {
                    floatObj.isFalse = true;
                }
            }
        }

        return isCheck;
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
