using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : Singleton<RotationManager> {

    [SerializeField, Header("星")]
    private Transform corePlanet = null;
    [SerializeField, Header("回転させるオブジェクト")]
    private Transform rotationTarget = null;
    [SerializeField, Header("回転加速度")]
    private float accelSpeed = 1.0f;
    [SerializeField, Header("回転最高速度")]
    private float maxSpeed = 1.0f;

    private float speed = 0.0f;

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
        speed *= 0.1f;    
    }

    private void PlanetRotation()
    {
        // フラグマネージャー取得
        FlagManager flagManager = FlagManager.Instance;

        if (flagManager.flagActive)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                speed += accelSpeed;
            }
            if (Input.GetKey(KeyCode.X))
            {
                speed -= accelSpeed;
            }
            speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
            //Debug.Log(speed);

            Quaternion quaternion;
            Transform axisTransform = flagManager.flagTransform;

            quaternion = Quaternion.AngleAxis(speed * Time.deltaTime, axisTransform.up);
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
            (speed < 0.1f && speed > -0.1f))
            return Vector3.zero;

        Transform flagTransform = flagManager.flagTransform;
        Vector3 moveDir = Vector3.Cross(flagTransform.up, position - flagTransform.position).normalized;
        if (speed > 0.0f) moveDir *= -1;

        return moveDir;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
