using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerState
{
    private PlayerManager player;
    public PlayerWalkingState(PlayerManager player, PlayerSO playerSO,PlayerMovement playerMovement,
        PlayerInputSystem playerInput, StateMachine<PlayerState> playerStateMachine, Animator playerAnimator)
        : base(player, playerSO, playerMovement, playerInput, playerStateMachine, playerAnimator)
    {
        this.player = player;
    }

    public override void AnimationTriggerEvent(string triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (!player.IsWalking)
            playerStateMachine.ChangeState(player.IdleState);
        
        playerMovement.direction = playerMovement.playerInput.direction;

        bool inputUp = playerInput.direction.x == 0 && playerInput.direction.y == 1;
        bool inputDown = playerInput.direction.x == 0 && playerInput.direction.y == - 1;

        bool inputLeft = playerInput.direction.x == - 1 && playerInput.direction.y == 0;
        bool inputRight = playerInput.direction.x == 1 && playerInput.direction.y == 0;


        if (inputUp)
            player.AnimationTriggerEvent("PlayerWalkUp");

        else if (inputDown)
            player.AnimationTriggerEvent("PlayerWalkDown");

        else if (inputRight)
            player.AnimationTriggerEvent("PlayerWalkRight");

        else if (inputLeft)
            player.AnimationTriggerEvent("PlayerWalkLeft");

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }
}
