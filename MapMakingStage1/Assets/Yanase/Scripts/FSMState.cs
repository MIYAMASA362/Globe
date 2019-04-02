using UnityEngine;
using System.Collections.Generic;


public abstract class FSMState<T>
{
    public string stateName;
    protected T entity;

    public void RegisterEntity(T entity)
    {
        this.entity = entity;
    }

    public virtual void OnStart(object param) { }
    public virtual void OnUpdate() { }
    public virtual void OnDestroy() { }
    public virtual void OnTouched(int type, int value = 0) { }
}