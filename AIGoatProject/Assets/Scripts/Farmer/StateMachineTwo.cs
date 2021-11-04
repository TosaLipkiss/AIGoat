using UnityEngine;

public interface IFarmerstate
{
    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent);
    public void Execute();
    public void Exit();

}

public class StateMachineTwo : MonoBehaviour
{
    IFarmerstate currentState;

    FarmerAgent farmerAgent;

    public GameObject rayObject;

    public bool alreadyGreetPlayer = false;

    public float delayTimer;

    public int disturbedCountdown = 2;

    void Start()
    {
        farmerAgent = GetComponent<FarmerAgent>();
        currentState = new FarmerRandowmWalk();
        currentState.Enter(this, farmerAgent);
    }

    void Update()
    {
        currentState.Execute();
    }

    public void ChangeState(IFarmerstate newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this, farmerAgent);
    }

    public void RandomState()
    {
        int randomState = Random.Range(0, 4);

        if (randomState <= 2)
        {
            ChangeState(new FarmerRandowmWalk());
        }
        else if (randomState == 3)
        {
            ChangeState(new FarmerIdle());
        }
    }

    public void FarmerRandomState()
    {
        int randomState = Random.Range(0, 4);

        if (randomState <= 2)
        {
            ChangeState(new FarmerRandowmWalk());
        }
        else if (randomState == 3)
        {
            ChangeState(new FarmerIdle());
        }
    }

    public void SetPlayerInfront()
    {
        if (!alreadyGreetPlayer)
        {
            alreadyGreetPlayer = true;

            ChangeState(new FarmerGreetPlayer());
        }
        else
        {
            ChangeState(new FarmerPlayerInfront());
        }
    }

    public void DelayTimer()
    {
        while (delayTimer < 20f)
        {
            delayTimer += Time.deltaTime;
        }

        delayTimer = 0f;
    }
}

#region randomIdlingStates (walk,Idle,Flute)
public class FarmerRandowmWalk : IFarmerstate
{
    StateMachineTwo stateMachine;
    FarmerAgent characterAgent;

    float timer;
    float stateDuration;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        stateDuration = Random.Range(2f, 5f);

        this.stateMachine = stateMachineTwo;
        this.characterAgent = farmerAgent;

        farmerAgent.PlayWalkSound();
        farmerAgent.WalkAnimation();
    }


    public void Execute()
    {
        timer += Time.deltaTime;

        characterAgent.WalkAround();

        if (characterAgent.CheckPlayerInfront())
        {
            stateMachine.SetPlayerInfront();
        }

        if (timer > stateDuration)
        {
            stateMachine.FarmerRandomState();
        }
    }

    public void Exit()
    {
        characterAgent.StopOtherGoatSound();
        characterAgent.StopWalking();
    }
}

public class FarmerIdle : IFarmerstate
{
    StateMachineTwo stateMachine;
    FarmerAgent characterAgent;

    float timer;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachine = stateMachineTwo;
        this.characterAgent = farmerAgent;

        int randomAnimation = Random.Range(0, 8);

        if (randomAnimation <= 2)
        {
            farmerAgent.IdleAnimation();
        }
        else if (randomAnimation == 3)
        {
            farmerAgent.IdleGaspAnimation();
            farmerAgent.IdleGaspSound();
        }
        else if (randomAnimation >= 4 || randomAnimation <= 6)
        {
            farmerAgent.IdleLookAroundpAnimation();
        }
        else if (randomAnimation == 7)
        {
            farmerAgent.IdleHmmAnimation();
        }
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (timer > 3f)
        {
            stateMachine.FarmerRandomState();
        }

        if (characterAgent.CheckPlayerInfront())
        {
            stateMachine.SetPlayerInfront();
        }
    }

    public void Exit()
    {
        characterAgent.StopGoatSound();
    }
}

#endregion


#region InteractPlayerStates
public class FarmerPlayerInfront : IFarmerstate
{
    StateMachineTwo stateMachine;
    FarmerAgent characterAgent;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachine = stateMachineTwo;
        this.characterAgent = farmerAgent;

        int randomAnimation = Random.Range(0, 2);

        if (randomAnimation == 0)
        {
            farmerAgent.PlayerInfrontAnimation();
        }
        else if (randomAnimation == 1)
        {
            farmerAgent.WhatsYouUpToAnimation();
            farmerAgent.WhatYouUpToSound();
        }
    }

    public void Execute()
    {
        if (!characterAgent.CheckPlayerInfront())
        {
            stateMachine.delayTimer += Time.deltaTime;
            if (stateMachine.delayTimer > 2f)
            {
                stateMachine.delayTimer = 0f;
                stateMachine.FarmerRandomState();
            }
        }
    }

    public void Exit()
    {

    }
}

public class FarmerGreetPlayer : IFarmerstate
{
    StateMachineTwo stateMachine;
    FarmerAgent characterAgent;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachine = stateMachineTwo;
        this.characterAgent = farmerAgent;

        farmerAgent.GreetPlayerAnimation();
        farmerAgent.GreetPlayerSound();
    }

    public void Execute()
    {
        if (!characterAgent.CheckPlayerInfront())
        {
            stateMachine.delayTimer += Time.deltaTime;
            if (stateMachine.delayTimer > 2f)
            {
                stateMachine.delayTimer = 0f;
                stateMachine.FarmerRandomState();
            }
        }
    }

    public void Exit()
    {

    }
}

public class FarmerDistrubed : IFarmerstate
{
    StateMachineTwo stateMachine;
    FarmerAgent characterAgent;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachine = stateMachineTwo;
        this.characterAgent = farmerAgent;

        int randomAnimation = Random.Range(0, 1);

        if (randomAnimation == 0)
        {
            farmerAgent.DisturbedAnimation();
            farmerAgent.DisturbedSound();
        }

        if (stateMachineTwo.disturbedCountdown == 0)
        {
            stateMachineTwo.disturbedCountdown = 2;
        }
    }

    public void Execute()
    {
        if (!characterAgent.CheckPlayerInfront())
        {
            stateMachine.delayTimer += Time.deltaTime;
            if (stateMachine.delayTimer > 2f)
            {
                stateMachine.delayTimer = 0f;
                stateMachine.FarmerRandomState();
            }
        }
    }

    public void Exit()
    {

    }
}
#endregion
