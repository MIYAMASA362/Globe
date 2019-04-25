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
        public float turnSpeed = 2;
        [SerializeField] float rayStartPosition = 0.4f;
        [SerializeField] float rayEndPosition = -0.5f;

        public float angle;
        public Vector3 cross;
        public Transform anglePivot;
        public float crossMult = 6;
        [Header("States")]
        public bool onGround = false;
        public bool OnAxis = false;
        public GameObject activeModel;
        [HideInInspector]
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
            delta = d;
            onGround = OnGround();
            Transform gravityCenter = RotationManager.Instance.planetTransform;

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

            if (Physics.Raycast(origin, dir, out hit, rayLength, ignoreLayers))
            {
                if(hit.collider.isTrigger)
                {
                    isHit = false;
                }
                else
                {
                    isHit = true;
                    Vector3 targetPosition = hit.point + (transform.up * 0.2f);
                    transform.position = targetPosition;//Vector3.Lerp

                    // 設置軸の上にいるかどうか
                    OnAxis = JudgeAxis(hit.collider.gameObject);

                    if (hit.collider.gameObject.tag == "FloatGround")
                        transform.parent = hit.collider.transform;
                    else
                        transform.parent = null;
                }
            }

            if(!isHit) transform.parent = null;

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
            Transform parent = hitObject.transform.parent;

            if (parent)
            {
                Rigidbody castRigid = parent.GetComponent<Rigidbody>();
                if (castRigid)
                {
                    transform.parent = parent;
                    return;
                }
            }

            if (transform.parent && hitObject.layer.ToString() == "Water")
            {
                PlanetWalker planetWalker = GetComponent<PlanetWalker>();
                transform.localPosition = planetWalker.oldPosition;
                planetWalker.oldPosition = transform.position;
            }
            else
            {
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
                        if(flagManager.DestoyFlag()) axisObject = null;
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
