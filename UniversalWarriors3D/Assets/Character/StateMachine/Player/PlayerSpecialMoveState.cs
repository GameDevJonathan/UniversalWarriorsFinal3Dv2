public class PlayerSpecialMoveState : PlayerBaseState
{
    SpecialMoves specialMove;
    int moveIndex;
    public PlayerSpecialMoveState(PlayerStateMachine stateMachine, int moveIndex) : base(stateMachine)
    {
        this.specialMove = stateMachine.SpecialMoves[moveIndex];

    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(specialMove.AnimationName, specialMove.TransitionDuration);

    }
    public override void Tick(float deltaTime)
    {


        if (GetNormalizedTime(stateMachine.Animator, "SpecialMove") > 1f)
        {
            stateMachine.SwitchState(new Grounded(stateMachine, true));
            return;
        }

    }

    public override void Exit()
    {
    }

}
