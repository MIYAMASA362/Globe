using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class CameraManager : Singleton<CameraManager>
{
    public enum State
    {
        Start,
        Game
    }

    public State state = State.Game;

    public bool isChange;
    public PlanetCamera planetCamera;
    public CharacterCamera characterCamera;

    private bool onCharacterCamera = false;

    public float timer = 0.0f;
    public bool isStart;

    public GoalScript goal;

    private void Start()
    {
        isChange = true;
    }

    void Update()
    {
        switch(state)
        {
            case State.Start:
                timer += Time.deltaTime;
                float time1 = 1.0f;
                float time2 = 3.0f;

                if (timer > time1 && timer < time2)
                {
                    planetCamera.SetTarget(goal.transform.position, Time.deltaTime * 10f);
                }
                else if(timer > time2)
                {
                    if (timer < time2 + 0.2f) 
                    {
                        planetCamera.SetTarget(characterCamera.followTarget.position, Time.deltaTime * 10f);
                    }

                    if (!planetCamera.isMoveTarget)
                    {
                        characterCamera.tiltAngle = 38f;
                        characterCamera.SetAngle(planetCamera.transform.position);
                        isChange = !isChange;
                        state = State.Game;
                        PlanetScene.Instance.EndOpening();
                    }
                }

                break;
            case State.Game:


                if (Input.GetButtonDown(InputManager.View_Swith))
                {
                    if (!planetCamera.gameObject.activeInHierarchy)
                    {
                        Vector3 dir = -characterCamera.cameraTransform.forward;
                        planetCamera.transform.rotation = characterCamera.cameraTransform.rotation;
                        planetCamera.transform.position = RotationManager.Instance.planetTransform.position + dir * planetCamera.distance;

                        isChange = !isChange;
                    }
                    else
                    {
                        characterCamera.tiltAngle = 38f;
                        onCharacterCamera = true;
                        planetCamera.SetTarget(characterCamera.followTarget.position, Time.deltaTime * 10);
                    }
                }

                if (onCharacterCamera && !planetCamera.isMoveTarget)
                {
                    isChange = !isChange;
                    onCharacterCamera = false;
                    characterCamera.SetAngle(planetCamera.targetPosition);
                }
                break;
        }

        planetCamera.gameObject.SetActive(!isChange);
        characterCamera.gameObject.SetActive(isChange);
    }

    public void SetStart()
    {
        timer = 0.0f;
        isStart = true;
        isChange = false;
        state = State.Start;
        planetCamera.SetTarget(characterCamera.followTarget.position, Time.deltaTime * 10f);
    }
}

