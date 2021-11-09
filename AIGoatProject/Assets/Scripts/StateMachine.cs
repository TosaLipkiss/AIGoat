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
    public bool busy = true;

    public float delayTimer;

    public int disturbedCountdown = 2;



    void Start()
    {
        characterAgent = GetComponent<CharacterAgent>();
        currentState = new RandomWalk();
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
            ChangeState(new RandomWalk());
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
        if (!alreadyGreetPlayer)
        {
            alreadyGreetPlayer = true;

            ChangeState(new GreetPlayer());
        }
        else
        {
            ChangeState(new PlayerInfront());
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


/// ///////STATES///////////

#region randomIdlingStates (walk,Idle,Flute)
public class RandomWalk : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    float timer;
    float stateDuration;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();
        stateMachine.busy = false;

        stateDuration = Random.Range(2f, 5f);

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        BirdHouse.feed += EnterBirdHouseTrigger;
        FindMushrooms.pick += FindClosestMushroom;
        NeighbourInteraction.interactNeigbour += EnterNeighbourTrigger;

        characterAgent.PlayWalkSound();
        characterAgent.WalkAnimation();
    }

    void EnterBirdHouseTrigger()
    {
        stateMachine.ChangeState(new WalkTowardBirdHouse());
    }

    void FindClosestMushroom()
    {
        stateMachine.ChangeState(new WalkTowardMushroom());
    }

    void EnterNeighbourTrigger()
    {
        stateMachine.ChangeState(new WalkTowardFarmer());
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
            stateMachine.RandomState();
        }
    }

    public void Exit()
    {
        FindMushrooms.pick -= FindClosestMushroom;
        BirdHouse.feed -= EnterBirdHouseTrigger;
        NeighbourInteraction.interactNeigbour -= EnterNeighbourTrigger;

        stateMachine.busy = true;
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

        int randomAnimation = Random.Range(0, 8);

        if (randomAnimation <= 2)
        {
            characterAgent.IdleAnimation();
        }
        else if (randomAnimation == 3)
        {
            characterAgent.IdleGaspAnimation();
            characterAgent.IdleGaspSound();
        }
        else if (randomAnimation >= 4 || randomAnimation <= 6)
        {
            characterAgent.IdleLookAroundpAnimation();
        }
        else if (randomAnimation == 7)
        {
            characterAgent.IdleHmmAnimation();
        }
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (timer > 3f)
        {
            stateMachine.RandomState();
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
        else if (characterAgent.CheckPlayerInfront() && stateMachine.alreadyGreetPlayer && stateMachine.disturbedCountdown == 0)
        {
            stateMachine.ChangeState(new Distrubed());
        }
        else if (characterAgent.CheckPlayerInfront() && stateMachine.alreadyGreetPlayer)
        {
            stateMachine.disturbedCountdown--;
            stateMachine.SetPlayerInfront();
        }
    }

    public void Exit()
    {
        characterAgent.StopOtherGoatSound();
        characterAgent.StopPlayFlute();
    }
}

#endregion

#region InteractPlayerStates
public class PlayerInfront : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        int randomAnimation = Random.Range(0, 3);

        if (randomAnimation == 0)
        {
            characterAgent.PlayerInfrontAnimation();
        }
        else if (randomAnimation == 1)
        {
            characterAgent.WhatsYouUpToAnimation();
            characterAgent.WhatYouUpToSound();
        }
        else if(randomAnimation == 2)
        {
            characterAgent.YouAreTallAnimation();
            characterAgent.YouAreTallSound();
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
                stateMachine.RandomState();
            }
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
            stateMachine.delayTimer += Time.deltaTime;
            if (stateMachine.delayTimer > 2f)
            {
                stateMachine.delayTimer = 0f;
                stateMachine.RandomState();
            }
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

        int randomAnimation = Random.Range(0, 2);

        if (randomAnimation == 0)
        {
            characterAgent.DisturbedAnimation();
            characterAgent.DisturbedSound();
        }
        else if (randomAnimation == 1)
        {
            characterAgent.DisturbedReallyAnimation();
            characterAgent.DisturbedReallySound();
        }

        if (stateMachine.disturbedCountdown == 0)
        {
            stateMachine.disturbedCountdown = 2;
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
                stateMachine.RandomState();
            }
        }
    }

    public void Exit()
    {

    }
}
#endregion

public class WalkTowardFarmer : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.PlayWalkSound();
        characterAgent.WalkAnimation();
    }

    public void Execute()
    {
        characterAgent.ChangeDestinationFarmer();

        if (Vector3.Distance(characterAgent.character.transform.position, characterAgent.farmerDestination.transform.position) < 1f)
        {
            stateMachine.ChangeState(new TalkToFarmer());
        }
    }

    public void Exit()
    {

    }
}


