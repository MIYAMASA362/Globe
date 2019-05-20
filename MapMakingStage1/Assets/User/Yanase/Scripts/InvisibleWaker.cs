using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class InvisibleWaker : MonoBehaviour {

    Transform character;
    public float height = 5f;
    public float offsetHeight = 2f;
    public float followSpeed = 20.0f;
    public float moveSpeed = 10.0f;
    float moveAmount = 0.0f;
    public bool isPlay = false;
    public float horizontal;
    public float vertical;

    public void Init(Transform followTarget)
    {
        character = followTarget;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        if(character)
        {
            height = (character.transform.position - RotationManager.Instance.planetTransform.position).magnitude;
        }
    }

    public void OnNotPlay(float d)
    {
        transform.position = Vector3.Lerp(transform.position, character.position, d * followSpeed);
        SetDistance();

        
    }

    private void SetDistance()
    {
        Vector3 corePosition = RotationManager.Instance.planetTransform.position;
        Vector3 targetDir = (corePosition - transform.position).normalized;
        float curDist = (corePosition - transform.position).magnitude;
        float targetDist = curDist - (height + offsetHeight);
        transform.position += targetDir * targetDist;

        if (((character.position - targetDir * offsetHeight) - transform.position).magnitude > 1f)
        {
            isPlay = true;
        }
        else
        {
            isPlay = false;
        }
    }

    public void OnPlay(float d)
    {
        Vector3 moveDirection = MoveDirection();
        transform.position += moveDirection * moveAmount * d * moveSpeed;
        SetDistance();
    }

    Vector3 MoveDirection()
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 centerPos = RotationManager.Instance.planetTransform.position;
        Vector3 gravityDir = (transform.position - centerPos).normalized;
        Vector3 forward = Vector3.Cross(cameraTransform.right, gravityDir);
        Vector3 right = -Vector3.Cross(forward, gravityDir);

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        return (forward * vertical + right * horizontal).normalized;
    }
}
