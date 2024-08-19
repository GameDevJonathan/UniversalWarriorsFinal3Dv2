using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
    private readonly int BlockHash = Animator.StringToHash("Blocking");
    private const float CrossFadeDuration = 0.2f;
    bool stillTargeting;
    public PlayerBlockState(PlayerStateMachine stateMachine, bool stillTargeting = false) : base(stateMachine)
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
        this.stillTargeting = stillTargeting;

    }

    public override void Enter()
    {
        Debug.Log("Entered Block State");

    }

    public override void Tick(float deltaTime)
    {

        Debug.Log("still targeting: " + stillTargeting);

        if (stateMachine.InputReader.BlockButtonReleased)
        {
            if (stillTargeting)
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine, stillTargeting));
            else
                stateMachine.SwitchState(new Grounded(stateMachine, true));
        }

    }
    public override void Exit()
    {
        Debug.Log("Leaving Block State");
    }
}
