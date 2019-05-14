﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class StateManager : MonoBehaviour
    {
        public Quaternion targetRotation;

        public float vertical;
        [Header("Inputs")]
        public float horizontal;
        public float moveAmount;
        public Vector3 moveDir;

        [Header("Stats")]
        public float moveSpeed = 3;
        public float moveAmountMult = 0.3f;
        public float turnSpeed = 2;
        [SerializeField] float rayStartPosition = 0.4f;
        [SerializeField] float rayEndPosition = -0.5f;
        [SerializeField] float rayRadius = 0.5f;
        public float characterHeight = 0.4f;

        public float angle;
        public Vector3 cross;
        public float crossMult = 6;
        [Header("States")]
        public bool onGround = false;
        public bool OnAxis = false;

        public GameObject activeModel;
        public Animator anim;
        public Rigidbody rigid;

        [HideInInspector]
        public float delta;

        public LayerMask ignoreLayers;

        public Vector3 groundNormal;
        public Transform raycastTransform;

        public Vector3 gravityDirection;
        public float gravity = 3f;

        private Transform axisTransform = null;
        private GameObject axisObject = null;

        //------------------------------------------
        //------------------------------------------
        public void Init()
        {

            SetupAnimator();
            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.drag = 3;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            gameObject.layer = 8;

            OnAxis = false;

        }//Init end
         //------------------------------------------
        void SetupAnimator()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                if (anim == null)
                {
                    Debug.Log("No model found");
                }
                else
                {
                    activeModel = anim.gameObject;
                }
            }

            if (anim == null)
                anim = activeModel.GetComponent<Animator>();

        }
        //------------------------------------------
        public void FixedTick(float d)
        {
            Transform gravityCenter = RotationManager.Instance.planetTransform;
            gravityDirection = (transform.position - gravityCenter.position).normalized;
            delta = d;

            onGround = OnGround();

            if (!onGround) rigid.AddForce(gravityDirection * -gravity);

            HandleMovementAnimations();

        }//Tick end
         //------------------------------------------


        public bool OnGround()
        {
            bool isHit = false;

            Vector3 origin = transform.position + (raycastTransform.up * rayStartPosition);

            float rayLength = rayStartPosition - rayEndPosition;
            Vector3 dir = -raycastTransform.up * rayLength;
            RaycastHit hit;

            if (Physics.SphereCast(origin, rayRadius, dir, out hit, rayLength, ignoreLayers))
            {
                if (!hit.collider.isTrigger)
                {
                    isHit = true;
                    Vector3 targetPosition = transform.position + (characterHeight - hit.distance) * gravityDirection;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

                    // 設置軸の上にいるかどうか
                    OnAxis = JudgeAxis(hit.collider.gameObject);

                    SetParent(hit.collider.gameObject);
                }
            }

            return isHit;
        }

        private bool JudgeAxis(GameObject hitObject)
        {
            if (hitObject.tag != "Axis") return false;

            axisTransform = hitObject.transform;

            return true;
        }

        private void SetParent(GameObject hitObject)
        {
            if (hitObject.tag == "FloatMesh")
            {
                if (transform.parent != hitObject.transform)
                {
                    transform.parent = hitObject.transform;
                    GetComponent<PlanetWalker>().oldPosition = transform.localPosition;
                }
                else
                {
                    transform.parent = hitObject.transform;
                }
                return;
            }

            if (transform.parent)
            {
                GetComponent<PlanetWalker>().oldPosition = transform.position;
                transform.parent = null;
            }
        }

        public void Tick(float d)
        {
            delta = d;

            if(OnAxis)
            {
                FlagSet();
            }
        }

        void FlagSet()
        {
            if (Input.GetButtonDown(InputManager.Set_EarthAxis))
            {
                FlagManager flagManager = FlagManager.Instance;
                if (flagManager.flagActive)
                {
                    if (axisObject == axisTransform.parent.gameObject)
                    {
                        if(flagManager.DestoyFlag(axisTransform.position)) axisObject = null;
                    }
                }
                else
                {
                    flagManager.SetFlag(axisTransform.position, axisTransform.GetComponent<FloatType>().type);
                    axisObject = axisTransform.parent.gameObject;
                }
            }
        }

        void HandleMovementAnimations()
        {
            //	anim.SetFloat("vertical",moveAmount,0.1f,delta);
        }
        //------------------------------------------

    }

}//end
