using System.Collections;
using UnityEngine;
using Klak.Motion;

namespace SA
{
    public class InputHandler : MonoBehaviour {

        private CharacterCamera characterCamera;
        public PlanetWalker planetWalker;
        public InvisibleWaker invisibleWalker;
        public SmoothFollow backPack;
        public float vertical;
        public float horizontal;
        public float inputSumooth = 0.2f;
        public Vector3 moveDir;

    	public float delta;

        public Transform CameraPivot;
        private StateManager states;

        public bool isInvisible = false;

        //------------------------------------------
        //------------------------------------------
        void Start()
        {
            states = GetComponent<StateManager>();
            states.Init();

            characterCamera = CameraManager.Instance.characterCamera;
            characterCamera.Init(CameraPivot);
            characterCamera.SetInputHandler(this);

            invisibleWalker.Init(CameraPivot);

        }//Start end 
         //------------------------------------------
        void FixedUpdate()
        {
            if (MySceneManager.IsPausing || MySceneManager.IsOption) return;

            delta = Time.fixedDeltaTime;
            states.FixedTick(delta);

            if (states.state == StateManager.State.GameMain)
            {
                GetInput();
                UpdateStates();
                characterCamera.Tick(delta);
            }

        }//Update end
         //------------------------------------------
        void Update()
        {
            if (MySceneManager.IsPausing || MySceneManager.IsOption)
            {
                backPack.enabled = false;
                return;
            }
            else
            {
                backPack.enabled = true;
            }

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

            isInvisible = false;

            if (CameraManager.Instance.characterCamera.gameObject.activeInHierarchy)
            {
                if ((Input.GetAxis(InputManager.OnInvisible) > 0.1f))
                {
                    isInvisible = true;
                }
            }

            planetWalker.horizontal = horizontal;
            planetWalker.vertical = vertical;
            invisibleWalker.horizontal = horizontal;
            invisibleWalker.vertical = vertical;
        }

        //------------------------------------------
        void UpdateStates()
        {
            if (isInvisible)
            {
                planetWalker.moveAmount *= 0.8f;
                planetWalker.horizontal *= 0.8f;
                planetWalker.vertical *= 0.8f;
                planetWalker.rigidbody.velocity *= 0.5f;
                planetWalker.anim.SetFloat("move", planetWalker.moveAmount);

                invisibleWalker.OnPlay(delta);

                characterCamera.Init(invisibleWalker.transform);
                characterCamera.SetDistance(0f);
            }
            else
            {
                invisibleWalker.OnNotPlay(delta);
                if(!invisibleWalker.isPlay)
                {
                    characterCamera.Init(CameraPivot);
                    characterCamera.ResetDistance();
                }

                planetWalker.FixedTick(delta);
            }
        }


    }

}