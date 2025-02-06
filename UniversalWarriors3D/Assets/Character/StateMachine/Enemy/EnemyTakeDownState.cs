using UnityEngine;

public class EnemyTakeDownState : EnemyBaseState
{
    TakeDowns takeDowns;
    private Vector3 direction;
    private float launchForce;

    public EnemyTakeDownState(EnemyStateMachine stateMachine, int takeDownIndex) : base(stateMachine)
    {
        takeDowns = stateMachine.TakeDowns[takeDownIndex];
    }

    public override void Enter()
    {
        //character controller is turned of in the player take down state

        FaceTakeDown();
        stateMachine.Animator.applyRootMotion = true;
        stateMachine.Animator.CrossFadeInFixedTime(takeDowns.AnimationName, takeDowns.TransitionDuration);

    }

    public override void Tick(float deltaTime)
    {



        ////Move(deltaTime);
        if ((takeDowns.AnimationName == "Rapid Combo Damage") &&
            (GetNormalizedTime(stateMachine.Animator, "TakeDown") > .6f))
        {
            stateMachine.CharacterController.enabled = true;
        }


        if (GetNormalizedTime(stateMachine.Animator, "TakeDown") > 1)
        {
            stateMachine.SwitchState(new EnemyProneState(stateMachine));
            return;
        }

    }


    public override void Exit()
    {

        stateMachine.CharacterController.enabled = true;
        stateMachine.Animator.applyRootMotion = false;
    }








}
