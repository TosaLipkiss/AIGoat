using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Istate
{
    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent);
    public void Execute();
    public void Exit();

}
public class StateMachine : MonoBehaviour
{
    Istate currentState;

    public AudioClip greetingsSound;
    public AudioClip whatYouUpToSound;
    public AudioClip gasp;

    public AudioClip walkSteps;
    public AudioClip flute;

    CharacterAgent characterAgent;

    public GameObject rayObject;

    public bool alreadyGreetPlayer = false;

    void Start()
    {
        characterAgent = GetComponent<CharacterAgent>();
        currentState = new RandowmWalk();
        currentState.Enter(this, characterAgent);
    }

    void Update()
    {
        currentState.Execute();
    }

    public void ChangeState(Istate newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this, characterAgent);
    }

    public void RandomState()
    {
        int randomState = Random.Range(0, 5);

        if (randomState <= 2)
        {
            ChangeState(new RandowmWalk());
        }
        else if (randomState == 3)
        {
            ChangeState(new Idle());
        }
        else if (randomState == 4)
        {
            ChangeState(new PlayFlute());
        }
    }

    public void SetPlayerInfront()
    {
        if(!alreadyGreetPlayer)
        {
            alreadyGreetPlayer = true;

            ChangeState(new GreetPlayer());
        }
        else
        {
            ChangeState(new PlayerInfront());
        }
    }
}

/// <summary>
/// ///////STATES
/// </summary>
public class RandowmWalk : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    float timer;
    float stateDuration;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        stateDuration = Random.Range(2f,5f);

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.PlayWalkSound();
        characterAgent.WalkAnimation();
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        characterAgent.WalkAround();

        if(characterAgent.CheckPlayerInfront())
        {
            stateMachine.SetPlayerInfront();
        }

        if (timer > stateDuration)
        {
            stateMachine.RandomState();
        }
    }

    public void Exit()
    {
        characterAgent.StopOtherGoatSound();
        characterAgent.StopWalking();
    }
}

public class Idle : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;


    float timer;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.IdleSound();
        characterAgent.IdleAnimation();
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (timer > 3f)
        {
            stateMachine.RandomState();
        }
    }

    public void Exit()
    {
        characterAgent.StopGoatSound();
    }
}

public class PlayFlute : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    float timer;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.PlayFluteSound();
        characterAgent.PlayFluteAnimation();
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        characterAgent.PlayFlute();

        if (timer > 6f)
        {
            stateMachine.RandomState();
        }

        if (characterAgent.CheckPlayerInfront() && !stateMachine.alreadyGreetPlayer)
        {
            stateMachine.SetPlayerInfront();
        }
        else if(characterAgent.CheckPlayerInfront() && stateMachine.alreadyGreetPlayer)
        {
            stateMachine.ChangeState(new Distrubed());
        }

    }

    public void Exit()
    {
        characterAgent.StopOtherGoatSound();
        characterAgent.StopPlayFlute();
    }
}

public class PlayerInfront : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.PlayerInfrontAnimation();
        characterAgent.InfrontPlayerSound();
    }

    public void Execute()
    {
        if(!characterAgent.CheckPlayerInfront())
        {
            stateMachine.RandomState();
        }
    }

    public void Exit()
    {

    }
}

public class GreetPlayer : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.GreetPlayerAnimation();
        characterAgent.GreetPlayerSound();
    }

    public void Execute()
    {
        if (!characterAgent.CheckPlayerInfront())
        {
            stateMachine.RandomState();
        }
    }

    public void Exit()
    {

    }
}

public class Distrubed : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.DisturbedByPlayer();
    }

    public void Execute()
    {
        if (!characterAgent.CheckPlayerInfront())
        {
            stateMachine.RandomState();
        }
    }

    public void Exit()
    {

    }
}
