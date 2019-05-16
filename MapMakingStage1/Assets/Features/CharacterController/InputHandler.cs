using System.Collections;
using UnityEngine;

namespace SA
{
    public class InputHandler : MonoBehaviour {

        private CharacterCamera characterCamera;
        public float vertical;
        public float horizontal;
        public float inputSumooth = 0.2f;
        public Vector3 moveDir;

    	public float delta;

        public Transform CameraPivot;
        private StateManager states;
        
        //------------------------------------------
        //------------------------------------------
        void Start()
        {
            states = GetComponent<StateManager>();
            states.Init();

            characterCamera = CameraManager.Instance.characterCamera.GetComponent<CharacterCamera>();
            characterCamera.Init(CameraPivot.transform);

        }//Start end 
         //------------------------------------------
        void FixedUpdate()
        {
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

            if (Input.GetButtonDown(InputManager.FollowTarget))
            {
                PlanetCamera camera = CameraManager.Instance.planetCamera;
                camera.SetTarget(states.transform.position, Time.deltaTime * 15f);
            }
        }
        //------------------------------------------
        void GetInput()
        {
            float h = Input.GetAxis(InputManager.Horizontal);
            float v = Input.GetAxis(InputManager.Vertical);

            horizontal = Mathf.Lerp(horizontal, h, inputSumooth);
            vertical = Mathf.Lerp(vertical, v, inputSumooth);
        }

        //------------------------------------------
        void UpdateStates()
        {
            states.vertical = vertical;
            states.horizontal = horizontal;

            Vector3 v = states.vertical * transform.forward;
            Vector3 h = horizontal * transform.right;
            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);

            states.FixedTick(Time.deltaTime);
        }


    }

}