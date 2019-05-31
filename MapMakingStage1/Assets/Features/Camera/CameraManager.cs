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

    public float cameraGoalDistance = 8.0f;
    private float dist = 0.0f;

    private void Start()
    {
        isChange = true;
        dist = planetCamera.distance;
    }

    void Update()
    {
        switch(state)
        {
            case State.Start:
                timer += Time.deltaTime;
                float time1 = 1.5f;
                float time2 = 3.5f;

                if (timer > time1 && timer < time2)
                {
                    if(timer < time2 - 1.0f)
                        planetCamera.SetTarget(goal.transform.position + planetCamera.transform.forward * 2f, Time.deltaTime * 10f);

                    planetCamera.distance = Mathf.Lerp(planetCamera.distance, cameraGoalDistance, Time.deltaTime * 3.0f);
                }
                else if(timer > time2)
                {
                    planetCamera.distance = Mathf.Lerp(planetCamera.distance, dist, Time.deltaTime * 5.0f);
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
                        timer = 0.0f;
                        PlanetScene.Instance.EndOpening();
                    }
                }

                break;
            case State.Game:

                timer += Time.deltaTime;
                if (timer < 1.0f) return;

                if (Input.GetButtonDown(InputManager.View_Swith))
                {
                    if (!planetCamera.gameObject.activeInHierarchy)
                    {
                        Vector3 dir = -characterCamera.cameraTransform.forward;
                        planetCamera.transform.rotation = characterCamera.cameraTransform.rotation;
                        planetCamera.transform.position = RotationManager.Instance.planetTransform.position + dir * planetCamera.distance;

                        isChange = !isChange;
                        timer = 0.0f;
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
                    timer = 0.0f;
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

