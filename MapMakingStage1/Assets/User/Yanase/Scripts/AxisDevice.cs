﻿using System.Collections;
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
    FloatType.Type type;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        initTransform = chaseTarget;
    }
	
	// Update is called once per frame
	void Update () {
        float delta = Time.deltaTime;
        if (!onSet)
        {
            rigidbody.isKinematic = false;
            collider.enabled = true;
            Vector3 moveForce = (chaseTarget.position - transform.position).normalized * delta * chaseSpeed;
            rigidbody.AddForceAtPosition(moveForce, transform.position + transform.up * 0.1f);
            transform.position = Vector3.Lerp(transform.position, chaseTarget.position, delta * chaseSpeed * 0.1f);
        }
        else
        {
            rigidbody.isKinematic = true;
            collider.enabled = false;
            transform.position = Vector3.Lerp(transform.position, chaseTarget.position, delta * setSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, chaseTarget.rotation, delta * setSpeed);

            float distance = (transform.position - chaseTarget.position).magnitude;
            if (distance < 0.1f)
            {
                FlagManager.Instance.SetFlag(chaseTarget.position, type);
            }
        }
	}

    public void SetTarget(Transform target, FloatType.Type type)
    {
        if (!target) return;

        chaseTarget = target;
        this.type = type;
        onSet = true;
    }

    public void ResetChase()
    {
        chaseTarget = initTransform;
        onSet = false;
    }
}
