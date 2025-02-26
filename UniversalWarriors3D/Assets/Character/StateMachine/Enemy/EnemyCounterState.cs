public class EnemyCounterState : EnemyBaseState
{
    Counters counter;
    public EnemyCounterState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        this.counter = stateMachine.Counters;
    }

    public override void Enter()
    {
        stateMachine.CharacterController.enabled = false;
        stateMachine.Animator.applyRootMotion = true;
        stateMachine.Animator.CrossFadeInFixedTime(counter.AnimationName, counter.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Counter") > 1f)
        {
            stateMachine.SwitchState(new EnemyProneState(stateMachine));
            return;
        }
    }


    public override void Exit()
    {
        stateMachine.CharacterController.enabled = true;
        stateMachine.Animator.applyRootMotion = false;

    }


}
