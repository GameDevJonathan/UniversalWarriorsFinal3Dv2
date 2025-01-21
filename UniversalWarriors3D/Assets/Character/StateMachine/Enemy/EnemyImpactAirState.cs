using UnityEngine;

public class EnemyImpactAirState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Air Impact");
    private const float CrossFadeDuration = 0.1f;
    public EnemyImpactAirState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entered Air Impact State");
        
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        stateMachine.ForceReceiver.Reset();
        stateMachine.ForceReceiver.useGravity = false;

    }
    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Hurt") > 1)
        {
            //stateMachine.SwitchState(new EnemyLaunchedState(stateMachine, 0f));
        }
    }

    public override void Exit()
    {
        stateMachine.ForceReceiver.Reset();
        stateMachine.ForceReceiver.useGravity = true;
        
    }

}
