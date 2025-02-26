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


    }

    public override void Tick(float deltaTime)
    {
        stateMachine.ParryTime = Mathf.Clamp(stateMachine.ParryTime, 0f, 1f);
        stateMachine.ParryTime = GetNormalizedTime(stateMachine.Animator, "Block");


        Debug.Log("still targeting: " + stillTargeting);

        if (stateMachine.InputReader.BlockButtonReleased)
        {
            if (stillTargeting)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine, stillTargeting));
                return;

            }
            else
            {
                stateMachine.SwitchState(new Grounded(stateMachine, true));
                return;
            }
        }

    }
    public override void Exit()
    {
        stateMachine.ParryTime = 0;
        Debug.Log("Leaving Block State");
    }
}
