using UnityEngine;

public interface IFarmerstate
{
    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent);
    public void Execute();
    public void Exit();

}

public class StateMachineTwo : MonoBehaviour
{
    public IFarmerstate currentState;

    FarmerAgent farmerAgent;

    public GameObject rayObject;

    public bool alreadyGreetPlayer = false;
    public bool busy = true;

    public float delayTimer;

    public int disturbedCountdown = 2;

    void Start()
    {
        farmerAgent = GetComponent<FarmerAgent>();
        currentState = new FarmerRandomWalk();
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
            ChangeState(new FarmerRandomWalk());
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
            ChangeState(new FarmerRandomWalk());
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
}

#region randomIdlingStates (walk,Idle,mop)
public class FarmerRandomWalk : IFarmerstate
{
    StateMachineTwo stateMachineTwo;
    FarmerAgent farmerAgent;

    float timer;
    float stateDuration;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        if (stateMachineTwo.delayTimer > 10f)
        {
            stateMachineTwo.busy = false;
            stateMachineTwo.delayTimer = 0f;
        }

        stateDuration = Random.Range(2f, 5f);

        this.stateMachineTwo = stateMachineTwo;
        this.farmerAgent = farmerAgent;

        NeighbourInteraction.interactNeigbour += EnterNeighbourTrigger;
        MopSpot.sweep += EnterMopTrigger;

        farmerAgent.PlayWalkSound();
        farmerAgent.WalkAnimation();
    }

    void EnterNeighbourTrigger()
    {
        stateMachineTwo.ChangeState(new WalkTowardGoat());
    }

    void EnterMopTrigger()
    {
        stateMachineTwo.ChangeState(new Mop());
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        stateMachineTwo.delayTimer += Time.deltaTime;

        farmerAgent.WalkAround();

        if (farmerAgent.CheckPlayerInfront())
        {
            stateMachineTwo.SetPlayerInfront();
        }

        if (timer > stateDuration)
        {
            stateMachineTwo.FarmerRandomState();
        }
    }

    public void Exit()
    {
        NeighbourInteraction.interactNeigbour -= EnterNeighbourTrigger;
        MopSpot.sweep -= EnterMopTrigger;
        stateMachineTwo.busy = true;
        farmerAgent.StopOtherFarmerSound();
        farmerAgent.StopWalking();
    }
}

public class FarmerIdle : IFarmerstate
{
    StateMachineTwo stateMachineTwo;
    FarmerAgent farmerAgent;

    float timer;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachineTwo = stateMachineTwo;
        this.farmerAgent = farmerAgent;

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
            stateMachineTwo.FarmerRandomState();
        }

        if (farmerAgent.CheckPlayerInfront())
        {
            stateMachineTwo.SetPlayerInfront();
        }
    }

    public void Exit()
    {
        farmerAgent.StopFarmerSound();
    }
}


public class Mop : IFarmerstate
{
    StateMachineTwo stateMachineTwo;
    FarmerAgent farmerAgent;

    float timer;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachineTwo = stateMachineTwo;
        this.farmerAgent = farmerAgent;

        farmerAgent.MopSound();
        farmerAgent.MopAnimation();

        farmerAgent.UseMop();
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (timer > 6f)
        {
            stateMachineTwo.RandomState();
        }
        if (farmerAgent.CheckPlayerInfront() && !stateMachineTwo.alreadyGreetPlayer)
        {
            stateMachineTwo.SetPlayerInfront();
        }
        else if (farmerAgent.CheckPlayerInfront() && stateMachineTwo.alreadyGreetPlayer && stateMachineTwo.disturbedCountdown == 2)
        {
            stateMachineTwo.ChangeState(new FarmerDistrubed());
        }
        else if (farmerAgent.CheckPlayerInfront() && stateMachineTwo.alreadyGreetPlayer)
        {
            stateMachineTwo.disturbedCountdown--;
            stateMachineTwo.SetPlayerInfront();
        }
    }

    public void Exit()
    {
        farmerAgent.StopOtherFarmerSound();
        farmerAgent.DismissMop();
    }
}

