using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack_Left_Hook");
    private const float TransitionDuration = 0.1f;
    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash,TransitionDuration);
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage);
    }
    public override void Tick(float deltaTime)
    {

        if(GetNormalizedTime(stateMachine.Animator,"Attack") >= 1)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }


}
