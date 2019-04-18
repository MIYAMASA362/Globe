using System.Collections;
using UnityEngine;
using FrameWork.Camera;

namespace SA
{
    public class InputHandler : MonoBehaviour {


        public float vertical;
        public float horizontal;
        public Vector3 moveDir;

    	public float delta;
        public GameObject pi;

        public Transform CameraPivot;

        private StateManager states;
        public CharacterCamera characterCamera;

        //------------------------------------------
        //------------------------------------------
        void Start()
        {

            states = GetComponent<StateManager>();
            states.Init();
            
            characterCamera.Init(CameraPivot.transform);
        }//Start end 
         //------------------------------------------
        void FixedUpdate()
        {
            if(!characterCamera)
                characterCamera = CameraManager.Instance.characterCamera.GetComponent<CharacterCamera>();

            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            characterCamera.Tick(delta);

        }//Update end
         //------------------------------------------
        void Update()
        {
            delta = Time.deltaTime;
            states.Tick(delta);
        }
        //------------------------------------------
        void GetInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }
        //------------------------------------------
        void UpdateStates()
        {
            states.vertical = vertical;
            states.horizontal = horizontal;

            Vector3 v = states.vertical * pi.transform.forward;
            Vector3 h = horizontal * pi.transform.right;
            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);


            states.FixedTick(Time.deltaTime);
        }


    }

}