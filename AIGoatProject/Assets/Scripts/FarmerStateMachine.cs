using System;
using System.Collections.Generic;
using UnityEngine;

public class StateBase
{
    Dictionary<string, Type> triggers;

    public StateBase(Dictionary<string, Type> triggers)
    {
        this.triggers = triggers;
    }

    public virtual void Enter()
    {

    }
    public virtual void Execute()
    {

    }
    public virtual void Exit()
    {

    }

    public void Trigger(string trigger)
    {

    }
}

public class FarmerStateMachine : MonoBehaviour
{
    StateBase currentState = null;
    StateBase[] states = null;

    private void Start()
    {
        states = new StateBase[]
        {
            new WalkLeft(new Dictionary<string, Type>(){
                {"targetReached",typeof(WalkRight) },
            }),

            new WalkRight(new Dictionary<string, Type>(){
                {"targetReached",typeof(WalkLeft) },
            }),
        };

        SetState<WalkRight>();
    }

    public void Update()
    {
        if(currentState != null)
        {
            currentState.Execute();
        }
    }

    void SetState<T>()
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].GetType() == typeof(T))
            {
                currentState = states[i];
                return;
            }
        }
    }
}

/// <summary>
/// //STATES
/// </summary>
public class WalkLeft : StateBase
{
    public WalkLeft(Dictionary<string, Type> triggers) : base(triggers) { }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        Debug.Log("Kör Left");

        if (1 == 1)
        {
            Trigger("InventoryFull");
            return;
        }
    }

    public override void Exit()
    {

    }
}

public class WalkRight : StateBase
{
    public WalkRight(Dictionary<string, Type> triggers) : base(triggers) { }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        Debug.Log("Kör Right");

        if (1 == 1)
        {
            Trigger("InventoryFull");
            return;
        }
    }

    public override void Exit()
    {

    }
}


