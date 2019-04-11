using System.Collections;
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
        public float runSpeed = 6;
        public float turnSpeed = 2;
        public float toGround = .5f;

        public float angle;
        public Vector3 cross;
        public Transform anglePivot;
        public float crossMult = 6;
        [Header("States")]
        public bool onGround = false;
        public bool OnAxis = false;
        public bool run;
        public GameObject activeModel;
        [HideInInspector]
        public Animator anim;

        public Rigidbody rigid;
        [HideInInspector]
        public float delta;

        public LayerMask ignoreLayers;


        public Vector3 groundNormal;
        public Transform raycastTransform;

        public Transform gravityCenter;
        public Vector3 gravityDirection;
        public float gravity = 3f;


        public float newMov;

        public float e;
        public float r;

        public Transform camHolder;


        private Transform axisTransform = null;
        private GameObject axisObject = null;

        //------------------------------------------
        //------------------------------------------
        public void Init()
        {

            SetupAnimator();
            rigid = GetComponent<Rigidbody>();
            // rigid.angularDrag = 999;
            // rigid.drag = 4;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            gameObject.layer = 8;
            ignoreLayers = ~(1 << 9);

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
            delta = d;
            onGround = OnGround();


            // vertical = Input.GetAxis("Vertical");
            // horizontal = Input.GetAxis("Horizontal");

            //rigid.drag = (moveAmount > 0  || onGround == false) ? 0 : 4; 

            float targetSpeed = moveSpeed;
            if (run)
                targetSpeed = runSpeed;


            gravityDirection = (transform.position - gravityCenter.position).normalized;

            if (onGround)
            {

                transform.up -= (transform.up - gravityDirection);

            }
            else
            {
                transform.up -= (transform.up - gravityDirection);
                rigid.AddForce(gravityDirection * -gravity);

            }

            Vector3 targetDir = moveDir;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            //	 angle = Vector3.Angle(targetDir, transform.forward);
            // cross = Vector3.Cross(targetDir, transform.forward);
            // Vector3 anglePivotVec = new Vector3(0,(-cross.y * crossMult) * moveAmount,0);
            // anglePivot.localEulerAngles = anglePivotVec;

            Vector3 eulerAnglesRotation = Quaternion.LookRotation(groundNormal).eulerAngles;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * (moveAmount * moveAmountMult) * turnSpeed);


            transform.rotation = targetRotation;

            // transform.rotation = targetRotation;

            HandleMovementAnimations();

        }//Tick end
         //------------------------------------------


        public bool OnGround()
        {
            bool r = false;

            Vector3 origin = transform.position + (raycastTransform.TransformDirection(Vector3.up) * toGround);
            Vector3 dir = raycastTransform.TransformDirection(-Vector3.up);
            float dis = toGround + 0.3f;
            RaycastHit hit;
            Debug.DrawRay(origin, dir * dis);
            if (Physics.Raycast(origin, dir, out hit, dis, ignoreLayers))
            {
                r = true;
                Vector3 targetPosition = hit.point + (transform.up * 0.2f);
                transform.position = targetPosition;//Vector3.Lerp

                // 設置軸の上にいるかどうか
                OnAxis = JudgeAxis(hit.collider.gameObject);
            }

            return r;
        }

        private bool JudgeAxis(GameObject hitObject)
        {
            if (hitObject.tag != "Axis") return false;

            axisTransform = hitObject.transform;

            return true;
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
            if (Input.GetKeyDown(KeyCode.C))
            {
                FlagManager flagManager = FlagManager.Instance;
                if (flagManager.flagActive)
                {
                    if (axisObject == axisTransform.parent.gameObject)
                    {
                        flagManager.DestoyFlag();
                        axisObject = null;
                    }
                }
                else
                {
                    flagManager.SetFlag(axisTransform.position);
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
