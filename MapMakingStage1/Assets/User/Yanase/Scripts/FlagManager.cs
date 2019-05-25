using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class FlagManager : Singleton<FlagManager> {

    [Header("軸回転させる旗"), SerializeField]
    private GameObject flag = null;

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

    float vibrationTime = 0.0f;

    void Start () {

        FindFloatGround();

        InitScale = flag.transform.localScale;
        flag.transform.localScale = Vector3.zero;

        flagAudio = flag.GetComponent<AudioSource>();
        if (!flagAudio) flagAudio = flag.AddComponent<AudioSource>();
    }

    void Update()
    {
        if(vibrationTime > 0)
        {
            vibrationTime -= Time.deltaTime;
            RotationManager flagManager = RotationManager.Instance;
            if (vibrationTime > 0)
                GamePad.SetVibration(PlayerIndex.One, flagManager.XBoxVibration, flagManager.XBoxVibration);
            else
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        }

        if (flagActive)
        {
            flag.transform.localScale = Vector3.Lerp(flag.transform.localScale, InitScale, Time.deltaTime * flagScaleSpeed);

            if (Input.GetButtonDown(InputManager.Change_AscDes))
            {
                if (CheckFloatOnGround())
                {
                    if (flagAudio)
                    {
                        AudioManager audioManager = AudioManager.Instance;
                        audioManager.PlaySEOneShot(flagAudio, audioManager.SE_FLOATSWAP);
                    }
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

        vibrationTime = 0.3f;

        AudioManager audioManager = AudioManager.Instance;
        if (flagAudio)
        {
            audioManager.PlaySEOneShot(flagAudio, audioManager.SE_FLAGPLUGIN);
            audioManager.PlaySEOneShot(flagAudio, audioManager.SE_DEPLOY);
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

        vibrationTime = 0.3f;

        if (flagAudio)
        {
            AudioManager audioManager = AudioManager.Instance;
            audioManager.PlaySEOneShot(flagAudio, audioManager.SE_FLAGUNPLUG);
        }

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

            if (flagAudio)
            {
                AudioManager audioManager = AudioManager.Instance;
                audioManager.PlaySEOneShot(flagAudio, audioManager. SE_FLOATSWAP);
            }
        }

        return isCheck;
    }

    void ChangeFloatOnGround()
    {
        foreach (var floatObj in floatObjects)
        {
            if (curFloatType == floatObj.type)
            {
                floatObj.isFloat = !floatObj.isFloat;
                floatObj.a = true;
            }
        }

        if (flagAudio)
        {
            AudioManager manager = AudioManager.Instance;
            manager.PlaySEOneShot(flagAudio, manager.SE_FLOATGROUNDENTER);
        }
    }
}
