public class PlayerCounterState : PlayerBaseState
{
    Counters counter;
    public PlayerCounterState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.counter = stateMachine.Counters;

    }

    public override void Enter()
    {
        stateMachine.Animator.applyRootMotion = true;
        stateMachine.Animator.CrossFadeInFixedTime(counter.AnimationName, counter.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Counter") > 1f)
        {
            stateMachine.SwitchState(new Grounded(stateMachine, true));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.applyRootMotion = false;

    }

}
