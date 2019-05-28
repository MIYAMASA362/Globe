using System.Collections;
using UnityEngine;

public class PlanetCamera : MonoBehaviour
{
    public float moveSpeed = 2.0f;

    public float distance = 13f;

    Vector3 targetDir = Vector3.zero;
    Vector3 corePosition;

    public bool isMoveTarget;
    Vector3 moveTargetPosition;
    private float followSpeed = 0.0f;

    public float holizontal = 0.0f;
    public float vertical = 0.0f;
    public float sumooth = 0.1f;
    [HideInInspector] public Vector3 targetPosition = Vector3.zero;

    private void Start()
    {
        corePosition = RotationManager.Instance.planetTransform.position;
    }


    private void Update()
    {
    }

    //------------------------------------------
    void LateUpdate()
    {
        if (!this.gameObject.activeInHierarchy) return;
        

        targetDir = (corePosition - transform.position).normalized;

        if(isMoveTarget)
        {
            MoveTarget(moveTargetPosition, followSpeed);
        }
        else
        {
            if (CameraManager.Instance.state == CameraManager.State.Game)
            {
                InputMove();
            }
        }

        SetDistance();
        SetRotation();
    }

    private void InputMove()
    {
        float h = Input.GetAxis(InputManager.Camera_Horizontal);
        float v = Input.GetAxis(InputManager.Character_Camera_Vertical);

        DataManager data = DataManager.Instance;
        if (data.commonData.IsCameraReverseHorizontal) h *= -1f;
        if (data.commonData.IsCameraReverseVertical) v *= -1f;

        holizontal = Mathf.Lerp(holizontal, h, sumooth);
        vertical = Mathf.Lerp(vertical, v, sumooth);

        float moveAmount = Mathf.Abs(holizontal) + Mathf.Abs(vertical);
        if (moveAmount < 0.08f) moveAmount = 0.0f;

        Vector3 moveDir = (holizontal * transform.right + vertical * transform.up).normalized;

        transform.position -= moveDir * moveAmount * Time.deltaTime * moveSpeed;
    }

    private void SetDistance()
    {
        targetDir = (corePosition - transform.position).normalized;
        float curDist = (corePosition - transform.position).magnitude;
        float targetDist = curDist - distance;
        transform.position += targetDir * targetDist;
    }

    private void SetRotation()
    {
        targetDir = (corePosition - transform.position).normalized;
        Quaternion q1 = Quaternion.FromToRotation(transform.forward, targetDir) * transform.rotation;
        transform.rotation = q1;
    }

    private void MoveTarget(Vector3 position, float speed)
    {
        Vector3 movePos = position - transform.up * 5f;
        Vector3 dir = (movePos - corePosition);
        targetPosition = movePos - (dir - (dir.normalized * distance));

        transform.position += Vector3.Lerp(transform.position, targetPosition, speed);

        targetDir = (corePosition - transform.position).normalized;
        float curDist = (corePosition - transform.position).magnitude;
        float targetDist = curDist - distance;
        transform.position += targetDir * targetDist;

        float dist = (transform.position - targetPosition).magnitude;
        if (dist < 1f)
        {
            isMoveTarget = false;
        }
    }

    public void SetTarget(Vector3 position, float speed)
    {
        Vector3 dir = (position - corePosition);
        Vector3 targetPos = position + ((dir.normalized * distance) - dir);

        moveTargetPosition = targetPos;
        isMoveTarget = true;
        followSpeed = speed;
    }
}