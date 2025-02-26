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
        stateMachine.Health.CallHitStop();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        FacePlayer();
        if (GetNormalizedTime(stateMachine.Animator, "Hurt") >= .4f
            && stateMachine.Health.isStunned)
        {
            stateMachine.SwitchState(new EnemyStunState(stateMachine));
            return;
        }


        if (GetNormalizedTime(stateMachine.Animator, "Hurt") > 1)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

    }

    public override void Exit()
    {

    }

}
