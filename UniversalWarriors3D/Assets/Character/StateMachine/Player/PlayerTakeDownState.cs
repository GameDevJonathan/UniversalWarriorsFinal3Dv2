using UnityEngine;

public class PlayerTakeDownState : PlayerBaseState
{
    TakeDowns takedowns;
    int index;
    
    public PlayerTakeDownState(PlayerStateMachine stateMachine, int takeDownIndex) : base(stateMachine)
    {
        
        index = (!stateMachine.testTakeDown) ? takeDownIndex : 0;
        //index = takeDownIndex;
        
        
        takedowns = stateMachine.TakeDowns[index];
    }

    public override void Enter()
    {
        stateMachine.CharacterController.detectCollisions = false;
        stateMachine.CharacterController.enabled = false;
        stateMachine.Targeter.TakeDownTarget.TryGetComponent<Health>(out Health target);
        target.TakeDownReset();
        stateMachine.Targeter.TakeDownTarget.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine enemyStateMachine);        
        
        
        enemyStateMachine.CharacterController.enabled = false;
        //enemyStateMachine.Animator.applyRootMotion = true;
        enemyStateMachine.SwitchState(new EnemyTakeDownState(enemyStateMachine,index));
          
        
        stateMachine.Animator.applyRootMotion = true;
        Debug.Log("Entered TakeDown State");

        stateMachine.Animator.CrossFadeInFixedTime(takedowns.AnimationName, takedowns.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator,"TakeDown") > 1f)
        {
            stateMachine.SwitchState(new Grounded(stateMachine, true));
            return;
        }
        
    }

    public override void Exit()
    {
        stateMachine.CharacterController.enabled = true;
        
        stateMachine.CharacterController.detectCollisions = true;
        stateMachine.Animator.applyRootMotion = false;
        
    }


    
}
