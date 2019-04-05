﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetWalker : MonoBehaviour {

    //--- Attribute -----------------------------

    //--- private ---------------------
    [Header("Stats")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform raycastTransform;
    [SerializeField] LayerMask Hitlayer;
    [Space(10)]
    [SerializeField] float speed = 2f;
    [SerializeField] float runSpeed = 7f;

    [Header("Status")]
    [SerializeField] bool onGround = false;

    //--- private ---------------------
    new Rigidbody rigidbody = null;
    RaycastHit casthit;

    //Input
    bool jump;
    float horizontal;
    float vertical;

    //Move
    Vector3 MoveVec;
    [SerializeField]Vector3 oldPosition;

    //--- MonoBehavior --------------------------

	// Use this for initialization
	void Start ()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        oldPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Get_Input();
    }

    void FixedUpdate()
    {
        Get_RayCast();
        
            oldPosition = this.transform.position;
            Vector3 forward = Vector3.Cross(this.transform.up,-cameraTransform.right).normalized;
            Vector3 right = Vector3.Cross(this.transform.up,forward).normalized;
            MoveVec = (forward * vertical + right * horizontal).normalized;

            rigidbody.AddForce(MoveVec * speed, ForceMode.VelocityChange);
        
    }

    void LateUpdate()
    {
        
    }

    //--- Method --------------------------------

    //Input
    void Get_Input()
    {
        jump        = Input.GetButton(InputManager.Jump);
        horizontal  = Input.GetAxis(InputManager.Horizontal);
        vertical    = Input.GetAxis(InputManager.Vertical);
    }

    //RayCast
    void Get_RayCast()
    {
        Debug.DrawRay(this.transform.position + rigidbody.velocity, -this.transform.up * 0.5f, Color.red);
        if (Physics.Raycast(this.transform.position + rigidbody.velocity, -this.transform.up, out casthit, 0.5f, Hitlayer))
        {
            onGround = true;
        }
        else onGround = false;
    }

    //

}
