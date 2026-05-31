using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using UnityEngine;

public class PlayerAttackingState : PlayerState
{
    public PlayerAttackingState(PlayerManager player, PlayerSO playerSO, PlayerMovement playerMovement, PlayerInputSystem playerInput,
        StateMachine<PlayerState> playerStateMachine, Animator playerAnimator) 
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
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
