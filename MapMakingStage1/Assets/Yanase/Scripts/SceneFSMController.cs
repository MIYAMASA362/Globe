using UnityEngine;
using System.Collections.Generic;


public class SceneFSMController<T>
{
    private T Owner;
    public FSMState<T> CurrentState { get; private set; }
    private FSMState<T> PreviousState;

    private Dictionary<string, FSMState<T>> stateRef;

    public void Awake()
    {
        CurrentState = null;
        PreviousState = null;
    }

    public SceneFSMController(T owner)
    {
        Owner = owner;
        stateRef = new Dictionary<string, FSMState<T>>();
    }

    // call from Scene
    public void OnUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
    }

    public void ChangeState(string state_name, object param = null)
    {
        ChangeState(stateRef[state_name], param);
    }

    public void ChangeState(FSMState<T> NewState, object param = null)
    {
        PreviousState = CurrentState;

        if (CurrentState != null)
        {
            CurrentState.OnDestroy();
        }

        CurrentState = NewState;

        if (CurrentState != null)
        {
            //Debug.Log(CurrentState.stateName);
            CurrentState.OnStart(param);
        }
    }

    public void RevertToPreviousState()
    {
        if (PreviousState != null)
        {
            ChangeState(PreviousState);
        }
    }

    public FSMState<T> RegisterState(string state_name, FSMState<T> state)
    {
        state.RegisterEntity(Owner);
        stateRef.Add(state_name, state);
        state.stateName = state_name;
        return state;
    }

    public void UnregisterState(FSMState<T> state)
    {
        stateRef.Remove(state.stateName);
    }
}
