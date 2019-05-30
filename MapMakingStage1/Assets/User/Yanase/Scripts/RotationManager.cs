using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class RotationManager : Singleton<RotationManager> {

    public bool isStageCreate = false;

    [SerializeField] private Transform corePlanet = null;
    [SerializeField] private Transform rotationTarget = null;

    public static float XBoxVibration = 0.5f;
    private float initVibration = 0.0f;

    [Header("SE")]
    private AudioSource planetAudio;

    [Space(4)]
    [SerializeField] private float rotationSpeed = 0.0f;
    [SerializeField] private float accelSpeed = 1.0f;
    [SerializeField] private float maxSpeed = 8.0f;

    public bool isRotation = false;

    [SerializeField] private float rotationAngle = 30.0f;
    [SerializeField] private float curRotation = 0.0f;


    public Transform planetTransform
    {
        get { return corePlanet; }
    }

    public Transform rotationTransform
    {
        get { return rotationTarget; }
    }

    //Initialize
    private void Start ()
    {
        initVibration = XBoxVibration;
        planetAudio = corePlanet.GetComponent<AudioSource>();
        if (!planetAudio) planetAudio = corePlanet.gameObject.AddComponent<AudioSource>();
    }

    //Update
    private void Update()
    {
        PlanetRotation(rotationAngle);

        DataManager data = DataManager.Instance;
        XBoxVibration = initVibration * (data.commonData.fVibration / 100);
    }

    //FixedUpdate
    private void LateUpdate()
    {
        if (planetAudio)
        {
            if (MySceneManager.IsPausing || MySceneManager.IsOption ||
            PlanetScene.Instance.state != PlanetScene.STATE.MAINGAME)
            {
                planetAudio.Stop();
                corePlanet.GetComponent<Animator>().SetBool("vibration", false);
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                return;
            }

            if (isRotation)
            {
                if (!planetAudio.isPlaying)
                {
                    AudioManager audioManager = AudioManager.Instance;
                    audioManager.PlaySE(planetAudio, audioManager.SE_PLANETROTATION);
                }
            }
            else
            {
                planetAudio.Stop();
            }
        }
    }

    private void PlanetRotation(float angle)
    {
        if (MySceneManager.IsPausing || MySceneManager.IsOption || 
            PlanetScene.Instance.state != PlanetScene.STATE.MAINGAME) return;

        // フラグマネージャー取得
        FlagManager flagManager = FlagManager.Instance;

        if (flagManager.flagActive)
        {
            if (isRotation)
            {
                rotationSpeed += accelSpeed;
                rotationSpeed = Mathf.Clamp(rotationSpeed, 0f, maxSpeed);

                float roll = ((curRotation >= 0.0000f) ? rotationSpeed : -rotationSpeed) * Time.deltaTime;
                float deff = (curRotation >= 0.0000f) ? (curRotation - roll) : -(curRotation - roll);

                if (deff <= 0f)
                {
                    roll = curRotation;
                    isRotation = false;
                    curRotation = 0.0f;
                    InputRotation();
                    
                }
                else curRotation -= roll;

                Quaternion quaternion;
                Transform axisTransform = flagManager.flagTransform;

                quaternion = Quaternion.AngleAxis(roll, axisTransform.up);
                // 回転値を合成
                axisTransform.rotation = quaternion * axisTransform.rotation;

                // キャラクターカメラも回転
                if (CameraManager.Instance.characterCamera.gameObject.activeInHierarchy)
                {
                    Transform camera = CameraManager.Instance.characterCamera.transform;
                    camera.rotation = quaternion * camera.rotation;
                }

                if (isStageCreate)
                {
                    rotationTarget.rotation = quaternion * rotationTarget.rotation;
                }
                else
                {
                    corePlanet.rotation = quaternion * corePlanet.rotation;
                }

                corePlanet.GetComponent<Animator>().SetBool("vibration", true);
                GamePad.SetVibration(PlayerIndex.One, XBoxVibration, XBoxVibration);
            }
            else
            {
                InputRotation();
                rotationSpeed = 0.0f;

                corePlanet.GetComponent<Animator>().SetBool("vibration", false);
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
            }
        }
    }

    private void InputRotation()
    {
        if (MySceneManager.IsPausing || MySceneManager.IsOption ||
            PlanetScene.Instance.state != PlanetScene.STATE.MAINGAME) return;

        bool left = Input.GetButton(InputManager.Left_AxisRotation);
        bool right = Input.GetButton(InputManager.Right_AxisRotation);

        if (left)
        {
            curRotation = -rotationAngle;
            isRotation = true;
        }
        else if (right)
        {
            curRotation = rotationAngle;
            isRotation = true;
        }
    }

    public float GetSpeed()
    {
        return rotationSpeed;
    }
}
