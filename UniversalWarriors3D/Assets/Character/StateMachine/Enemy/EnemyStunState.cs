using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
    private readonly int DizzyHash = Animator.StringToHash("Dizzy");    
    private const float crossFadeTime = 0.1f;
    

    public EnemyStunState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DizzyHash, crossFadeTime);
        Debug.Log("Dizzy State:: I am dizzy entered");
        
    }

    public override void Tick(float deltaTime)
    {
        if (!stateMachine.Health.isStunned)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        
    }

    
}
