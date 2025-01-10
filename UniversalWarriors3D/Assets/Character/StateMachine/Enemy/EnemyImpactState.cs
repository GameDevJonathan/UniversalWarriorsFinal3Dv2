using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");
    private const float CrossFadeDuration = 0.1f;
    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator, "Hurt") > 1)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
        
    }

    public override void Exit()
    {
        
    }

}
