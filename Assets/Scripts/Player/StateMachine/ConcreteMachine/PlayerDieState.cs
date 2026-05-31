using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(PlayerManager player, PlayerSO playerSO, PlayerMovement playerMovement, PlayerInputSystem playerInput,
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
        
        playerMovement.moveSpeed = 0;
        player.AnimationTriggerEvent("PlayerDying");

        // Notify the server that this player is out
        if (!player.IsOwner)
            return;
        if (ManageRounds.Instance != null)
        {
            ManageRounds.Instance.PlayerDiedServerRpc();
        }
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
