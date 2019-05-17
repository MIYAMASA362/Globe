using System.Collections;
using UnityEngine;

public class PlanetCamera : MonoBehaviour
{
    public float upSpeed = 3.0f;
    public float moveSpeed = 2.0f;

    public float distance = 13f;

    Vector3 targetDir = Vector3.zero;
    Vector3 corePosition;

    public bool isMoveTarget;
    Vector3 moveTargetPosition;
    private float followSpeed = 0.0f;

    public float holizontal = 0.0f;
    public float vertical = 0.0f;
    public float sumooth = 0.5f;

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
            InputMove();
        }
        
        Tick();
    }

    private void InputMove()
    {
        float h = Input.GetAxis(InputManager.Camera_Horizontal);
        float v = Input.GetAxis(InputManager.Character_Camera_Vertical);

        holizontal = Mathf.Lerp(holizontal, h, sumooth);
        vertical = Mathf.Lerp(vertical, v, sumooth);

        transform.position -= holizontal * transform.right * Time.deltaTime * moveSpeed;
        transform.position -= vertical * transform.up * Time.deltaTime * moveSpeed;
    }


    private void Tick()
    {
        targetDir = (corePosition - transform.position).normalized;
        float curDist = (corePosition - transform.position).magnitude;
        float targetDist = curDist - distance;
        transform.position += targetDir * targetDist;

        targetDir = (corePosition - transform.position).normalized;
        Quaternion q1 = Quaternion.FromToRotation(transform.forward, targetDir) * transform.rotation;
        transform.rotation = q1;
    }

    private void MoveTarget(Vector3 position, float speed)
    {
        Vector3 movePos = position - transform.up * 8f;
        Vector3 dir = (movePos - corePosition);
        Vector3 targetPos = movePos - (dir - (dir.normalized * distance));

        transform.position += Vector3.Lerp(transform.position, targetPos, speed);

        targetDir = (corePosition - transform.position).normalized;
        float curDist = (corePosition - transform.position).magnitude;
        float targetDist = curDist - distance;
        transform.position += targetDir * targetDist;

        float dist = (transform.position - targetPos).magnitude;
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