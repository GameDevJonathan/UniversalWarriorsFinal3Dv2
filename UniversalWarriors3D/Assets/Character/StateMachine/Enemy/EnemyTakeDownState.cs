using UnityEngine;

public class EnemyTakeDownState : EnemyBaseState
{
    TakeDowns takeDowns;
    public EnemyTakeDownState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        takeDowns = stateMachine.TakeDowns[1];
    }

    public override void Enter()
    {
        stateMachine.CharacterController.detectCollisions = false;
        stateMachine.CharacterController.enabled = false;
        stateMachine.Animator.applyRootMotion = true;
        stateMachine.Agent.enabled = false;
        FaceTakeDown();
        stateMachine.Animator.CrossFadeInFixedTime(takeDowns.AnimationName, takeDowns.TransitionDuration);
        
    }

    public override void Tick(float deltaTime)
    {
        

    }


    public override void Exit()
    {
        
    }

    
}
