using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class CharacterCamera : MonoBehaviour
{
    private InputHandler inputHandler;
    public Transform followTarget;
    public Transform rotationPivot;
    public Transform cameraTransform;

    public float followSpeed = 6;
    public float mouseSpeed = 3;

    public float minAngle = -5;
    public float maxAngle = 80;
    public float distance = 3f;
    private float initDistance = 0f;

    public float lookAngle;
    public float tiltAngle;

    float turnSmoothing = .1f;
    float smoothX;
    float smoothY;
    float smoothXvelocity;
    float smoothYvelocity;

    private void Awake()
    {
        initDistance = distance;
    }

    private void Start()
    {
        rotationPivot.transform.localPosition = Vector3.zero;
        cameraTransform.localPosition = new Vector3(0.0f, 0.0f, -distance);
    }

    //------------------------------------------
    public void Init(Transform t)
    {
        followTarget = t;
    }

    public void SetInputHandler(InputHandler handle)
    {
        inputHandler = handle;
    }

    private void Update()
    {
    }

    //------------------------------------------
    void LateUpdate()
    {
        if (MySceneManager.IsPausing || MySceneManager.IsFadeing) return;

        Vector3 dist = new Vector3(0.0f, 0.0f, -distance);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, dist, Time.deltaTime * 5f);

        float speed = Time.deltaTime * followSpeed;
        Vector3 targetPosition = Vector3.Lerp(transform.position, followTarget.position, speed);
        transform.position = targetPosition;

        Vector3 planetPosition = RotationManager.Instance.planetTransform.position;
        Vector3 gravityDirection = (planetPosition - transform.position).normalized;

        Quaternion q = Quaternion.FromToRotation(-transform.up, gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
    }
    public void Tick(float d)
    {
        FollowTarget(d);
        if (!this.gameObject.activeInHierarchy) return;

        float h = Input.GetAxis(InputManager.Camera_Horizontal);
        float v = Input.GetAxis(InputManager.Character_Camera_Vertical);

        DataManager data = DataManager.Instance;
        if (data.commonData.IsCameraReverseHorizontal) h *= -1f;
        if (data.commonData.IsCameraReverseVertical) v *= -1f;

        if (inputHandler && inputHandler.isInvisible)
        {
            h *= -1f;
            v *= -1f;
        }
        

        float targetSpeed = mouseSpeed;

        HandleRotations(d, v, h, targetSpeed);
    }
    //------------------------------------------
    void FollowTarget(float d)
    {
        float speed = d * followSpeed;
        Vector3 targetPosition = Vector3.Lerp(transform.position, followTarget.position, speed);
        transform.position = targetPosition;

    }
    //------------------------------------------
    void HandleRotations(float d, float v, float h, float targetSpeed)
    {
        smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
        smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);

        lookAngle += smoothX * targetSpeed;
        tiltAngle -= smoothY * targetSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);

        rotationPivot.localRotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
    }

    public void SetAngle(Vector3 position)
    {
        Vector3 dir = (position - transform.position).normalized;
        Vector3 localDir = transform.InverseTransformDirection(dir);
        localDir.y = 0;

        Quaternion q = Quaternion.LookRotation(-localDir);
       
        tiltAngle = 38.0f;
        lookAngle = q.eulerAngles.y;

        rotationPivot.localRotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
    }

    public void SetDistance(float dist)
    {
        if (!this.gameObject.activeInHierarchy) return;
        distance = dist;
    }

    public void ResetDistance()
    {
        if (!this.gameObject.activeInHierarchy) return;
        distance = initDistance;
    }
}