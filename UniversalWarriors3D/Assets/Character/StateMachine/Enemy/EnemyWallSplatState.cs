using UnityEngine;

public class EnemyWallSplatState : EnemyBaseState
{

    private readonly int WallSplatHash = Animator.StringToHash("WallSplat");
    private const float crossFadeTime = 0.1f;
    public EnemyWallSplatState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        if (stateMachine.Animator.applyRootMotion == false)
        {
            stateMachine.Animator.applyRootMotion = true;
        }
    }

    public override void Enter()
    {
        stateMachine.Animator.applyRootMotion = true;
        stateMachine.Animator.CrossFadeInFixedTime(WallSplatHash, crossFadeTime);

    }
    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "WallSplat") > 1)
        {
            stateMachine.SwitchState(new EnemyProneState(stateMachine, true));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.applyRootMotion = false;
        stateMachine.wallSplat = false;
    }



}