#endregion


#region InteractPlayerStates
public class FarmerPlayerInfront : IFarmerstate
{
    StateMachineTwo stateMachine;
    FarmerAgent farmerAgent;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachine = stateMachineTwo;
        this.farmerAgent = farmerAgent;

        int randomAnimation = Random.Range(0, 3);

        if (randomAnimation == 0)
        {
            farmerAgent.PlayerInfrontAnimation();
        }
        else if (randomAnimation == 1)
        {
            farmerAgent.WhatsYouUpToAnimation();
            farmerAgent.DoYourWantSomething();
        }
        else if (randomAnimation == 2)
        {
            farmerAgent.WhatsYouUpToAnimation();
            farmerAgent.WatchYourBack();
        }
    }

    public void Execute()
    {
        if (!farmerAgent.CheckPlayerInfront())
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
    FarmerAgent farmerAgent;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachine = stateMachineTwo;
        this.farmerAgent = farmerAgent;

        farmerAgent.GreetPlayerAnimation();
        farmerAgent.GreetPlayerSound();
    }

    public void Execute()
    {
        if (!farmerAgent.CheckPlayerInfront())
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
    StateMachineTwo stateMachineTwo;
    FarmerAgent farmerAgent;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachineTwo = stateMachineTwo;
        this.farmerAgent = farmerAgent;

        int randomAnimation = Random.Range(0, 2);

        if (randomAnimation == 0)
        {
            farmerAgent.DisturbedAnimation();
            farmerAgent.DisturbedSound();
        }
        else if (randomAnimation == 1)
        {
            farmerAgent.DisturbedAnimation();
            farmerAgent.BetterBeImportant();
        }

        if (stateMachineTwo.disturbedCountdown == 0)
        {
            stateMachineTwo.disturbedCountdown = 2;
        }
    }

    public void Execute()
    {
        if (!farmerAgent.CheckPlayerInfront())
        {
            stateMachineTwo.delayTimer += Time.deltaTime;
            if (stateMachineTwo.delayTimer > 2f)
            {
                stateMachineTwo.delayTimer = 0f;
                stateMachineTwo.FarmerRandomState();
            }
        }
    }

    public void Exit()
    {

    }
}
#endregion

#region Neighbor
public class WalkTowardGoat : IFarmerstate
{
    StateMachineTwo stateMachineTwo;
    FarmerAgent farmerAgent;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachineTwo = stateMachineTwo;
        this.farmerAgent = farmerAgent;

        farmerAgent.PlayWalkSound();
        farmerAgent.WalkAnimation();
    }

    public void Execute()
    {
        farmerAgent.ChangeDestinationGoat();

        if (Vector3.Distance(farmerAgent.character.transform.position, farmerAgent.goatDestination.transform.position) < 1f)
        {
            stateMachineTwo.ChangeState(new TalkToGoat());
        }
    }

    public void Exit()
    {

    }
}


public class TalkToGoat : IFarmerstate
{
    StateMachineTwo stateMachineTwo;
    FarmerAgent farmerAgent;

    float timer;

    public void Enter(StateMachineTwo stateMachineTwo, FarmerAgent farmerAgent)
    {
        farmerAgent.ResetAgent();

        this.stateMachineTwo = stateMachineTwo;
        this.farmerAgent = farmerAgent;

        farmerAgent.StopOtherFarmerSound();

        farmerAgent.RotateTowardsNeighbour();

        farmerAgent.TalkToNeighborAnimation();
        farmerAgent.TalkToNeighborSound();
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (timer > 6f)
        {
            stateMachineTwo.ChangeState(new FarmerRandomWalk());
        }
    }

    public void Exit()
    {
        farmerAgent.ResetDestination();
        farmerAgent.ChangeDestination();
    }
}

#endregion