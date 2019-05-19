using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class RotationManager : Singleton<RotationManager> {

    [SerializeField] private Transform corePlanet = null;
    [SerializeField] private Transform rotationTarget = null;

    public float XBoxVibration = 0.8f;

    [Header("SE")]
    public float SE_PlanetRotationVolume = 2f;
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
        planetAudio = corePlanet.GetComponent<AudioSource>();
        if (!planetAudio) planetAudio = corePlanet.gameObject.AddComponent<AudioSource>();
    }
	
    //Update
	private void Update ()
    {
        PlanetRotation(rotationAngle);
    }

    //FixedUpdate
    private void LateUpdate()
    {
        if (planetAudio)
        {
            if (isRotation)
            {
                if (!planetAudio.isPlaying)
                {
                    AudioManager.Instance.PlaySE(planetAudio, AUDIO.SE_PLANETROTATION, SE_PlanetRotationVolume);
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
        // フラグマネージャー取得
        FlagManager flagManager = FlagManager.Instance;

        if (flagManager.flagActive)
        {
            if (isRotation)
            {
                rotationSpeed += accelSpeed;
                rotationSpeed = Mathf.Clamp(rotationSpeed, 0f, maxSpeed);

                float roll = ((curRotation >= 0) ? rotationSpeed : -rotationSpeed) * Time.deltaTime;
                float deff = (curRotation >= 0) ? (curRotation - roll) : -(curRotation - roll);

                if (deff <= 0)
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
                axisTransform.rotation = Quaternion.Inverse(quaternion) * axisTransform.rotation;
                corePlanet.rotation = quaternion * corePlanet.transform.rotation;

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
