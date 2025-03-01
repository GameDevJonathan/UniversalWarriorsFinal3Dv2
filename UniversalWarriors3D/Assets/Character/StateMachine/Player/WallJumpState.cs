using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : PlayerBaseState
{
    private readonly int WallJumpHash = Animator.StringToHash("WallJump");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 Momentum;
    private Vector3 JumpVector;

    public WallJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered WallJump State");
        stateMachine.ForceReceiver.Jump(stateMachine.WallRun.wallJumpForce);
        Momentum = stateMachine.CharacterController.velocity;
        Momentum.y = 0f;
        JumpVector = stateMachine.WallRun.WallJump();
        stateMachine.Animator.CrossFadeInFixedTime(WallJumpHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(JumpVector + Momentum,deltaTime);

        if (stateMachine.WallRun.AboveGround() && stateMachine.WallRun.HitWall())
        {
            Debug.Log(stateMachine.WallRun.HitWall());
            //Debug.Log(stateMachine.WallRun.AboveGround());

            if (stateMachine.InputReader.MovementValue.y > 0)
            {
                Debug.Log("Can Enter Wall Run State");
                stateMachine.SwitchState(new PlayerWallRunning(stateMachine));
                return;
            }
        }


        if (stateMachine.WallRun.AboveGround() && stateMachine.WallRun.HItWallForward())
        {
            Debug.Log(stateMachine.WallRun.HItWallForward());
            //Debug.Log(stateMachine.WallRun.AboveGround());

            if (stateMachine.InputReader.MovementValue.y > 0)
            {
                Debug.Log("Can Enter Wall Run State");
                stateMachine.SwitchState(new PlayerWallHang(stateMachine));
                return;
            }
        }






        if (stateMachine.CharacterController.velocity.y <= 0f)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
            return;
        }

    }

    public override void Exit()
    {
        
    }

    

    
}
