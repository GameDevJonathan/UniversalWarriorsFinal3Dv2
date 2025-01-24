using UnityEngine;

public class PlayerTakeDownState : PlayerBaseState
{
    TakeDowns takedowns;
    public PlayerTakeDownState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        takedowns = stateMachine.TakeDowns[1];
    }

    public override void Enter()
    {
        stateMachine.CharacterController.detectCollisions = false;
        stateMachine.Animator.applyRootMotion = true;
        Debug.Log("Entered TakeDown State");
        stateMachine.Targeter.TakeDownTarget.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine enemyStateMachine);        
        enemyStateMachine.SwitchState(new EnemyTakeDownState(enemyStateMachine));
        
        stateMachine.Targeter.TakeDownTarget.TryGetComponent<Health>(out Health target);
        target.TakeDownReset();
        stateMachine.Animator.CrossFadeInFixedTime(takedowns.AnimationName, takedowns.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator,"TakeDown") > 1f)
        {
            stateMachine.SwitchState(new Grounded(stateMachine, true));
        }
        
    }

    public override void Exit()
    {
        stateMachine.CharacterController.detectCollisions = true;
        stateMachine.Animator.applyRootMotion = false;
        
    }


    
}
