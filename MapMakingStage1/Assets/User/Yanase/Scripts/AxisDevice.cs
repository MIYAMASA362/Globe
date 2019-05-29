using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Motion;

public class AxisDevice : MonoBehaviour {

    public Transform chaseTarget;
    private Transform initTransform;
    new private Rigidbody rigidbody;
    new private Collider collider;
    public float chaseSpeed = 1.0f;
    public float setSpeed = 1.0f;
    bool onSet = false;

    public float randomVelocityForce = 0.1f;
    public float randomTorqueForce = 0.1f;
    public float intervalTime = 1.0f;
    private float intervalTimer = 0.0f;
    private Vector3 randamDir;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        initTransform = chaseTarget;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float delta = Time.deltaTime;
        if (!onSet)
        {
            rigidbody.isKinematic = false;
            collider.enabled = true;

            float dist = (chaseTarget.position - transform.position).magnitude;
            Vector3 moveForce = (chaseTarget.position - transform.position).normalized * delta * chaseSpeed;
                intervalTimer += Time.deltaTime;
            if (intervalTimer > intervalTime)
            {
                intervalTimer = 0.0f;

                float x = Random.Range(-1.0f, 1.0f);
                float y = Random.Range(-1.0f, 1.0f);
                float z = Random.Range(-1.0f, 1.0f);
                randamDir = new Vector3(x, y, z).normalized;
            }
            rigidbody.AddForce(randamDir * randomVelocityForce);
            rigidbody.AddRelativeTorque(randamDir * randomTorqueForce);

            transform.position = Vector3.Lerp(transform.position, chaseTarget.position, delta * chaseSpeed * 0.1f);

        }
        else
        {
            rigidbody.isKinematic = true;
            collider.enabled = false;
            transform.position = Vector3.Lerp(transform.position, chaseTarget.position, delta * setSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, chaseTarget.rotation, delta * setSpeed);
        }
	}

    public void SetTarget(Transform target)
    {
        if (!target) return;

        chaseTarget = target;
        onSet = true;
    }

    public void ResetChase()
    {
        chaseTarget = initTransform;
        onSet = false;
    }
}
