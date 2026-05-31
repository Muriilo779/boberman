using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using UnityEngine;

public class PlayerState : IState
{
    //Pretty much to handle animation 
    protected PlayerManager player;
    protected StateMachine<PlayerState> playerStateMachine;
    protected PlayerInputSystem playerInput;
    protected PlayerMovement playerMovement;
    protected Animator playerAnimator;
    protected string currentState;
    protected PlayerSO playerSO;

    public PlayerState(PlayerManager player, PlayerSO playerSO, PlayerMovement playerMovement, PlayerInputSystem playerInput, StateMachine<PlayerState> playerStateMachine, Animator playerAnimator)
    {
        this.playerSO = playerSO;
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.playerMovement = playerMovement;
        this.playerInput = playerInput;
        this.playerAnimator = playerAnimator;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(string newState) 
    {
        if (currentState == newState)
            return;

        playerAnimator.Play(newState);

        currentState = newState;
    }
}
