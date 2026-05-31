using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Windows;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerManager player, PlayerSO playerSO, PlayerMovement playerMovement, PlayerInputSystem playerInput, StateMachine<PlayerState> playerStateMachine, Animator playerAnimator)
                          : base(player, playerSO, playerMovement, playerInput, playerStateMachine, playerAnimator)
    {
    }

    public override void AnimationTriggerEvent(string triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        if (player.CurrentState == "PlayerWalkUp")
            player.AnimationTriggerEvent("PlayerIdleUp");

        else if (player.CurrentState == "PlayerWalkDown")
            player.AnimationTriggerEvent("PlayerIdleDown");

        else if (player.CurrentState == "PlayerWalkLeft")
            player.AnimationTriggerEvent("PlayerIdleLeft");

        else if (player.CurrentState == "PlayerWalkRight")
            player.AnimationTriggerEvent("PlayerIdleRight");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (player.IsWalking)
            playerStateMachine.ChangeState(player.WalkingState);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
