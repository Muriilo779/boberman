using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerState
{
    private PlayerManager player;
    public FacingDirection FacingDir { get; private set; }
    public bool IsMoonwalking { get; private set; }
    
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
        {
            IsMoonwalking = false; 
            playerStateMachine.ChangeState(player.IdleState);
            return;
        }

        if (IsMoonwalking && ValidateMoonwalk())
            return;
        
        playerMovement.direction = playerMovement.playerInput.direction;

        // If I press two directions at the same time, none of these booleans will be true. 
        // This allow the "moonwalk"
        bool inputUp = playerInput.direction is { x: 0, y: 1 }
            || FacingDir == FacingDirection.Up && playerMovement.direction.y > 0;
        
        bool inputDown = playerInput.direction is { x: 0, y: -1 }
            || FacingDir == FacingDirection.Down && playerMovement.direction.y < 0;

        bool inputLeft = playerInput.direction is { x: -1, y: 0 }
                         || FacingDir == FacingDirection.Left && playerMovement.direction.x < 0;
        
        bool inputRight = playerInput.direction is { x: 1, y: 0 }
            || FacingDir == FacingDirection.Right && playerMovement.direction.x > 0;

        if (inputUp)
        {
            player.AnimationTriggerEvent("PlayerWalkUp");
            FacingDir = FacingDirection.Up;
            IsMoonwalking = false;
        }
        else if (inputDown)
        {
            player.AnimationTriggerEvent("PlayerWalkDown");
            FacingDir = FacingDirection.Down;
            IsMoonwalking = false;
        }
        else if (inputRight)
        {
            player.AnimationTriggerEvent("PlayerWalkRight");
            FacingDir = FacingDirection.Right;
            IsMoonwalking = false;
        }
        else if (inputLeft)
        {
            player.AnimationTriggerEvent("PlayerWalkLeft");
            FacingDir = FacingDirection.Left;
            IsMoonwalking = false;
        }
        else
        {
            IsMoonwalking = true;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

    private bool ValidateMoonwalk()
    {
        bool facingLeft = FacingDir == FacingDirection.Left && playerInput.direction.x < 0;
        bool facingRight = FacingDir == FacingDirection.Right && playerInput.direction.x > 0;
        
        bool facingUp = FacingDir == FacingDirection.Up && playerInput.direction.y > 0;
        bool facingDown = FacingDir == FacingDirection.Down && playerInput.direction.y < 0;
        
        return !(facingLeft || facingRight || facingUp || facingDown);
    }
}
public enum FacingDirection {
    Up,
    Down,
    Left,
    Right
}
