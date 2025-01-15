using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeJumpState : PlayerBaseState
{
    private readonly int LedgeJumpStartHash = Animator.StringToHash("LedgeJumpStart");
    private const float CrossFadeDuration = 0.2f;
    private Vector3 Momentum;
    private float fallTime = 0f;
    private float fallTimeRate = 1f;
    private bool alreadyAppliedForce;

    public PlayerLedgeJumpState(PlayerStateMachine stateMachine, float height) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        //stateMachine.Animator.applyRootMotion = true;
        Momentum = stateMachine.CharacterController.velocity;
        //Momentum.y = 0f;
        stateMachine.Animator.CrossFadeInFixedTime(LedgeJumpStartHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        fallTime += fallTimeRate * Time.deltaTime;


        if( GetNormalizedTime(stateMachine.Animator,"LedgeJump") > .11f && !alreadyAppliedForce)
        {
            Debug.Log("jumped ledge");
            //stateMachine.ForceReceiver.Jump(10f);
            //alreadyAppliedForce = true;
            TryApplyForce(stateMachine.transform.up, stateMachine.transform.forward, 40f, Momentum.z);
        }
        Move(Momentum,deltaTime);

        if(GetNormalizedTime(stateMachine.Animator,"LedgeJumpLoop") > 1f)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine, true));
        }


        //if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsTag("LedgeJump"))
        //{
        //    if (stateMachine.Animator.applyRootMotion)
        //    {
        //        stateMachine.Animator.applyRootMotion = !stateMachine.Animator.applyRootMotion;
        //        stateMachine.ForceReceiver.Reset();
        //    }

        //    //Move(Momentum, deltaTime);           
        //    if (stateMachine.CharacterController.isGrounded)
        //    {
        //        stateMachine.ForceReceiver.Jump(10f);


        //    }

        //    if (movement != Vector3.zero)
        //        FaceMovement(movement, deltaTime);
        //}

        //if (stateMachine.CharacterController.isGrounded && stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsTag("LedgeJumpLand"))
        //{
        //    stateMachine.SwitchState(new PlayerLandState(stateMachine, movement, fallTime));
        //}

    }

    public override void Exit()
    {

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

    private void TryApplyForce(Vector3 UpwardDirection, Vector3 ForwardDirection, float UpForce = 1, float ForwardForce = 1)
    {
        if (alreadyAppliedForce) return;
        stateMachine.ForceReceiver.AddForce((UpwardDirection * UpForce) + (ForwardDirection * ForwardForce));
        alreadyAppliedForce = true;

    }


}
