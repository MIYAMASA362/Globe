using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : Singleton<RotationManager> {

    [SerializeField] private Transform corePlanet = null;
    [SerializeField] private Transform rotationTarget = null;

    [Header("SE")]
    private AudioSource planetAudio;
    public AudioClip SE_PlanetRotation;
    public float SE_FlagPlugInVolume = 2f;

    [Header("回転表示オブジェクト群"), SerializeField]
    public GameObject ArrowObject = null;

    public Material ArrowMaterial;

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
        ArrowMaterial = Resources.Load<Material>("Materials/ArrowMaterial");
        ArrowMaterial.SetTextureOffset("_MainTex", new Vector2(0f, 0f));
        ArrowObject.SetActive(false);

        planetAudio = corePlanet.GetComponent<AudioSource>();
    }
	
    //Update
	private void Update ()
    {
        PlanetRotation(rotationAngle);
    }

    //FixedUpdate
    private void LateUpdate()
    {
        if(planetAudio)
        {
            if(isRotation)
            {
                if(!planetAudio.isPlaying) planetAudio.PlayOneShot(SE_PlanetRotation, SE_FlagPlugInVolume);
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
                ArrowObject.SetActive(true);
            else
                ArrowObject.SetActive(false);

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
                rotationTarget.rotation = quaternion * rotationTarget.transform.rotation;

                ArrowObject.transform.position = flagManager.flagTransform.position;
                ArrowObject.transform.rotation = Quaternion.Inverse(quaternion) * ArrowObject.transform.rotation;

                corePlanet.GetComponent<Animator>().SetBool("vibration", true);
            }
            else
            {
                InputRotation();
                rotationSpeed = 0.0f;

                corePlanet.GetComponent<Animator>().SetBool("vibration", false);
            }
        }
    }

    private void InputRotation()
    {
        if (Input.GetButton(InputManager.Left_AxisRotation))
        {
            curRotation = -rotationAngle;
            ArrowMaterial.SetTextureOffset("_MainTex", new Vector2(0f, 0f));
            isRotation = true;
        }
        if (Input.GetButton(InputManager.Right_AxisRotation))
        {
            curRotation = rotationAngle;
            ArrowMaterial.SetTextureOffset("_MainTex", new Vector2(1f, 0f));
            isRotation = true;
        }
    }

    public float GetSpeed()
    {
        return rotationSpeed;
    }
}
