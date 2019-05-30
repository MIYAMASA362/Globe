using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class StateManager : MonoBehaviour
    {
        public enum State
        {
            Start,
            GameMain,
            End
        }

        public State state = State.GameMain;

        public ParticleSystem circleParticle;
        public AxisDevice axisDevice;

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
        public GroundType.Type groundType;
        public bool onGround = false;
        public bool OnAxis = false;
        public bool OnUI = false;

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

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation, delta * 5.0f);

            onGround = OnGround();
            
            if (!onGround)
                rigid.AddForce(gravityDirection * -gravity);

            if(state == State.Start)
            {
                if(onGround)
                {
                    anim.SetTrigger("impact");

                    if(anim.GetBool("getUp"))
                    {
                        state = State.GameMain;
                    }
                }
            }

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
                    OnUI = false;
                    OnAxis = JudgeAxis(hit.collider.gameObject);

                    SetParent(hit.collider.gameObject);

                    GroundType ground = hit.collider.gameObject.GetComponent<GroundType>();
                    if (ground)
                    {
                        groundType = ground.type;
                    }
                }
            }

            return isHit;
        }

        private bool JudgeAxis(GameObject hitObject)
        {
            if (hitObject.tag != "Axis") return false;

            axisTransform = hitObject.transform;

            if (axisObject == axisTransform.parent.gameObject || !FlagManager.Instance.flagActive)
            {
                OnUI = true;
            }

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

            Transform planetTransform = RotationManager.Instance.planetTransform;
            if (transform.parent != planetTransform)
            {
                transform.parent = planetTransform;
                GetComponent<PlanetWalker>().oldPosition = transform.localPosition;
            }
        }

        public void Tick(float d)
        {
            delta = d;

            if(OnAxis)
            {
                FlagSet();
            }

            if (FlagManager.Instance.flagActive)
            {
                if(!circleParticle.isPlaying)
                {
                    if (Input.GetButtonDown(InputManager.Change_AscDes))
                        circleParticle.Play();

                    if (Input.GetButton(InputManager.Right_AxisRotation) || Input.GetButton(InputManager.Left_AxisRotation))
                        circleParticle.Play();
                }
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
                        if (flagManager.DestoyFlag(axisTransform.position))
                        {
                            axisObject = null;
                            axisDevice.ResetChase();
                            circleParticle.Play();
                        }
                        else anim.SetBool("not", true);
                    }
                    else anim.SetBool("not", true);
                }
                else
                {
                    Transform target = axisTransform.GetChild(0);
                    if (axisDevice.chaseTarget == target) return;

                    FlagManager.Instance.SetFlag(target.position, axisTransform.GetComponent<FloatType>().type);
                    axisDevice.SetTarget(target);
                    circleParticle.Play();
                    axisObject = axisTransform.parent.gameObject;
                }
            }
        }

        public void EndTick()
        {
            anim.SetTrigger("end");
        }
    }

}//end
