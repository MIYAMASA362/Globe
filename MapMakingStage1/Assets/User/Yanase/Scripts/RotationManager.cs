using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : Singleton<RotationManager> {

    [SerializeField] private Transform corePlanet = null;
    [SerializeField] private Transform rotationTarget = null;
    [SerializeField] private float accelSpeed = 1.0f;
    [SerializeField] private float maxSpeed = 1.0f;

    private float rotationSpeed = 0.0f;
    private bool isRotation = false;

    public Transform planetTransform
    {
        get { return corePlanet; }
    }

    //Initialize
    private void Start ()
    {
    }
	
    //Update
	private void Update ()
    {
        PlanetRotation();
    }

    //FixedUpdate
    private void FixedUpdate()
    {
        if(!isRotation) rotationSpeed *= 0.9f;    
    }

    private void PlanetRotation()
    {
        // フラグマネージャー取得
        FlagManager flagManager = FlagManager.Instance;
        isRotation = false;

        if (flagManager.flagActive)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                rotationSpeed += accelSpeed;
                isRotation = true;
            }
            if (Input.GetKey(KeyCode.X))
            {
                rotationSpeed -= accelSpeed;
                isRotation = true;
            }

            rotationSpeed = Mathf.Clamp(rotationSpeed, -maxSpeed, maxSpeed);
            //Debug.Log(speed);

            Quaternion quaternion;
            Transform axisTransform = flagManager.flagTransform;

            quaternion = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, axisTransform.up);
            // 回転値を合成
            axisTransform.rotation = quaternion * axisTransform.rotation;
            rotationTarget.rotation = quaternion * rotationTarget.transform.rotation;
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
        Vector3 moveDir = Vector3.Cross(flagTransform.up, position - flagTransform.position).normalized * (rotationSpeed * Time.deltaTime);

        return moveDir;
    }

    public float GetSpeed()
    {
        return rotationSpeed;
    }
}
