using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");
    private const float CrossFadeDuration = 0.1f;
    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Hurt") > 1)
        {

            if (stateMachine.Targeter.CurrentTarget != null)
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            else
                stateMachine.SwitchState(new Grounded(stateMachine));
        }
    }

    public override void Exit()
    {

    }
}
