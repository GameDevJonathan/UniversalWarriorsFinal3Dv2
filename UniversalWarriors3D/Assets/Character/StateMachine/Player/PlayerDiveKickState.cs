using AmplifyShaderEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiveKickState : PlayerBaseState
{
    Attacks attack;
    private readonly int DiveKickHash = Animator.StringToHash("DiveKick");
    private readonly int DiveKickEndHash = Animator.StringToHash("DiveKickEnd");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 Momentum;
    private bool alreadyAppliedForce = false;
    private bool grounded => stateMachine.WallRun.CheckForGround();


    public PlayerDiveKickState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DiveKickHash, CrossFadeDuration);
        //stateMachine.ForceReceiver.Jump(-stateMachine.DiveForce);
        attack = stateMachine.Attacks[5];
        stateMachine.ForceReceiver.useGravity = false;
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "DiveKick") > attack.TimeForce && attack.ShouldAddAttackForce)
        {
            Debug.Log("adding Force");
            TryApplyForce(-stateMachine.transform.up, stateMachine.transform.forward, deltaTime,
                stateMachine.DiveForce, stateMachine.ForwardForce, true);
        }        

        if (grounded && GetNormalizedTime(stateMachine.Animator,"DiveKick") > 1)
            stateMachine.Animator.CrossFadeInFixedTime(DiveKickEndHash, CrossFadeDuration);

        if (GetNormalizedTime(stateMachine.Animator, "DiveKickEnd") > 1f)
        {
            stateMachine.SwitchState(new Grounded(stateMachine, true));
        }
        if (alreadyAppliedForce)
        Move(deltaTime);
    }


    public override void Exit()
    {
        stateMachine.ForceReceiver.useGravity = true;
    }


    private void TryApplyForce(Vector3 UpwardDirection, Vector3 ForwardDirection, float deltaTime, float UpForce = 1, float ForwardForce = 1, bool continueForce = false)
    {
        if (alreadyAppliedForce && !continueForce) return;
        stateMachine.ForceReceiver.AddForce(((UpwardDirection * UpForce) + (ForwardDirection * ForwardForce)) * deltaTime );
        alreadyAppliedForce = true;

    }



}