public class TalkToFarmer : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    float timer;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.StopOtherGoatSound();
        characterAgent.RotateTowardsNeighbour();

        characterAgent.GreetPlayerAnimation();
        characterAgent.GreetPlayerSound();
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (timer > 3f)
        {
            stateMachine.ChangeState(new RandomWalk());
        }
    }

    public void Exit()
    {
        characterAgent.ResetDestination();
        characterAgent.ChangeDestination();
    }
}

#region BirdHouseState
public class WalkTowardBirdHouse : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        Debug.Log("Walk towards bird");
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.PlayWalkSound();
        characterAgent.WalkAnimation();

        characterAgent.ChangeDestinationBirdHouse();
    }

    public void Execute()
    {
        if (Vector3.Distance(characterAgent.character.transform.position, characterAgent.birdHouseDestination.transform.position) < 0.5f)
        {
            stateMachine.ChangeState(new FeedingTheBirds());
        }
    }

    public void Exit()
    {

    }
}

public class FeedingTheBirds : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    float timer;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.StopOtherGoatSound();

        characterAgent.BagSound();
        characterAgent.FeedBirdAnimation();

        characterAgent.FeedingBirds(); 
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        characterAgent.FeedingBirds();

        if(timer > 1f && !characterAgent.voiceOnCooldown)
        {
            characterAgent.PerfectSound();
        }

        if (timer > 2.7f)
        {
            stateMachine.ChangeState(new RandomWalk());
        }
    }

    public void Exit()
    {
        characterAgent.ResetDestination();
        characterAgent.voiceOnCooldown = false;
    }
}
#endregion

#region MushroomState
public class WalkTowardMushroom : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.PlayWalkSound();
        characterAgent.WalkAnimation();

        characterAgent.ChangeDestinationMushroom();
    }

    public void Execute()
    {
        if (Vector3.Distance(characterAgent.character.transform.position, characterAgent.destination.transform.position) < 0.5f)
        {
            stateMachine.ChangeState(new PickUpMushroom());
        }
    }

    public void Exit()
    {

    }
}


public class PickUpMushroom : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    float timer;
    bool inventoryIsFull = false;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();
        characterAgent.StopOtherGoatSound();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.CheckInventoryStorage();

        characterAgent.BagSound();
        characterAgent.PickMushroomAnimation();
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        characterAgent.AddMushroomInInventory();
        characterAgent.CheckInventoryStorage();

        if(timer > 1.5f)
        {
            characterAgent.PutMushroomInBag();
        }

        if (timer > 3.5f)
        {
            characterAgent.DisableMushroomInHand();
        }

        if (!inventoryIsFull && timer > 4f)
        {
            stateMachine.ChangeState(new RandomWalk());
        }

        if(characterAgent.inventory == 5)  //ändra här och en till plats för att ändra maxantal
        {
            inventoryIsFull = true;

            if(inventoryIsFull && timer > 4f)
            {
                Debug.Log("inventory full");
                stateMachine.ChangeState(new WalkTowardHome());
            }
        }

    }

    public void Exit()
    {
        inventoryIsFull = false;
        characterAgent.ResetDestination();
    }
}

public class WalkTowardHome : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {
        characterAgent.ResetAgent();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.ChangeDestinationHome();

        Home.emptyPockets += EnterHomeTrigger;

        characterAgent.PlayWalkSound();
        characterAgent.WalkAnimation();
    }

    public void Execute()
    {

    }
    void EnterHomeTrigger()
    {
        stateMachine.ChangeState(new EmptyInventory());
    }

    public void Exit()
    {
        Home.emptyPockets -= EnterHomeTrigger;
    }
}

public class EmptyInventory : Istate
{
    StateMachine stateMachine;
    CharacterAgent characterAgent;

    float timer;

    bool mushroomsIsInHands = false;

    public void Enter(StateMachine stateMachine, CharacterAgent characterAgent)
    {

        characterAgent.ResetAgent();
        characterAgent.StopOtherGoatSound();

        this.stateMachine = stateMachine;
        this.characterAgent = characterAgent;

        characterAgent.BagSound();
        characterAgent.EmptyPocketsAnimation();
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        characterAgent.FaceTowardChest();

        if (timer > 1f && !mushroomsIsInHands)
        {
            characterAgent.MushroomStackInHand();
            mushroomsIsInHands = true;
        }

        else if(timer > 2.8f)
        {
            characterAgent.DisableMushroomStackInHand();
        }

        if (timer > 4f)
        {
            characterAgent.EmptyInventory();
            stateMachine.ChangeState(new RandomWalk());
        }
    }


    public void Exit()
    {
        characterAgent.timerFulfilled = false;
        characterAgent.ResetDestination();
    }
}

#endregion MushroomState
