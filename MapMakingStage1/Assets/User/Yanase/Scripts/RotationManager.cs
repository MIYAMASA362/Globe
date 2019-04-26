using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : Singleton<RotationManager> {

    [SerializeField] private Transform corePlanet = null;
    [SerializeField] private Transform rotationTarget = null;
    [SerializeField] private float accelSpeed = 1.0f;
    [SerializeField] private float maxSpeed = 1.0f;

    [Header("回転表示オブジェクト群"), SerializeField]
    public GameObject ArrowObject = null;

    public Material ArrowMaterial;

    [Space(4)]
    public float rotationSpeed = 5.0f;

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
    }
	
    //Update
	private void Update ()
    {
        PlanetRotation(rotationAngle);
    }

    //FixedUpdate
    private void FixedUpdate()
    {
        //if (!isRotation)
        //{
        //    rotationSpeed = 0.0f;
        //}
    }

    private void PlanetRotation()
    {
        // フラグマネージャー取得
        FlagManager flagManager = FlagManager.Instance;
        isRotation = false;

        if (flagManager.flagActive)
        {
            if (Input.GetAxis(InputManager.Left_AxisRotation) >= 0.05f)
            {
                if (rotationSpeed < 0) rotationSpeed = 0.0f;

                rotationSpeed += accelSpeed;
                ArrowMaterial.SetTextureOffset("_MainTex",new Vector2(0f,0f));
                isRotation = true;
            }
            if (Input.GetAxis(InputManager.Right_AxisRotation) <= -0.05f)
            {
                if (rotationSpeed > 0) rotationSpeed = 0.0f;

                rotationSpeed -= accelSpeed;
                ArrowMaterial.SetTextureOffset("_MainTex", new Vector2(1f, 0f));
                isRotation = true;
            }

            if (rotationSpeed != 0)
                ArrowObject.SetActive(true);
            else
                ArrowObject.SetActive(false);

            rotationSpeed = Mathf.Clamp(rotationSpeed, -maxSpeed, maxSpeed);

            Quaternion quaternion;
            Transform axisTransform = flagManager.flagTransform;

            quaternion = Quaternion.AngleAxis(-rotationSpeed * Time.deltaTime, axisTransform.up);
            // 回転値を合成
            axisTransform.rotation = Quaternion.Inverse(quaternion) * axisTransform.rotation;
            rotationTarget.rotation = quaternion * rotationTarget.transform.rotation;

            ArrowObject.transform.position = flagManager.flagTransform.position;
            ArrowObject.transform.rotation = Quaternion.Inverse(quaternion) * ArrowObject.transform.rotation;
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
                Quaternion quaternion;
                Transform axisTransform = flagManager.flagTransform;
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

                quaternion = Quaternion.AngleAxis(roll, axisTransform.up);
                // 回転値を合成
                axisTransform.rotation = Quaternion.Inverse(quaternion) * axisTransform.rotation;
                rotationTarget.rotation = quaternion * rotationTarget.transform.rotation;

                ArrowObject.transform.position = flagManager.flagTransform.position;
                ArrowObject.transform.rotation = Quaternion.Inverse(quaternion) * ArrowObject.transform.rotation;
            }
            else
            {
                InputRotation();
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

    public Vector3 GetMoveDir(Vector3 position)
    {
        // フラグマネージャー取得
        FlagManager flagManager = FlagManager.Instance;

        if (!flagManager.flagActive || 
            (rotationSpeed < 0.1f && rotationSpeed > -0.1f))
            return Vector3.zero;

        Transform flagTransform = flagManager.flagTransform;
        Vector3 moveDir = Vector3.Cross(flagTransform.up, position - flagTransform.position).normalized * ((rotationSpeed * 1.5f) * Time.deltaTime);

        return moveDir;
    }

    public float GetSpeed()
    {
        return rotationSpeed;
    }
}
