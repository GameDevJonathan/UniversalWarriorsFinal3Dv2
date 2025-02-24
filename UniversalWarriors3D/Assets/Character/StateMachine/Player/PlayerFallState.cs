using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("JumpLoop");
    private const float CrossFadeDuration = 0.3f;
    private Vector3 Momentum;
    private float fallTime = 0f;
    private float fallTimeRate = 1f;
    private bool animChange;


    public PlayerFallState(PlayerStateMachine stateMachine, bool animationChange = false) : base(stateMachine)
    {
        this.animChange = animationChange;
    }

    public override void Enter()
    {
        //Debug.Log("Entered fallState");
        stateMachine.InputReader.isDashing = false;
        Momentum = stateMachine.CharacterController.velocity;
        Momentum.y = 0f;
        
        if(!animChange)
        stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);


        stateMachine.InputReader.HeavyMeleeEvent += HeavyMeleeEvent;

    }

    public override void Tick(float deltaTime)
    {
        //Debug.Log($"Fall State State::{stateMachine.WallRun.CheckForGround()}");
        Vector3 movement = CalculateMovement();

        //Move(Momentum, deltaTime);
        Move(movement + Momentum, deltaTime);

        if (movement != Vector3.zero)
            FaceMovement(movement, deltaTime);



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


        fallTime += fallTimeRate * Time.deltaTime;
        //Debug.Log($"FallTime:{fallTime}");
       

        if (stateMachine.CharacterController.isGrounded)
        {
            stateMachine.SwitchState(new PlayerLandState(stateMachine, movement, fallTime));
        }
    }

    public override void Exit()
    {
        stateMachine.InputReader.HeavyMeleeEvent -= HeavyMeleeEvent;
        stateMachine.MeshTrail.isTrailActive = false;
    }


    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();



        return forward * stateMachine.InputReader.MovementValue.y +
               right * stateMachine.InputReader.MovementValue.x;
    }

    private void HeavyMeleeEvent()
    {
        stateMachine.EquipTime = 10f;
        stateMachine.ForceReceiver.Reset();
        stateMachine.Targeter.SelectClosestTarget();
        stateMachine.SwitchState(new PlayerDiveKickState(stateMachine));
    }


}
